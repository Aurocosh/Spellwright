using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class CallOfTheDepthsSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            AddEffect(ModContent.BuffType<CallOfTheDepthsBuff>(), (playerLevel) => UtilTime.MinutesToTicks((int)(2f * playerLevel)));

            AddApplicableModifier(SpellModifier.IsDispel);
            AddApplicableModifier(SpellModifier.IsEternal);
        }
    }
}
