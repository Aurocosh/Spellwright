using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Core.Spells;
using Spellwright.UI.States;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.SpellRelated
{
    internal class RecallSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;
            if (spellData.ExtraData is not string recallData || recallData is null || recallData.Trim().Length == 0)
                return PrintSpellList(player, playerLevel);
            else
                return PrintSpellInfo(player, playerLevel, recallData);
        }

        private bool PrintSpellList(Player player, int playerLevel)
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
            spellLevelLists.Add(GetTranslation("KnownSpells").Value);

            for (int i = 0; i < 11; i++)
            {
                if (!spellsByLevel.TryGetValue(i, out List<ModSpell> spells))
                    continue;
                if (spells.Count == 0)
                    continue;

                var levelWord = GetTranslation("Level").Value;
                var levelHeader = $"{levelWord} {i}";

                if (spellPlayer.PlayerLevel < i)
                {
                    var cost = ascendSpell.GetLevelUpCost(i);
                    if (cost != null)
                    {
                        var costDescritpion = cost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                        var LevelUpCost = GetTranslation("LevelUpCost").Format(costDescritpion);
                        levelHeader += $" [{LevelUpCost}]";
                    }
                }

                var lines = new List<string>();
                lines.Add(levelHeader);
                foreach (var spell in spells)
                {
                    var line = spell.DisplayName.GetTranslation(Language.ActiveCulture);
                    if (!spellPlayer.UnlockedSpells.Contains(spell.Type))
                    {
                        if (spell.UnlockCost != null)
                        {
                            var costDescritpion = spell.UnlockCost.GetDescription(player, spellPlayer.PlayerLevel, SpellData.EmptyData);
                            var unlockCostDescription = GetTranslation("SpellUnlockCost").Format(costDescritpion);
                            line += $" [{unlockCostDescription}]";
                        }
                    }

                    lines.Add(line);
                }

                spellLevelLists.Add(string.Join("\n", lines));
            }

            var result = string.Join("\n\n", spellLevelLists.ToArray());

            UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
            uiMessageState.SetMessage(result);
            Spellwright.Instance.userInterface.SetState(uiMessageState);

            return true;
        }

        private static bool PrintSpellInfo(Player player, int playerLevel, string recallData)
        {
            var spellStructure = SpellProcessor.ProcessIncantation(recallData);
            ModSpell spell = SpellLibrary.GetSpellByIncantation(spellStructure.SpellName);
            if (spell == null)
            {
                Main.NewText(Spellwright.GetTranslation("CastErrors", "IncantationInvalid"), Color.Red);
                return false;
            }

            bool isModifiersApplicable = spell.IsModifiersApplicable(spellStructure.SpellModifiers);
            if (!isModifiersApplicable)
            {
                Main.NewText(Spellwright.GetTranslation("CastErrors", "ModifiersInvalid"), Color.Red);
                return false;
            }

            bool isSpellDataValid = spell.ProcessExtraData(player, spellStructure, out object extraData);
            if (!isSpellDataValid)
            {
                Main.NewText(Spellwright.GetTranslation("CastErrors", "DataInvalid"), Color.Red);
                return false;
            }

            var costModifier = spell.GetCostModifier(spellStructure.SpellModifiers);
            var subSpellData = new SpellData(spellStructure.SpellModifiers, spellStructure.Argument, costModifier, extraData);

            string name = spell.DisplayName.GetTranslation(Language.ActiveCulture);

            var descriptionParts = new List<string>();
            descriptionParts.Add(name);

            var descriptionValues = spell.GetDescriptionValues(player, playerLevel, subSpellData, true);
            string description = spell.Description.GetTranslation(Language.ActiveCulture);
            descriptionValues.Add(new SpellParameter("Description", description));

            foreach (var value in descriptionValues)
            {
                var parameterName = Spellwright.GetTranslation("DescriptionParts", value.Name);
                var descriptionPart = $"{parameterName}: {value.Value}";
                descriptionParts.Add(descriptionPart);
            }

            var fulllMessage = string.Join("\n", descriptionParts);

            UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
            uiMessageState.SetMessage(fulllMessage);
            Spellwright.Instance.userInterface.SetState(uiMessageState);

            return true;
        }

        public override List<SpellParameter> GetDescriptionValues(Player player, int playerLevel, SpellData spellData, bool fullVersion)
        {
            var values = base.GetDescriptionValues(player, playerLevel, spellData, fullVersion);
            string examples = Spellwright.GetTranslation("Spells", Name, "Examples").Value;
            examples = examples.Replace("\\t", "   ");
            examples = examples.Replace("\\n", "\n");
            values.Add(new SpellParameter("Examples", examples));
            return values;
        }

        public override bool ProcessExtraData(Player player, SpellStructure structure, out object extraData)
        {
            extraData = structure.Argument;
            return true;
        }

        public override void SerializeExtraData(TagCompound tag, object extraData)
        {
            if (extraData is string recallData)
                tag.Add("RecallData", recallData);
        }

        public override object DeserializeExtraData(TagCompound tag)
        {
            string recallData = tag.GetString("RecallData");
            return recallData;
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
