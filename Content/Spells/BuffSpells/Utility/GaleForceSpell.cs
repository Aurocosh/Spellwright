using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class GaleForceSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            AddEffect(ModContent.BuffType<GaleForceBuff>(), (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            AddApplicableModifier(SpellModifier.IsDispel);
            AddApplicableModifier(SpellModifier.IsEternal);
        }
    }
}
