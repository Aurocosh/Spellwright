using Spellwright.Content.Buffs.Minions;
using Spellwright.Content.Minions;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Minions
{
    internal class BirdOfMidasSpell : MinionSummonSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
            damage = 2;
            maxSummonCount = 1;
            buffType = ModContent.BuffType<BirdOfMidasBuff>();
            projectileType = ModContent.ProjectileType<BirdOfMidasMinion>();
        }
    }
}
