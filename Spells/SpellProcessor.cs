using Spellwright.Players;
using Spellwright.Spells.Base;
using Terraria;

namespace Spellwright.Spells
{
    internal static class SpellProcessor
    {
        public static SpellCastResult ProcessCast(string incantationText)
        {
            var incantationParts = incantationText.Split(new[] { ':' }, 2);
            if (incantationParts.Length == 0)
                return SpellCastResult.IncantationInvalid;

            string spellName = incantationParts[0];
            string spellArgument = incantationParts.Length > 1 ? incantationParts[1].Trim() : "";
            Spell spell = Spellwright.instance.spellLibrary.GetSpellByIncantation(spellName);

            if (spell == null)
                return SpellCastResult.IncantationInvalid;

            bool isSpellDataValid = spell.ProcessExtraData(spellArgument, out SpellData spellData);
            if (!isSpellDataValid)
                return SpellCastResult.ArgumentInvalid;

            SpellwrightPlayer spellwrightPlayer = SpellwrightPlayer.Instance;
            if (spell.Type == Base.SpellType.Invocation)
            {
                spell.Cast(Main.LocalPlayer, spellwrightPlayer.PlayerLevel, spellData);
            }
            else if (spell.Type == Base.SpellType.Spell)
            {
                spellwrightPlayer.GuaranteedUsesLeft = spell.GetGuaranteedUses(spellwrightPlayer.PlayerLevel);
                spellwrightPlayer.CurrentSpell = spell;
                spellwrightPlayer.SpellData = spellData;
            }
            else
            {
                spellwrightPlayer.CurrentCantrip = spell;
            }
            return SpellCastResult.Success;
        }
    }
}
