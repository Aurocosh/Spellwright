using Microsoft.Xna.Framework;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.Extensions
{
    internal static class PlayerExtensions
    {
        public static Rectangle GetAreaRect(this Player player, int radius)
        {
            var playerCenter = player.Center;
            return new Rectangle((int)playerCenter.X - radius, (int)playerCenter.Y - radius, player.width + radius * 2, player.height + radius * 2);
        }
        public static Rectangle GetAreaRect(this Player player, int widthHalf, int heightHalf)
        {
            var playerCenter = player.Center;
            return new Rectangle((int)playerCenter.X - widthHalf, (int)playerCenter.Y - heightHalf, player.width + widthHalf * 2, player.height + heightHalf * 2);
        }

        public static IEnumerable<int> GetInventoryIndexes(this Player player, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            if (!reverseOrder)
            {
                if (includedParts.HasFlag(InventoryArea.Hotbar))
                    for (int i = 0; i < 10; i++)
                        yield return i;
                if (includedParts.HasFlag(InventoryArea.Inventory))
                    for (var i = 10; i < 50; i++)
                        yield return i;
                if (includedParts.HasFlag(InventoryArea.Coins))
                    for (var i = 50; i < 54; i++)
                        yield return i;
                if (includedParts.HasFlag(InventoryArea.Ammo))
                    for (var i = 54; i < 58; i++)
                        yield return i;
            }
            else
            {
                if (includedParts.HasFlag(InventoryArea.Ammo))
                    for (var i = 57; i >= 54; i--)
                        yield return i;
                if (includedParts.HasFlag(InventoryArea.Coins))
                    for (var i = 53; i >= 50; i--)
                        yield return i;
                if (includedParts.HasFlag(InventoryArea.Inventory))
                    for (var i = 49; i >= 10; i--)
                        yield return i;
                if (includedParts.HasFlag(InventoryArea.Hotbar))
                    for (var i = 9; i >= 0; i--)
                        yield return i;
            }
        }

        public static IEnumerable<Item> GetInventoryItems(this Player player, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            return GetInventoryIndexes(player, includedParts, reverseOrder).Select(i => player.inventory[i]);
        }

        public static Item InventoryFindItem(this Player player, int itemTypeId, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            var items = GetInventoryItems(player, includedParts, reverseOrder);
            return UtilInventory.InventoryFindItem(items, itemTypeId);
        }

        public static Item InventoryFindItem(this Player player, Func<Item, bool> filter, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            var items = GetInventoryItems(player, includedParts, reverseOrder);
            return UtilInventory.InventoryFindItem(items, filter);
        }

        public static bool ConsumeItems(this Player player, int itemTypeId, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            var items = GetInventoryItems(player, includedParts, reverseOrder);
            return UtilInventory.ConsumeItems(items, itemTypeId, amount);
        }

        public static bool ConsumeItems(this Player player, Func<Item, bool> filter, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            var items = GetInventoryItems(player, includedParts, reverseOrder);
            return UtilInventory.ConsumeItems(items, filter, amount);
        }

        public static bool HasItems(this Player player, int itemTypeId, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            var items = GetInventoryItems(player, includedParts, reverseOrder);
            return UtilInventory.HasItems(items, itemTypeId, amount);
        }

        public static bool HasItems(this Player player, Func<Item, bool> filter, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            var items = GetInventoryItems(player, includedParts, reverseOrder);
            return UtilInventory.HasItems(items, filter, amount);
        }

        public static int CountItems(this Player player, int itemTypeId, InventoryArea includedParts = InventoryArea.All)
        {
            var items = GetInventoryItems(player, includedParts);
            return UtilInventory.CountItems(items, itemTypeId);
        }

        public static int CountItems(this Player player, Func<Item, bool> filter, InventoryArea includedParts = InventoryArea.All)
        {
            var items = GetInventoryItems(player, includedParts);
            return UtilInventory.CountItems(items, filter);
        }

        public static IEnumerable<Item> IterateAllVacuumBagItems(this Player player)
        {
            if (!Spellwright.Instance.IntegAndroLib.IsEnabled)
                yield break;

            foreach (Item item in player.GetInventoryItems(InventoryArea.MainSlots | InventoryArea.Ammo))
            {
                if (item.IsValidItem() && Spellwright.Instance.IntegAndroLib.TryGetStorageByItemId(item.type, out Item[] itemStorage))
                {
                    foreach (Item internalItem in itemStorage)
                    {
                        if (internalItem.IsValidItem())
                            yield return internalItem;
                    }
                }
            }
        }

        public static void ClearBuffs(this Player player, IEnumerable<int> buffTypes)
        {
            var buffHash = new HashSet<int>(buffTypes);
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                int buffTime = player.buffTime[i];
                int buffType = player.buffType[i];

                if (buffTime > 0 && buffHash.Contains(buffType))
                {
                    player.buffTime[i] = 0;
                    player.buffType[i] = 0;
                }
            }

            // Code from DelBuff
            //single pass compactor (vanilla is n^2)
            int packedIdx = 0;
            for (int i = 0; i < Player.MaxBuffs - 1; i++)
            {
                if (player.buffTime[i] == 0 || player.buffType[i] == 0)
                    continue;

                if (packedIdx < i)
                {
                    player.buffTime[packedIdx] = player.buffTime[i];
                    player.buffType[packedIdx] = player.buffType[i];
                    player.buffTime[i] = 0;
                    player.buffType[i] = 0;
                }

                packedIdx++;
            }
        }

        public static bool QuickBuff_ShouldBotherUsingThisBuff(this Player player, int attemptedType)
        {
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                int buffId = player.buffType[i];
                if (attemptedType == BuffID.FairyBlue && (buffId == BuffID.FairyBlue || buffId == BuffID.FairyRed || buffId == BuffID.FairyGreen))
                    return false;

                if (BuffID.Sets.IsWellFed[attemptedType] && BuffID.Sets.IsWellFed[buffId])
                    return false;

                if (buffId == attemptedType)
                    return false;

                if (Main.meleeBuff[attemptedType] && Main.meleeBuff[buffId])
                    return false;
            }

            bool isVanityPet = Main.vanityPet[attemptedType];
            bool isLightPet = Main.lightPet[attemptedType];
            if (isLightPet || isVanityPet)
            {
                for (int j = 0; j < Player.MaxBuffs; j++)
                {
                    int anotherBuffId = player.buffType[j];
                    if (Main.lightPet[anotherBuffId] && isLightPet)
                        return false;

                    if (Main.vanityPet[anotherBuffId] && isVanityPet)
                        return false;
                }
            }

            return true;
        }
    }
}
