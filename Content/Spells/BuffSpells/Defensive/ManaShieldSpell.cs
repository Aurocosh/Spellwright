using Spellwright.Content.Buffs.Spells.Defensive;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells.Defensive
{
    internal class ManaShieldSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 7;
            AddEffect(ModContent.BuffType<ManaShieldBuff>(), (playerLevel) => UtilTime.MinutesToTicks(2 * playerLevel));

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.ManaFlower, 1)
                .WithCost(ItemID.SuperManaPotion, 30);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 6);
        }
    }
}
