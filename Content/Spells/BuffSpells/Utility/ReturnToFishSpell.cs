using Spellwright.Content.Buffs.Spells.Utility;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Utility
{
    internal class ReturnToFishSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;

            static int durationGetter(int playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel);
            AddEffect(ModContent.BuffType<ReturnToFishBuff>(), durationGetter);
            AddApplicableModifier(ModifierConstants.EternalModifiers);

            UnlockCost = new SingleItemSpellCost(ItemID.SharkFin, 5);
            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 2);
        }
    }
}
