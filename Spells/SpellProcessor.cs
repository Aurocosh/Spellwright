using Spellwright.Players;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Spells
{
    internal static class SpellProcessor
    {
        public static SpellCastResult ProcessCast(string incantationText)
        {
            SpellStructure spellStructure = ProcessIncantation(incantationText);
            if (spellStructure == null)
                return SpellCastResult.IncantationInvalid;
            if (spellStructure.SpellName.Length == 0)
                return SpellCastResult.IncantationInvalid;

            Spell spell = Spellwright.instance.spellLibrary.GetSpellByIncantation(spellStructure.SpellName);
            if (spell == null)
                return SpellCastResult.IncantationInvalid;

            bool isModifiersApplicable = spell.IsModifiersApplicable(spellStructure.SpellModifiers);
            if (!isModifiersApplicable)
                return SpellCastResult.ModifiersInvalid;

            bool isSpellDataValid = spell.ProcessExtraData(spellStructure, out object extraData);
            if (!isSpellDataValid)
                return SpellCastResult.ArgumentInvalid;

            var spellData = new SpellData(spellStructure.SpellModifiers, spellStructure.Argument, extraData);
            SpellwrightPlayer spellwrightPlayer = SpellwrightPlayer.Instance;
            if (spell.Type == SpellType.Invocation)
            {
                Player player = Main.LocalPlayer;
                int playerLevel = spellwrightPlayer.PlayerLevel;
                if (!spell.ConsumeReagents(player, playerLevel, spellData))
                    return SpellCastResult.IncantationInvalid;
                spell.Cast(player, playerLevel, spellData);
            }
            else if (spell.Type == SpellType.Spell)
            {
                spellwrightPlayer.GuaranteedUsesLeft = spell.GetGuaranteedUses(spellwrightPlayer.PlayerLevel);
                spellwrightPlayer.CurrentSpell = spell;
                spellwrightPlayer.SpellData = spellData;
            }
            else
            {
                spellwrightPlayer.CurrentCantrip = spell;
                spellwrightPlayer.CantripData = spellData;
            }
            return SpellCastResult.Success;
        }

        private static SpellStructure ProcessIncantation(string incantationText)
        {
            incantationText = incantationText.ToLower();

            var incantationParts = incantationText.Split(new[] { ':' }, 2);
            if (incantationParts.Length == 0)
                return null;

            string spellFunctionalPart = incantationParts[0];
            string spellArgument = incantationParts.Length > 1 ? incantationParts[1].Trim() : "";

            var words = spellFunctionalPart.Split(new[] { ' ' });
            if (words.Length == 0)
                return null;

            var spellModifiers = new List<SpellModifier>();
            var spellParts = new List<string>();

            foreach (var word in words)
            {
                var modifier = SpellModifiersProcessor.GetModifier(word);

                if (modifier != null)
                    spellModifiers.Add(modifier.Value);
                else
                    spellParts.Add(word);
            }

            var spellName = string.Join(' ', spellParts);

            return new SpellStructure(spellModifiers, spellName, spellArgument);
        }
    }
}
