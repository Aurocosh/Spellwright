using Spellwright.Spells.Base;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells
{
    internal class FireBallSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 25 + 5 * playerLevel;
        protected override int GetDamage(int playerLevel) => 20 + 5 * playerLevel;

        public FireBallSpell(string name, string incantation) : base(name, incantation)
        {
            stability = .85f;

            damage = 20;
            knockback = 10;
            damageType = DamageClass.Magic;
            projectileType = ProjectileID.ImpFireball;
            projectileSpeed = 40;
            canAutoReuse = false;
            useTimeMultiplier = 3f;
        }
    }
}
