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
            projectileSpeed = 3;
            canAutoReuse = true;
            useTimeMultiplier = 1f;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            Vector2 mousePosition = Main.MouseWorld;
            Vector2 center = player.Center + new Vector2(player.width * .2f * player.direction, -player.height * .4f);
            Vector2 velocity = center.DirectionTo(mousePosition);
            var source = new EntitySource_Parent(player);
            SpawnProjectile(player, playerLevel, spellData, source, center, velocity);
            return true;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            SpawnProjectile(player, playerLevel, spellData, source, position, direction);
            return true;
        }

        private void SpawnProjectile(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            PlayUseSound(player.Center);

            direction.Normalize();
            Vector2 velocity = direction * projectileSpeed;
            float damageModifier = player.GetDamage(DamageType);
            int realDamage = (int)(GetDamage(playerLevel) * damageModifier);
            float realKnockback = GetKnockback(playerLevel);
            int realProjectileType = GetProjectileType(playerLevel);
            int projectileID = Projectile.NewProjectile(source, position, velocity, realProjectileType, realDamage, realKnockback, player.whoAmI);
            //Projectile projectile = Main.projectile[projectileID];
        }
    }
}
