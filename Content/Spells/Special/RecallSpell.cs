using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Core.Spells;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Spells.Special
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
            if (spellData.ExtraData is not string recallData)
                return false;

            var spellStructure = SpellProcessor.ProcessIncantation(recallData);
            ModSpell spell = SpellLibrary.GetSpellByIncantation(spellStructure.SpellName);
            if (spell == null)
                return false;

            string name = spell.DisplayName.GetTranslation(Language.ActiveCulture);

            var descriptionParts = new List<string>();
            descriptionParts.Add(name);

            var descriptionValues = spell.GetDescriptionValues(playerLevel, true);
            string description = spell.Description.GetTranslation(Language.ActiveCulture);
            descriptionValues.Add(new SpellParameter("Description", description));

            foreach (var value in descriptionValues)
            {
                var parameterName = Spellwright.GetTranslation("DescriptionParts", value.Name);
                var descriptionPart = $"{parameterName}: {value.Value}";
                descriptionParts.Add(descriptionPart);
            }

            var fulllMessage = string.Join("\n", descriptionParts);
            Main.NewTextMultiline(fulllMessage, false, Color.White);

            return true;
        }
        public override bool ProcessExtraData(SpellStructure structure, out object extraData)
        {
            if (structure.Argument.Length == 0)
            {
                extraData = null;
                return false;
            }

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
