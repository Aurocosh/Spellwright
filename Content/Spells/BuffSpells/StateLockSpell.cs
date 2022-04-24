using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class StateLockSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 8;

            int buff = ModContent.BuffType<StateLockBuff>();
            AddEffect(buff, (playerLevel) => 10000);

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.LightShard, 5)
                .WithCost(ItemID.DarkShard, 5);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 10);
        }
    }
}
