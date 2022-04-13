using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class InnerSunshineSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            AddEffect(BuffID.Shine, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            AddApplicableModifier(SpellModifier.IsDispel);
            AddApplicableModifier(SpellModifier.IsEternal);
        }
    }
}
