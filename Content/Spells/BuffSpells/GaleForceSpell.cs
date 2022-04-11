using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class GaleForceSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            AddEffect(ModContent.BuffType<GaleForceBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
        }
    }
}
