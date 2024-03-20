using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Storage;
using Spellwright.Content.Spells.Storage.Base;
using Spellwright.Core.Links.Base;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using Spellwright.UI.Components.TextBox.Text;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace Spellwright.Core.Links
{
    internal class VoidStoragePageHandler : PageHandler
    {
        private enum VoidStorageType
        {
            Item = 0,
            Potion = 1,
            Reagent = 2
        }

        public VoidStoragePageHandler()
        {
        }

        private ItemVoidSpell _itemVoidSpell = null;
        private PotionVoidSpell _potionVoidSpell = null;
        private ReagentVoidSpell _reagentVoidSpell = null;

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            VoidStorageType storageType = linkData.GetId(VoidStorageType.Item);

            var spellwrightPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            List<Item> storage = GetPlayerStorage(storageType, statPlayer);
            StorageSpell storageSpell = GetStorageSpell(storageType);
            int storageSize = storageSpell?.StorageSize(spellwrightPlayer.PlayerLevel) ?? 0;

            var stringBuilder = new StringBuilder();

            var title = GetFormText(storageType + "Void").WithColor(Color.Purple);
            stringBuilder.AppendLine(title.ToString());

            if (storageType == VoidStorageType.Potion)
            {
                if (linkData.HasParameter("drink"))
                {
                    int potionType = linkData.GetParameter("drink", 0);
                    DrinkPotion(player, storage, potionType);
                    linkData.RemoveParameter("drink");
                }

                if (linkData.HasParameter("fav"))
                {
                    int potionType = linkData.GetParameter("fav", 0);
                    ToggleFavorites(storage, potionType);
                    linkData.RemoveParameter("fav");
                }
            }

            if (linkData.HasParameter("group"))
            {
                statPlayer.GroupStorageByType = !statPlayer.GroupStorageByType;
                linkData.RemoveParameter("group");
            }

            var storageSizeTitle = GetTranslation("StorageSize").Format(storageSize).AsFormText().WithColor(Color.Gray);
            stringBuilder.AppendLine(storageSizeTitle.ToString());
            var countTitle = GetTranslation("ItemsInStorage").Format(storage.Count).AsFormText().WithColor(Color.Gray);
            stringBuilder.AppendLine(countTitle.ToString());

            string groupTrKey = statPlayer.GroupStorageByType ? "ItemsGrouped" : "ItemsNotGrouped";
            var groupTitle = GetFormText(groupTrKey).WithLink("VoidStorage", storageType).WithParam("group").WithColor(Color.Orange).ToString();
            stringBuilder.AppendLine(groupTitle.ToString());
            stringBuilder.AppendLine();

            var sortedStorage = storage.OrderBy(x => x.Name).ThenByDescending(x => x.stack);

            var displayedItems =
                from item in storage
                where item.type != ItemID.None && item.stack > 0
                orderby item.favorited descending, item.Name ascending, item.stack descending
                group item by item.type into itemGroup
                select itemGroup;

            foreach (var value in displayedItems)
            {
                var firstItem = value.First();
                var totalCount = value.Sum(x => x.stack);
                string name = Lang.GetItemNameValue(firstItem.type);

                var itemsToDisplay = value.AsEnumerable();
                if (statPlayer.GroupStorageByType)
                    itemsToDisplay = Enumerable.Repeat(firstItem, 1);

                foreach (var nextItem in itemsToDisplay)
                {
                    int count = statPlayer.GroupStorageByType ? totalCount : nextItem.stack;
                    if (storageType == VoidStorageType.Potion)
                    {
                        string statusText = firstItem.favorited ? "Unlocked" : "Locked";
                        Color color = firstItem.favorited ? Color.DarkOrange : Color.DarkGray;
                        string lockText = new FormattedText(statusText, color).WithLink("VoidStorage", storageType).WithParam("fav", firstItem.type).ToString();
                        string drinkLink = new FormattedText("Drink", Color.Gray).WithLink("VoidStorage", storageType).WithParam("drink", firstItem.type).ToString();
                        stringBuilder.Append($" ({lockText}, {drinkLink}) - ");
                    }

                    stringBuilder.Append($"{name} ({count})");
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }

        private static List<Item> GetPlayerStorage(VoidStorageType storageType, SpellwrightStatPlayer statPlayer)
        {
            if (storageType == VoidStorageType.Item)
                return statPlayer.StoredItems;
            else if (storageType == VoidStorageType.Potion)
                return statPlayer.PotionItems;
            else
                return statPlayer.ReagentItems;
        }

        private StorageSpell GetStorageSpell(VoidStorageType storageType)
        {
            switch (storageType)
            {
                case VoidStorageType.Item:
                    {
                        _itemVoidSpell ??= SpellLibrary.GetSpellByType<ItemVoidSpell>();
                        return _itemVoidSpell;
                    }
                case VoidStorageType.Potion:
                    {
                        _potionVoidSpell ??= SpellLibrary.GetSpellByType<PotionVoidSpell>();
                        return _potionVoidSpell;
                    }
                case VoidStorageType.Reagent:
                    {
                        _reagentVoidSpell ??= SpellLibrary.GetSpellByType<ReagentVoidSpell>();
                        return _reagentVoidSpell;
                    }
            }
            return null;
        }

        private static void DrinkPotion(Player player, List<Item> storage, int potionType)
        {
            if (potionType <= 0)
                return;

            var potions =
                from item in storage
                where item.type == potionType && item.stack > 0
                select item;

            var firstItem = potions.FirstOrDefault();
            if (firstItem != null)
                UtilPlayer.DrinkPotion(player, firstItem);
        }

        private static void ToggleFavorites(List<Item> storage, int potionType)
        {
            if (potionType <= 0)
                return;

            var potions =
                from item in storage
                where item.type == potionType && item.stack > 0
                select item;

            var firstItem = potions.FirstOrDefault();
            if (firstItem != null)
            {
                bool status = !firstItem.favorited;
                foreach (var item in potions)
                    item.favorited = status;
            }
        }
    }
}
