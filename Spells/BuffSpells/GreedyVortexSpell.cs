using Spellwright.Content.Buffs;
using Spellwright.Spells.Base;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class GreedyVortexSpell : BuffSpell
    {
        public GreedyVortexSpell(string name, string incantation) : base(name, incantation)
        {
            AddEffect(ModContent.BuffType<GreedyVortexBuff>(), (playerLevel) => UtilTime.MinutesToTicks(3 + (int)(1.5f * playerLevel)));
        }
    }
}
