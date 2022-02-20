using Spellwright.Content.Buffs.Spells;
using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class GaleForceSpell : BuffSpell
    {
        public GaleForceSpell(string name, string incantation) : base(name, incantation)
        {
            AddEffect(ModContent.BuffType<GaleForceBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
