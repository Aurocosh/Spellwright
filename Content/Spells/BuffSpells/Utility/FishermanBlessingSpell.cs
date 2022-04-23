using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class FishermanBlessingSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;

            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel);
            AddEffect(ModContent.BuffType<FishermanBlessingBuff>(), durationGetter);
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }
    }
}
