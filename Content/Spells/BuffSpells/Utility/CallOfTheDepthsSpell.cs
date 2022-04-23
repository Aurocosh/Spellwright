using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class CallOfTheDepthsSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            AddEffect(ModContent.BuffType<CallOfTheDepthsBuff>(), (playerLevel) => UtilTime.MinutesToTicks((int)(2f * playerLevel)));
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            UnlockCost = new OptionalSpellCost()
                .WithCost(ItemID.StoneBlock, 200)
                .WithCost(ItemID.Diamond, 5);

            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 20);
        }
    }
}
