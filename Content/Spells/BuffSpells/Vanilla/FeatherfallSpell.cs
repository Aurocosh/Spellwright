using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Vanilla
{
    internal class FeatherfallSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            AddEffect(BuffID.Featherfall, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            UnlockCost = new SingleItemSpellCost(ItemID.FeatherfallPotion, 10);
            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 3);
        }
    }
}
