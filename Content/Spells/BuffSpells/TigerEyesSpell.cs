using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class TigerEyesSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            AddEffect(BuffID.Hunter, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
