using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Spells.BuffSpells
{
    internal class TigerEyesSpell : BuffSpell
    {
        public TigerEyesSpell(string name, string incantation) : base(name, incantation)
        {
            AddEffect(BuffID.Hunter, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
