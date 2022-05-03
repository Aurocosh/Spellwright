using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Extensions;
using Spellwright.UI.Components.TextBox.Text;
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

            if (storageType != VoidStorageType.Item)
            {
                bool locked = IsStorageLocked(statPlayer, storageType);
                bool hasToggle = linkData.HasParameter("toggle");
                if (hasToggle)
                {
                    locked = !locked;
                    SetStorageLocked(statPlayer, storageType, locked);
                    linkData.RemoveParameter("toggle");
                }

                string statusText = locked ? "Locked" : "Unlocked";
                Color color = locked ? Color.DarkGray : Color.DarkOrange;
                string lockText = new FormattedText(statusText, color).WithLink("VoidStorage").WithParam("type", storageType).WithParam("toggle").ToString();

                string storageStatus = GetTranslation("StorageStatus").Format(lockText);
                stringBuilder.AppendLine(storageStatus);
            }

            var countTitle = GetTranslation("ItemsInStorage").Format(storage.Count).AsFormText().WithColor(Color.Gray);
            stringBuilder.AppendLine(countTitle.ToString());
            stringBuilder.AppendLine();

            var sortedStorage = storage.OrderBy(x => x.Name).ThenByDescending(x => x.stack);
            foreach (var item in sortedStorage)
            {
                if (item.type != ItemID.None && item.stack > 0)
                {
                    string name = Lang.GetItemNameValue(item.type);
                    string line = $"{name} ({item.stack})";
                    stringBuilder.AppendLine(line);
                }
            }

            return stringBuilder.ToString();
        }

        private static bool IsStorageLocked(SpellwrightStatPlayer statPlayer, VoidStorageType storageType)
        {
            if (storageType == VoidStorageType.Potion)
                return statPlayer.PotionsLocked;
            else if (storageType == VoidStorageType.Reagent)
                return statPlayer.ReagentsLocked;
            else
                return false;
        }

        private static void SetStorageLocked(SpellwrightStatPlayer statPlayer, VoidStorageType storageType, bool value)
        {
            if (storageType == VoidStorageType.Potion)
                statPlayer.PotionsLocked = value;
            else if (storageType == VoidStorageType.Reagent)
                statPlayer.ReagentsLocked = value;
        }
    }
}
