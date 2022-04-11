using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class ManaStarfallSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            AddEffect(ModContent.BuffType<ManaStarfallBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + playerLevel));
        }
    }
}
