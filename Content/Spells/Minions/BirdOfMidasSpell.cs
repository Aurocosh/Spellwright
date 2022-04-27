using Spellwright.Content.Buffs.Minions;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Minions;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Minions
{
    internal class BirdOfMidasSpell : MinionSummonSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Invocation;
            damage = 20;
            maxSummonCount = 1;
            buffType = ModContent.BuffType<BirdOfMidasBuff>();
            projectileType = ModContent.ProjectileType<BirdOfMidasMinion>();

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.GoldCoin, 10)
                .WithCost(ItemID.Feather, 1);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 5);
        }
    }
}
