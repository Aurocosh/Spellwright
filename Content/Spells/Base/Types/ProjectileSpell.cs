using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class ProjectileSpell : ModSpell
    {
        protected int projectileType;
        protected float projectileSpeed;

        protected virtual int GetProjectileType(int playerLevel) => projectileType;
        public virtual float GetProjectileSpeed(int playerLevel) => projectileSpeed;

        public ProjectileSpell()
        {
            UseType = SpellType.Spell;

            damage = 0;
            knockback = 0;
            damageType = DamageClass.Magic;
            projectileType = ProjectileID.WoodenArrowFriendly;
            projectileSpeed = 10;
            canAutoReuse = true;
            useTimeMultiplier = 1f;
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            direction.Normalize();
            Vector2 velocity = direction * projectileSpeed;

            int realDamage = GetDamage(playerLevel);
            float realKnockback = GetKnockback(playerLevel);
            int realProjectileType = GetProjectileType(playerLevel);
            int projectileID = Projectile.NewProjectile(source, position, velocity, realProjectileType, realDamage, realKnockback, player.whoAmI);
            //Projectile projectile = Main.projectile[projectileID];

            return true;
        }
    }
}
