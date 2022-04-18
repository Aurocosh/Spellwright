﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

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

        public static Item InventoryFindItem(this Player player, int itemTypeId, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return InventoryFindItem(player, IsValid, includedParts, reverseOrder);
        }

        public static Item InventoryFindItem(this Player player, Func<Item, bool> filter, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            foreach (int i in player.GetInventoryIndexes(includedParts, reverseOrder))
            {
                Item item = player.inventory[i];
                if (filter.Invoke(item) && item.stack > 0)
                    return item;
            }

            return null;
        }

        public static bool ConsumeItems(this Player player, int itemTypeId, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return ConsumeItems(player, IsValid, amount, includedParts, reverseOrder);
        }

        public static bool ConsumeItems(this Player player, Func<Item, bool> filter, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            if (amount <= 0)
                return false;

            int amountInInventory = 0;
            List<Item> fittingItems = new();
            foreach (int i in player.GetInventoryIndexes(includedParts, reverseOrder))
            {
                Item item = player.inventory[i];
                if (filter.Invoke(item) && item.stack > 0)
                {
                    fittingItems.Add(item);
                    amountInInventory += item.stack;
                    if (amountInInventory > amount)
                        break;
                }
            }

            if (amountInInventory < amount)
                return false;

            foreach (Item item in fittingItems)
            {
                int amountToSub = Math.Min(amount, item.stack);
                item.stack -= amountToSub;
                if (item.stack == 0)
                    item.TurnToAir();
                amount -= amountToSub;
                if (amount == 0)
                    break;
            }

            return true;
        }

        public static bool HasItems(this Player player, int itemTypeId, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return HasItems(player, IsValid, amount, includedParts, reverseOrder);
        }

        public static bool HasItems(this Player player, Func<Item, bool> filter, int amount = 1, InventoryArea includedParts = InventoryArea.All, bool reverseOrder = false)
        {
            if (amount <= 0)
                return false;

            int amountInInventory = 0;
            foreach (int i in player.GetInventoryIndexes(includedParts, reverseOrder))
            {
                Item item = player.inventory[i];
                if (filter.Invoke(item) && item.stack > 0)
                {
                    amountInInventory += item.stack;
                    if (amountInInventory > amount)
                        return true;
                }
            }

            return false;
        }

        public static int CountItems(this Player player, int itemTypeId, InventoryArea includedParts = InventoryArea.All)
        {
            bool IsValid(Item item) => item.type == itemTypeId;
            return CountItems(player, IsValid, includedParts);
        }

        public static int CountItems(this Player player, Func<Item, bool> filter, InventoryArea includedParts = InventoryArea.All)
        {
            int amountInInventory = 0;
            foreach (int i in player.GetInventoryIndexes(includedParts, false))
            {
                Item item = player.inventory[i];
                if (filter.Invoke(item) && item.stack > 0)
                    amountInInventory += item.stack;
            }

            return amountInInventory;
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
    }
}
