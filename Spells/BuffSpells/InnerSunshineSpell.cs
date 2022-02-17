using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Spells.BuffSpells
{
    internal class InnerSunshineSpell : BuffSpell
    {
        public InnerSunshineSpell(string name, string incantation) : base(name, incantation)
        {
            AddEffect(BuffID.Shine, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
