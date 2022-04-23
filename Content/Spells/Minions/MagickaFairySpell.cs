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
    internal class MagickaFairySpell : MinionSummonSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 10;
            UseType = SpellType.Invocation;
            damage = 0;
            maxSummonCount = 1;
            buffType = ModContent.BuffType<MagickaFairyBuff>();
            projectileType = ModContent.ProjectileType<MagickaFairyMinion>();

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.FairyBell, 1)
                .WithCost(ItemID.PixieDust, 50);

            SpellCost = new SingleItemSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 3);
        }
    }
}
