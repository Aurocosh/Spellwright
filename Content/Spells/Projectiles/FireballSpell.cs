using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class FireballSpell : ProjectileSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 25 + 5 * playerLevel;
        protected override int GetDamage(int playerLevel) => 20 + 5 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
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
