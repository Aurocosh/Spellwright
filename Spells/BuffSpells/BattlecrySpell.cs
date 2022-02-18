using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Spells.BuffSpells
{
    internal class BattlecrySpell : BuffSpell
    {
        public BattlecrySpell(string name, string incantation) : base(name, incantation)
        {
            AddEffect(BuffID.Battle, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.IsAoe);
        }
    }
}
