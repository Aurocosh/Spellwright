using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class BattlecrySpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            AddEffect(BuffID.Battle, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            RemoveApplicableModifier(SpellModifier.IsAoe);
        }
    }
}
