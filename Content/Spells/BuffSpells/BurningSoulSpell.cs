using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class BurningSoulSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            AddEffect(ModContent.BuffType<BurningSoulBuff>(), (playerLevel) => UtilTime.MinutesToTicks(3 + (int)(1.4f * playerLevel)));
        }
    }
}
