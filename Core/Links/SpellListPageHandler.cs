using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.Core.Spells;
using Spellwright.UI.Components.TextBox.Text;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Core.Links
{
    internal class SpellListPageHandler : PageHandler
    {
        private static string CategoryParam = "category";
        private static string CostParam = "cost";

        private enum PageCategory
        {
            All = 0,
            Locked = 1,
            Unlocked = 2,
            Favorite = 3
        }

        public SpellListPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var category = linkData.GetParameter(CategoryParam, PageCategory.All);
            bool showCost = linkData.GetParameter(CostParam, false);

            bool showFavorite = category == PageCategory.Favorite;
            bool showLocked = category == PageCategory.All || category == PageCategory.Locked;
            bool showUnlocked = category == PageCategory.All || category == PageCategory.Unlocked;

            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            AscendSpell ascendSpell = ModContent.GetInstance<AscendSpell>();

            var spellsByLevel = new Dictionary<int, List<ModSpell>>();
            foreach (var spellId in spellPlayer.KnownSpells)
            {
                var spell = SpellLibrary.GetSpellById(spellId);
                if (spell != null)
                {
                    bool addSpell = false;
                    if (showFavorite && spellPlayer.FavoriteSpells.Contains(spellId))
                    {
                        addSpell = true;
                    }
                    else
                    {
                        bool isLocked = spell.UnlockCost != null && !spellPlayer.UnlockedSpells.Contains(spellId);
                        if ((showUnlocked && !isLocked) || (showLocked && isLocked))
                            addSpell = true;
                    }

                    if (addSpell)
                    {
                        int spellLevel = spell.SpellLevel;
                        if (!spellsByLevel.TryGetValue(spellLevel, out List<ModSpell> spells))
                        {
                            spells = new List<ModSpell>();
                            spellsByLevel[spellLevel] = spells;
                        }
                        spells.Add(spell);
                    }
                }
            }

            foreach (var item in spellsByLevel.Values)
                item.Sort(new SpellComparer());

            int maxSpellLevel = 0;
            foreach (var level in spellsByLevel.Keys)
                maxSpellLevel = Math.Max(maxSpellLevel, level);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(GetTranslation("Spells").Value);

            var allLink = GetSelfLink("All", category == PageCategory.All, showCost, PageCategory.All);
            var lockedLink = GetSelfLink("Locked", category == PageCategory.Locked, showCost, PageCategory.Locked);
            var unlockedLink = GetSelfLink("Unlocked", category == PageCategory.Unlocked, showCost, PageCategory.Unlocked);
            var favLink = GetSelfLink("Favorite", category == PageCategory.Favorite, showCost, PageCategory.Favorite);
            stringBuilder.AppendLine($"{allLink} | {unlockedLink} | {lockedLink} | {favLink}");

            var hideCostLink = GetSelfLink("HideCost", !showCost, false, category);
            var showCostLink = GetSelfLink("ShowCost", showCost, true, category);
            stringBuilder.AppendLine($"{hideCostLink} | {showCostLink}");

            int maxLevel = showLocked ? 10 : Math.Min(maxSpellLevel, spellPlayer.PlayerLevel);
            int limit = maxLevel + 1;
            for (int i = 0; i < limit; i++)
            {
                if (!spellsByLevel.TryGetValue(i, out List<ModSpell> spells))
                    continue;
                if (spells.Count == 0)
                    continue;

                var levelWord = GetTranslation("Level").Value;
                var levelHeader = $"{levelWord} {i}";

                if (spellPlayer.PlayerLevel < i)
                {
                    var cost = ascendSpell.GetLevelUpCost(i - 1);
                    if (cost != null)
                    {
                        var costDescritpion = cost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                        var LevelUpCost = GetTranslation("LevelUpCost").Format(costDescritpion);
                        levelHeader += $" [{LevelUpCost}]";
                    }
                }

                stringBuilder.AppendLine(levelHeader);
                foreach (var spell in spells)
                {
                    string line = GenerateSpellLine(player, showCost, spellPlayer, spell);
                    stringBuilder.AppendLine(line);
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private string GenerateSpellLine(Player player, bool showCost, SpellwrightPlayer spellPlayer, ModSpell spell)
        {
            var displayName = spell.DisplayName.GetTranslation(Language.ActiveCulture);
            var line = new FormattedText(displayName, Color.DarkGoldenrod).WithLink("Spell").WithParam("name", spell.Name).ToString();

            if (showCost)
            {
                bool isLocked = spell.UnlockCost != null && !spellPlayer.UnlockedSpells.Contains(spell.Type);
                if (isLocked)
                {
                    var descritpion = spell.UnlockCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                    var message = GetTranslation("SpellUnlockCost").Format(descritpion);
                    var lockedMessage = GetFormText("Locked").WithColor(Color.IndianRed).ToString();
                    line += $" [{lockedMessage}. {message}]";
                }
                else if (spell.UseCost != null)
                {
                    var descritpion = spell.UseCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                    var message = GetTranslation("SpellUseCost").Format(descritpion);
                    line += $" [{message}]";
                }
                else if (spell.CastCost != null)
                {
                    var descritpion = spell.CastCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                    var message = GetTranslation("SpellCastCost").Format(descritpion);
                    line += $" [{message}]";
                }
            }
            return line;
        }

        private string GetSelfLink(string key, bool isSelected, bool showCost, PageCategory category)
        {
            var textFormat = GetFormText(key);
            if (isSelected)
                textFormat.WithColor(Color.Red);
            else
                textFormat.WithLink("SpellList").WithParam(CostParam, showCost).WithParam(CategoryParam, ((int)category).ToString());
            return textFormat.ToString();
        }

        private class SpellComparer : IComparer<ModSpell>
        {
            public int Compare(ModSpell a, ModSpell b)
            {
                var aName = a.DisplayName.GetTranslation(Language.ActiveCulture);
                var bName = b.DisplayName.GetTranslation(Language.ActiveCulture);
                return aName.CompareTo(bName);
            }
        }
    }
}
