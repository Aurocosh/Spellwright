using Spellwright.Content.Buffs.Spells.Defensive;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Defensive
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
