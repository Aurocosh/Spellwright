using Spellwright.Content.Buffs.Minions;
using Spellwright.Content.Minions;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Stats;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Minions
{
    internal class SoulNibblerSpell : MinionSummonSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 7;
            UseType = SpellType.Invocation;
            damage = 26;
            maxSummonCount = 1;
            buffType = ModContent.BuffType<SoulNibblerBuff>();
            projectileType = ModContent.ProjectileType<SoulNibblerMinion>();

            UnlockCost = new MaxHealthSpellCost(200);
            CastCost = new HealthSpellCost(200);
        }
    }
}
