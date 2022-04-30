using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class KissOfCloverSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            int buffId = ModContent.BuffType<KissOfCloverBuff>();
            AddEffect(buffId, (playerLevel) => UtilTime.MinutesToTicks(5 + 2.5f * playerLevel));
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 30);
        }
    }
}
