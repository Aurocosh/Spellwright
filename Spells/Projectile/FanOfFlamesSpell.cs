using Spellwright.Spells.Base;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells
{
    internal class FanOfFlamesSpell : ProjectileSpraySpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 10 + 2 * playerLevel;
        protected override int GetDamage(int playerLevel) => 20 + 5 * playerLevel;
        protected override int GetProjectileCount(int playerLevel) => 5 + 1 * playerLevel;
        public FanOfFlamesSpell(string name, string incantation) : base(name, incantation)
        {
            stability = .2f;

            damage = 30;
            knockback = 10;
            damageType = DamageClass.Magic;
            projectileType = ProjectileID.ImpFireball;
            projectileSpeed = 20;
            canAutoReuse = false;
            useTimeMultiplier = 3f;

            projectileCount = 5;
            projectileSpray = 15;
            minSpeedChange = .2f;
            maxSpeedChange = .35f;
        }
    }
}
