using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Content.Spells.SpellRelated;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Core.Spells
{
    public class SpellInfoProvider
    {
        public static string GetSpellList(Player player)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            AscendSpell ascendSpell = ModContent.GetInstance<AscendSpell>();

            var spellsByLevel = new Dictionary<int, List<ModSpell>>();
            foreach (var spellId in spellPlayer.KnownSpells)
            {
                var spell = SpellLibrary.GetSpellById(spellId);
                if (spell != null)
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

            foreach (var item in spellsByLevel.Values)
                item.Sort(new SpellComparer());

            var spellLevelLists = new List<string>();
            spellLevelLists.Add(Spellwright.GetTranslation("SpellInfo", "KnownSpells").Value);

            for (int i = 0; i < 11; i++)
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
                    var displayNamee = spell.DisplayName.GetTranslation(Language.ActiveCulture);
                    var line = $"[{displayNamee}](link=spell:{internalName})";
                    if (!spellPlayer.UnlockedSpells.Contains(spell.Type))
                    {
                        if (spell.UnlockCost != null)
                        {
                            var costDescritpion = spell.UnlockCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                            var unlockCostDescription = Spellwright.GetTranslation("SpellInfo", "SpellUnlockCost").Format(costDescritpion);
                            line += $" [{unlockCostDescription}]";
                        }
                    }

                    lines.Add(line);
                }

                spellLevelLists.Add(string.Join("\n", lines));
            }

            var result = string.Join("\n\n", spellLevelLists.ToArray());
            return result;
        }


        public static string GetSpellData(Player player, int playerLevel, ModSpell spell, SpellData spellData, bool fullDescription = false)
        {
            string name = spell.DisplayName.GetTranslation(Language.ActiveCulture);

            var descriptionParts = new List<string>();
            descriptionParts.Add(name);

            var descriptionValues = spell.GetDescriptionValues(player, playerLevel, spellData, fullDescription);
            string description = spell.Description.GetTranslation(Language.ActiveCulture);
            descriptionValues.Add(new SpellParameter("Description", description));

            foreach (var value in descriptionValues)
            {
                var parameterName = Spellwright.GetTranslation("DescriptionParts", value.Name);
                var descriptionPart = $"{parameterName}: {value.Value}";
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
