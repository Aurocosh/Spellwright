using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class SeaBlessingSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;

            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel);
            AddEffect(ModContent.BuffType<SeaBlessingBuff>(), durationGetter);
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 10);
        }
    }
}
