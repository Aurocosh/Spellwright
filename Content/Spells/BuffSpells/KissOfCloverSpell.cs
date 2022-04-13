using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class KissOfCloverSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            int buffId = ModContent.BuffType<KissOfCloverBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            AddApplicableModifier(SpellModifier.IsDispel);
            AddApplicableModifier(SpellModifier.IsEternal);
        }
    }
}
