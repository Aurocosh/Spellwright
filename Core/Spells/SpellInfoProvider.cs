using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.UI.Components.TextBox.Text;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Core.Spells
{
    public class SpellInfoProvider
    {
        public static string GetSpellList(Player player, bool favoritedOnly = false, bool includeLocked = true, bool includeUnlocked = true, bool showCosts = false)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            AscendSpell ascendSpell = ModContent.GetInstance<AscendSpell>();

            var spellsByLevel = new Dictionary<int, List<ModSpell>>();
            foreach (var spellId in spellPlayer.KnownSpells)
            {
                var spell = SpellLibrary.GetSpellById(spellId);
                if (spell != null)
                {
                    bool isLocked = spell.UnlockCost != null && !spellPlayer.UnlockedSpells.Contains(spellId);
                    if ((includeUnlocked && !isLocked) || (includeLocked && isLocked))
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

            var spellLevelLists = new List<string>();
            spellLevelLists.Add(Spellwright.GetTranslation("SpellInfo", "KnownSpells").Value);

            int maxLevel = includeLocked ? 10 : spellPlayer.PlayerLevel;
            int limit = maxLevel + 1;
            for (int i = 0; i < limit; i++)
            {
                if (!spellsByLevel.TryGetValue(i, out List<ModSpell> spells))
                    continue;
                if (spells.Count == 0)
                    continue;

                var levelWord = Spellwright.GetTranslation("SpellInfo", "Level").Value;
                var levelHeader = $"{levelWord} {i}";

                if (spellPlayer.PlayerLevel < i)
                {
                    var cost = ascendSpell.GetLevelUpCost(i - 1);
                    if (cost != null)
                    {
                        var costDescritpion = cost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                        var LevelUpCost = Spellwright.GetTranslation("SpellInfo", "LevelUpCost").Format(costDescritpion);
                        levelHeader += $" [{LevelUpCost}]";
                    }
                }

                var lines = new List<string>();
                lines.Add(levelHeader);
                foreach (var spell in spells)
                {
                    var internalName = spell.Name;
                    var displayName = spell.DisplayName.GetTranslation(Language.ActiveCulture);

                    var line = new FormattedText(displayName, Color.DarkGoldenrod).WithLink("Spell", internalName).ToString();
                    if (!spellPlayer.UnlockedSpells.Contains(spell.Type))
                    {
                        if (showCosts && spell.UnlockCost != null)
                        {
                            var costDescritpion = spell.UnlockCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                            var unlockCostDescription = Spellwright.GetTranslation("SpellInfo", "SpellUnlockCost").Format(costDescritpion);
                            line += $" [{unlockCostDescription}]";
                        }
                        else if (showCosts && spell.UseCost != null)
                        {
                            var costDescritpion = spell.UseCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                            var useCostDescription = Spellwright.GetTranslation("SpellInfo", "SpellUseCost").Format(costDescritpion);
                            line += $" [{useCostDescription}]";
                        }
                    }

                    lines.Add(line);
                }

                spellLevelLists.Add(string.Join("\n", lines));
            }

            var result = string.Join("\n\n", spellLevelLists.ToArray());
            return result;
        }


        public static string GetSpellDescription(Player player, int playerLevel, ModSpell spell, SpellData spellData, bool fullDescription = false, bool isFormatted = false)
        {
            string name = spell.DisplayName.GetTranslation(Language.ActiveCulture);
            if (isFormatted)
                name = new FormattedText(name, Color.DarkGoldenrod).ToString();

            var descriptionParts = new List<string>();
            descriptionParts.Add(name);

            var descriptionValues = spell.GetDescriptionValues(player, playerLevel, spellData, fullDescription);
            string description = spell.Description.GetTranslation(Language.ActiveCulture);
            descriptionValues.Add(new SpellParameter("Description", description));

            foreach (var value in descriptionValues)
            {
                var parameterName = Spellwright.GetTranslation("DescriptionParts", value.Name).Value + ":";
                if (isFormatted)
                    parameterName = new FormattedText(parameterName, Color.Gray).ToString();

                var descriptionPart = $"{parameterName} {value.Value}";
                descriptionParts.Add(descriptionPart);
            }

            return string.Join("\n", descriptionParts);
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
