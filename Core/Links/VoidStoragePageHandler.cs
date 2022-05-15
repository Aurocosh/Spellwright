using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Extensions;
using Spellwright.UI.Components.TextBox.Text;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;

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

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            VoidStorageType storageType = linkData.GetParameter("type", VoidStorageType.Item);

            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            List<Item> storage;
            if (storageType == VoidStorageType.Item)
                storage = statPlayer.StoredItems;
            else if (storageType == VoidStorageType.Potion)
                storage = statPlayer.PotionItems;
            else
                storage = statPlayer.ReagentItems;

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

            var countTitle = GetTranslation("ItemsInStorage").Format(storage.Count).AsFormText().WithColor(Color.Gray);
            stringBuilder.AppendLine(countTitle.ToString());
            stringBuilder.AppendLine();

            var sortedStorage = storage.OrderBy(x => x.Name).ThenByDescending(x => x.stack);

            var displayedItems =
                from item in storage
                where item.type != ItemID.None && item.stack > 0
                orderby item.Name ascending
                group item by item.type into itemGroup
                select itemGroup;

            foreach (var value in displayedItems)
            {
                var item = value.First();
                int count = value.Sum(x => x.stack);

                string name = Lang.GetItemNameValue(item.type);
                stringBuilder.Append($"{name} ({count})");
                if (storageType == VoidStorageType.Potion)
                {
                    string statusText = item.favorited ? "Unlocked" : "Locked";
                    Color color = item.favorited ? Color.DarkOrange : Color.DarkGray;
                    string lockText = new FormattedText(statusText, color).WithLink("VoidStorage").WithParam("type", storageType).WithParam("fav", item.type).ToString();
                    string drinkLink = new FormattedText("Drink", Color.Gray).WithLink("VoidStorage").WithParam("type", storageType).WithParam("drink", item.type).ToString();
                    stringBuilder.Append($" ({lockText}, {drinkLink})");
                }
                stringBuilder.AppendLine();
            }


            //foreach (var item in sortedStorage)
            //{
            //    if (item.type != ItemID.None && item.stack > 0)
            //    {
            //        string name = Lang.GetItemNameValue(item.type);
            //        stringBuilder.Append($"{name} ({item.stack})");
            //        if (storageType == VoidStorageType.Potion)
            //        {
            //            string drinkLink = new FormattedText("Drink", Color.Gray).WithLink("VoidStorage").WithParam("type", storageType).WithParam("drink", item.type).ToString();
            //            stringBuilder.Append($" ({drinkLink})");
            //        }
            //        stringBuilder.AppendLine();
            //    }
            //}

            return stringBuilder.ToString();
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
