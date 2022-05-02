using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Description;
using Spellwright.Core.Spells;
using Spellwright.UI.States;
using System.Collections.Generic;
using Terraria;
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
            string result = SpellInfoProvider.GetSpellList(player);
            //result = "My favorite search engine is [Duck Duck Go](link=Spell:Bird Of Midas,color=blue). Got stuck between two objects, waited 5 minutes for the narrator to [speak](link=spell:Return), realised it was a bug; I've personally never had the need for a RectangleF structure, as I always cast floats to int when using Rectangles or simply avoid float altogether when positioning sprites. Not sure why SpriteBatch works with both float and int depending on the overload, but that's how Microsoft implemented it.";


            UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
            Spellwright.Instance.userInterface.SetState(uiMessageState);
            uiMessageState.SetMessage(result);
            if (Main.playerInventory)
                player.ToggleInv();

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
            string fulllMessage = SpellInfoProvider.GetSpellData(player, playerLevel, spell, subSpellData, true);

            UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
            Spellwright.Instance.userInterface.SetState(uiMessageState);
            uiMessageState.SetMessage(fulllMessage);
            if (Main.playerInventory)
                player.ToggleInv();

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
    }
}
