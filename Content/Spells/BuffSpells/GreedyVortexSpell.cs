using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class GreedyVortexSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            AddEffect(ModContent.BuffType<GreedyVortexBuff>(), (playerLevel) => UtilTime.MinutesToTicks(3 + (int)(1.5f * playerLevel)));
        }
    }
}
