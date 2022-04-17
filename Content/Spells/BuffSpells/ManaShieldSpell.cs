using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class ManaShieldSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            AddEffect(ModContent.BuffType<ManaShieldBuff>(), (playerLevel) => UtilTime.MinutesToTicks(2 * playerLevel));
        }
    }
}
