using Spellwright.Content.Buffs.Spells;
using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class ManaStarfallSpell : BuffSpell
    {
        public ManaStarfallSpell(string name, string incantation) : base(name, incantation)
        {
            AddEffect(ModContent.BuffType<ManaStarfallBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + playerLevel));
        }
    }
}
