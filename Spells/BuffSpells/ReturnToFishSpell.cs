using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ID;

namespace Spellwright.Spells.BuffSpells
{
    internal class ReturnToFishSpell : BuffSpell
    {
        public override int SpellLevel => 3;

        public ReturnToFishSpell(string name, string incantation) : base(name, incantation)
        {
            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel);

            AddEffect(BuffID.Gills, durationGetter);
            AddEffect(BuffID.Flipper, durationGetter);
        }
    }
}
