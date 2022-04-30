using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class ForceOfCreationSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            AddEffect(ModContent.BuffType<ForceOfCreationBuff>(), (playerLevel) => UtilTime.MinutesToTicks((int)(2f * playerLevel)));
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            UnlockCost = new SingleItemSpellCost(ItemID.StoneBlock, 999);
            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 2);
        }
    }
}
