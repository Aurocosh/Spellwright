using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Core.Spells;
using Spellwright.UI.States;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.SpellRelated
{
    internal class RecallSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;
            if (spellData.ExtraData is not string recallData || recallData is null || recallData.Trim().Length == 0)
                return PringSpellList(player, playerLevel);
            else
                return PrintSpellInfo(player, playerLevel, recallData);
        }

        private static bool PringSpellList(Player player, int playerLevel)
        {
            var modSpells = SpellLibrary.GetRegisteredSpells();
            var spellsByLevel = new Dictionary<int, List<ModSpell>>();

            foreach (var spell in modSpells)
            {
                int spellLevel = spell.SpellLevel;
                if (!spellsByLevel.TryGetValue(spellLevel, out List<ModSpell> spells))
                {
                    spells = new List<ModSpell>();
                    spellsByLevel[spellLevel] = spells;
                }
                spells.Add(spell);
            }

            var spellLevelLists = new List<string>();
            spellLevelLists.Add(Spellwright.GetTranslation("General", "KnownSpells").Value);

            for (int i = 0; i < 11; i++)
            {
                if (!spellsByLevel.TryGetValue(i, out List<ModSpell> spells))
                    continue;
                if (spells.Count == 0)
                    continue;

                var levelWord = Spellwright.GetTranslation("General", "Level").Value;
                var levelHeader = $"{levelWord} {i}";

                var lines = new List<string>();
                lines.Add(levelHeader);
                foreach (var spell in spells)
                {
                    var name = spell.DisplayName.GetTranslation(Language.ActiveCulture);
                    lines.Add(name);
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

            bool isSpellDataValid = spell.ProcessExtraData(spellStructure, out object extraData);
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

        public override bool ProcessExtraData(SpellStructure structure, out object extraData)
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
    }
}
