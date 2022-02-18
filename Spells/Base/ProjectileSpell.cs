using Microsoft.Xna.Framework;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells.Base
{
    internal abstract class ProjectileSpell : Spell
    {
        protected int projectileType;
        protected float projectileSpeed;

        protected virtual int GetProjectileType(int playerLevel) => projectileType;
        public virtual float GetProjectileSpeed(int playerLevel) => projectileSpeed;

        public ProjectileSpell(string name, string incantation, SpellType spellType = SpellType.Spell) : base(name, incantation, spellType)
        {
            damage = 0;
            knockback = 0;
            damageType = DamageClass.Magic;
            projectileType = ProjectileID.WoodenArrowFriendly;
            projectileSpeed = 10;
            canAutoReuse = true;
            useTimeMultiplier = 1f;
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
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
