using Spellwright.Content.Projectiles;
using Spellwright.Spells.Base;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells
{
    internal class StoneBulletSpell : ProjectileSpell
    {
        protected override int GetDamage(int playerLevel) => 15 + 5 * playerLevel;
        public override float GetUseSpeedMultiplier(int playerLevel) => 5f + 0.5f * playerLevel;

        public StoneBulletSpell(string name, string incantation) : base(name, incantation)
        {
            stability = 1.0f;
            knockback = 2;
            damageType = DamageClass.Throwing;
            projectileType = ModContent.ProjectileType<StoneBulletProjectile>();
            projectileSpeed = 450;
            canAutoReuse = true;
            reagentType = ItemID.StoneBlock;
            reagentUseCost = 1;
        }
    }
}
