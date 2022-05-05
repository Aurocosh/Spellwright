using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.Core.Spells;
using Spellwright.UI.Components.TextBox.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Core.Links
{
    internal class SpellListPageHandler : PageHandler
    {
        private static readonly string CategoryParam = "category";
        private static readonly string TypeParam = "type";
        private static readonly string CostParam = "cost";

        private enum PageCategory
        {
            All = 0,
            Locked = 1,
            Unlocked = 2,
            Favorite = 3
        }

        private class SpellPageData
        {
            public PageCategory Category { get; set; }
            public SpellType? TypeCategory { get; set; }
            public bool ShowCost { get; set; }

            public SpellPageData(PageCategory category, SpellType? typeCategory, bool showCost)
            {
                Category = category;
                TypeCategory = typeCategory;
                ShowCost = showCost;
            }
        }

        public SpellListPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var category = linkData.GetParameter(CategoryParam, PageCategory.All);
            int typeId = linkData.GetParameter(TypeParam, -1);
            SpellType? typeCategory = typeId != -1 ? (SpellType)typeId : null;
            var showCost = linkData.GetParameter(CostParam, false);

            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            Dictionary<int, List<ModSpell>> spellsByLevel = PrepareSpellList(spellPlayer, category, typeCategory);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(GetTranslation("Spells").Value);

            var pageData = new SpellPageData(category, typeCategory, showCost);

            var allLink = GetCategoryLink("All", PageCategory.All, pageData);
            var lockedLink = GetCategoryLink("Locked", PageCategory.Locked, pageData);
            var unlockedLink = GetCategoryLink("Unlocked", PageCategory.Unlocked, pageData);
            var favLink = GetCategoryLink("Favorite", PageCategory.Favorite, pageData);
            stringBuilder.AppendLine($"{allLink} | {unlockedLink} | {lockedLink} | {favLink}");

            var allTypesLink = GetTypeLink("AllTypes", null, pageData);
            var invocationLink = GetTypeLink("Invocation", SpellType.Invocation, pageData);
            var cantripLink = GetTypeLink("Cantrip", SpellType.Cantrip, pageData);
            var echoLink = GetTypeLink("Echo", SpellType.Echo, pageData);
            stringBuilder.AppendLine($"{allTypesLink} | {invocationLink} | {cantripLink} | {echoLink}");

            var hideCostLink = GetCostLink("HideCost", false, pageData);
            var showCostLink = GetCostLink("ShowCost", true, pageData);
            stringBuilder.AppendLine($"{hideCostLink} | {showCostLink}");

            AscendSpell ascendSpell = ModContent.GetInstance<AscendSpell>();
            int maxSpellLevel = spellsByLevel.Keys.DefaultIfEmpty(0).Max();
            bool allLevels = category == PageCategory.All || category == PageCategory.Locked;
            int maxLevel = allLevels ? 10 : Math.Min(maxSpellLevel, spellPlayer.PlayerLevel);
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

        private static Dictionary<int, List<ModSpell>> PrepareSpellList(SpellwrightPlayer spellPlayer, PageCategory category, SpellType? typeCategory)
        {
            bool showFavorite = category == PageCategory.Favorite;
            bool showLocked = category == PageCategory.All || category == PageCategory.Locked;
            bool showUnlocked = category == PageCategory.All || category == PageCategory.Unlocked;

            var spellsByLevel = new Dictionary<int, List<ModSpell>>();
            foreach (var spellId in spellPlayer.KnownSpells)
            {
                var spell = SpellLibrary.GetSpellById(spellId);
                if (spell != null)
                {
                    if (typeCategory != null && typeCategory != spell.UseType)
                        continue;
                    if (!showFavorite && spellPlayer.FavoriteSpells.Contains(spellId))
                        continue;
                    bool isLocked = spell.UnlockCost != null && !spellPlayer.UnlockedSpells.Contains(spellId);
                    if ((!showUnlocked || isLocked) && (!showLocked || !isLocked))
                        continue;

                    int spellLevel = spell.SpellLevel;
                    if (!spellsByLevel.TryGetValue(spellLevel, out List<ModSpell> spells))
                    {
                        spells = new List<ModSpell>();
                        spellsByLevel[spellLevel] = spells;
                    }
                    spells.Add(spell);
                }
            }

            foreach (var item in spellsByLevel.Values)
                item.Sort(new SpellComparer());

            return spellsByLevel;
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

        private string GetCategoryLink(string key, PageCategory category, SpellPageData pageData) => GetSelfLink(key, pageData.Category == category, category, pageData.TypeCategory, pageData.ShowCost);
        private string GetTypeLink(string key, SpellType? type, SpellPageData pageData) => GetSelfLink(key, pageData.TypeCategory == type, pageData.Category, type, pageData.ShowCost);
        private string GetCostLink(string key, bool showCost, SpellPageData pageData) => GetSelfLink(key, pageData.ShowCost == showCost, pageData.Category, pageData.TypeCategory, showCost);

        private string GetSelfLink(string key, bool isSelected, PageCategory category, SpellType? type, bool showCost)
        {
            var textFormat = GetFormText(key);
            if (isSelected)
            {
                textFormat.WithColor(Color.Red);
            }
            else
            {
                textFormat.WithLink("SpellList");
                textFormat.WithParam(CostParam, showCost);
                textFormat.WithParam(CategoryParam, ((int)category).ToString());
                if (type != null)
                    textFormat.WithParam(TypeParam, (int)type.Value);
            }

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
