using Spellwright.Content.Buffs.Minions;
using Spellwright.Content.Minions;
using Spellwright.Spells.Base;
using Terraria.ModLoader;

namespace Spellwright.Spells.BuffSpells
{
    internal class BirdOfMidasSpell : MinionSummonSpell
    {
        public BirdOfMidasSpell(string name, string incantation) : base(name, incantation)
        {
            damage = 2;
            maxSummonCount = 1;
            buffType = ModContent.BuffType<BirdOfMidasBuff>();
            projectileType = ModContent.ProjectileType<BirdOfMidasMinion>();
        }
    }
}
