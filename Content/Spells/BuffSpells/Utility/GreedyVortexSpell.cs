using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class GreedyVortexSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 9;
            AddEffect(ModContent.BuffType<GreedyVortexBuff>(), (playerLevel) => UtilTime.MinutesToTicks(3 + (int)(1.5f * playerLevel)));
        }
    }
}
