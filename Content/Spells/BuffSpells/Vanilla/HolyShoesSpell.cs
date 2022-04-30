using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Vanilla
{
    internal class HolyShoesSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            AddEffect(BuffID.WaterWalking, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            UnlockCost = new SingleItemSpellCost(ItemID.WaterWalkingPotion, 10);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 3);
        }
    }
}
