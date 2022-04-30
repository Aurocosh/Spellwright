using Spellwright.Content.Spells.Base;
using Spellwright.Extensions;
using Spellwright.UI.States;
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
        protected abstract int StorageSize(int playerLevel);
        protected abstract InventoryArea IncludedArea();

        public override bool ConsumeReagentsCast(Player player, int playerLevel, SpellData spellData)
        {
            var action = (StorageAction)spellData.ExtraData;
            if (action == StorageAction.Info)
                return true;
            return base.ConsumeReagentsCast(player, playerLevel, spellData);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            List<Item> storage = GetStorage(player);

            var action = (StorageAction)spellData.ExtraData;
            if (action == StorageAction.Info)
                PrintInfo(storage);
            else if (action == StorageAction.Push)
            {
                int maxStorageSize = StorageSize(playerLevel);
                return PushItems(player, maxStorageSize, storage);
            }
            else if (action == StorageAction.Pop)
                return PopItems(player, storage);

            return true;
        }

        private static void PrintInfo(List<Item> storage)
        {
            var lines = new List<string>();
            lines.Add(Spellwright.GetTranslation("General", "StoredItems").Value);

            var sortedStorage = storage.OrderBy(x => x.Name).ThenByDescending(x => x.stack);
            foreach (var item in sortedStorage)
                if (item.type != ItemID.None && item.stack > 0)
                {
                    string name = Lang.GetItemNameValue(item.type);
                    string line = $"{item.stack} {name}";
                    lines.Add(line);
                }

            var result = string.Join("\n", lines.ToArray());

            UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
            uiMessageState.SetMessage(result);
            Spellwright.Instance.userInterface.SetState(uiMessageState);
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
                    Main.item[itemIndex] = item.Clone();
                    Main.item[itemIndex].newAndShiny = false;
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
            foreach (int i in UtilPlayer.GetInventoryIndexes(IncludedArea(), true))
            {
                if (storage.Count >= maxStorageSize)
                    break;

                Item item = player.inventory[i];
                if (item.type != ItemID.None && item.stack > 0 && !item.favorited && CanAccept(item))
                {
                    foreach (var storedItem in storage)
                        if (UtilItem.MergeItem(item, storedItem) && item.stack == 0)
                            break;

                    if (item.stack > 0)
                        storage.Add(item);

                    player.inventory[i] = new Item();
                    storedAtLeastOne = true;
                }
            }

            return storedAtLeastOne;
        }

        public override bool ProcessExtraData(Player player, SpellStructure structure, out object extraData)
        {
            StorageAction action = StorageAction.Invalid;
            string argument = structure.Argument.ToLower();
            if (argument.Length == 0 || argument == "info")
                action = StorageAction.Info;
            else if (argument == "push")
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