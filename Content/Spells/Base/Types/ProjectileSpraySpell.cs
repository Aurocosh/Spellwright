using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class ProjectileSpraySpell : ProjectileSpell
    {
        protected int projectileCount;
        protected int projectileSpray;
        protected float minSpeedChange;
        protected float maxSpeedChange;
        protected virtual int GetProjectileCount(int playerLevel) => projectileCount;
        protected virtual int GetProjectileSpray(int playerLevel) => projectileSpray;

        public ProjectileSpraySpell()
        {
            UseType = SpellType.Spell;

            projectileCount = 3;
            projectileSpray = 10;
            minSpeedChange = .3f;
            maxSpeedChange = .3f;
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            direction.Normalize();
            Vector2 velocity = direction * projectileSpeed;

            int projectileCount = GetProjectileCount(playerLevel);
            for (int i = 0; i < projectileCount; i++)
            {
                int realDamage = GetDamage(playerLevel);
                float realKnockback = GetKnockback(playerLevel);
                int realProjectileType = GetProjectileType(playerLevel);
                int projectileSpray = GetProjectileSpray(playerLevel);
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(projectileSpray));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(minSpeedChange, maxSpeedChange);

                int projectileID = Projectile.NewProjectile(source, position, newVelocity, realProjectileType, realDamage, realKnockback, player.whoAmI);
            }

            return true;
        }

        public override List<SpellParameter> GetDescriptionValues(Player player, int playerLevel, SpellData spellData, bool fullVersion)
        {
            var values = base.GetDescriptionValues(player, playerLevel, spellData, fullVersion);
            int projectileSpray = GetProjectileSpray(playerLevel);
            values.Add(new SpellParameter("Spray", projectileSpray.ToString()));
            int projectileCount = GetProjectileCount(playerLevel);
            values.Add(new SpellParameter("ProjectileCount", projectileCount.ToString()));
            return values;
        }
    }
}
