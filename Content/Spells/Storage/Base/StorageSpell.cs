using Spellwright.Content.Spells.Base;
using Spellwright.Extensions;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.Storage.Base
{
    internal abstract class StorageSpell : ModSpell
    {
        protected abstract List<Item> GetStorage(Player player);
        protected abstract bool CanAccept(Item item);
        public abstract int StorageSize(int playerLevel);
        protected abstract InventoryArea IncludedArea();

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            List<Item> storage = GetStorage(player);

            var action = (StorageAction)spellData.ExtraData;
            if (action == StorageAction.Push)
            {
                int maxStorageSize = StorageSize(playerLevel);
                return PushItems(player, maxStorageSize, storage);
            }
            else
            {
                return PopItems(player, storage);
            }
        }

        private static bool PopItems(Player player, List<Item> storage)
        {
            if (storage.Count == 0)
                return false;

            foreach (var item in storage)
            {
                item.position = player.Center;
                if (item.type != ItemID.None && item.stack > 0)
                {
                    var source = new EntitySource_Parent(player);
                    int itemIndex = Item.NewItem(source, player.Center, player.width, player.height, item.type, item.stack, noBroadcast: false, item.prefix, noGrabDelay: true);
                    var groundItem = item.Clone();
                    groundItem.newAndShiny = false;
                    groundItem.favorited = false;
                    Main.item[itemIndex] = groundItem;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1f);
                }
            }
            storage.Clear();
            return true;
        }

        private bool PushItems(Player player, int maxStorageSize, List<Item> storage)
        {
            for (int i = storage.Count - 1; i >= 0; i--)
            {
                var item = storage[i];
                if (item.type == ItemID.None || item.stack == 0)
                    storage.RemoveAt(i);
            }

            bool storedAtLeastOne = false;
            var itemsTypesInStorage = new HashSet<int>(storage.Select(x => x.type));
            foreach (int i in UtilPlayer.GetInventoryIndexes(IncludedArea(), true))
            {
                Item item = player.inventory[i];
                if (item.type != ItemID.None && item.stack > 0 && !item.favorited && CanAccept(item))
                {
                    if (itemsTypesInStorage.Contains(item.type))
                    {
                        foreach (var storedItem in storage)
                        {
                            if (UtilItem.MergeItem(item, storedItem))
                            {
                                storedAtLeastOne = true;
                                if (item.stack == 0)
                                    break;
                            }
                        }
                    }

                    if (item.stack > 0 && storage.Count < maxStorageSize)
                    {
                        storage.Add(item);
                        itemsTypesInStorage.Add(item.type);
                        storedAtLeastOne = true;
                        player.inventory[i] = new Item();
                    }
                }
            }

            storage.Sort(ItemSortOrder);
            return storedAtLeastOne;
        }

        private static int ItemSortOrder(Item item1, Item item2)
        {
            int result = item1.type.CompareTo(item2.type);
            if (result != 0)
                return result;
            return item1.stack.CompareTo(item2.stack);
        }

        public override bool ProcessExtraData(Player player, SpellStructure structure, out object extraData)
        {
            StorageAction action = StorageAction.Invalid;
            string argument = structure.Argument.ToLower();
            if (argument.Length == 0 || argument == "push")
                action = StorageAction.Push;
            else if (argument == "pop")
                action = StorageAction.Pop;

            extraData = action;
            return action != StorageAction.Invalid;
        }

        public override void SerializeExtraData(TagCompound tag, object extraData)
        {
            tag.Add("ExtraData", (int)extraData);
        }

        public override object DeserializeExtraData(TagCompound tag)
        {
            return tag.GetInt("ExtraData");
        }
    }
}