using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class InnerSunshineSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            AddEffect(BuffID.Shine, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
