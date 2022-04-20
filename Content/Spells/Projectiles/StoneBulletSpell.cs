using Spellwright.Content.Projectiles;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Projectiles
{
    internal class StoneBulletSpell : ProjectileSpell
    {
        protected override int GetDamage(int playerLevel) => 15 + 5 * playerLevel;
        public override float GetUseSpeedMultiplier(int playerLevel) => 5f + 0.5f * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            stability = 1.0f;
            knockback = 2;
            damageType = DamageClass.Throwing;
            projectileType = ModContent.ProjectileType<StoneBulletProjectile>();
            projectileSpeed = 450;
            canAutoReuse = true;
            SpellCost = new SingleItemSpellCost(ItemID.StoneBlock, 1);
        }
    }
}
