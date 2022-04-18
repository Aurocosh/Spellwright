using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Primitives;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles.Explosive
{
    public class ShapedChargeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shaped charge");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 600;
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = 20;

            AIType = ProjectileID.Shuriken;
        }
        public override void AI()
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }

        private void Explode()
        {
            int damage = 40;
            int radius = 4;

            Vector2 position = Projectile.Center;

            Vector2 shift = Projectile.oldVelocity;
            shift.Normalize();
            shift *= radius * 8;

            var nextCenter = position;
            var explosionCenters = new List<Vector2>();
            var explosionCenterPoints = new List<Point>();
            for (int i = 0; i < 14; i++)
            {
                explosionCenters.Add(nextCenter);
                explosionCenterPoints.Add(nextCenter.ToGridPoint());
                nextCenter += shift;
            }

            var affectedPoints = new HashSet<Point>();
            foreach (Point point in explosionCenterPoints)
            {
                var circlePoints = new SolidCircle(point, radius);
                affectedPoints.UnionWith(circlePoints);
            }

            UtilExplosion.ExplodeTiles(affectedPoints, false);

            float radiusWorld = radius * 16;
            float radiusWorldSq = radiusWorld * radiusWorld;
            bool CanHitEntity(Entity entity)
            {
                foreach (Vector2 explosionCenter in explosionCenters)
                {
                    float distanceSq = Vector2.DistanceSquared(entity.Center, explosionCenter);
                    if (distanceSq < radiusWorldSq)
                        return true;
                }
                return false;
            }
            UtilExplosion.DealExplosionDamage(Projectile, damage, 300, CanHitEntity);

            foreach (Vector2 explosionCenter in explosionCenters)
                UtilDust.SpawnExplosionDust(explosionCenter, Projectile.velocity, DustID.Torch, Color.Red, radius * 16, 1);

            SoundEngine.PlaySound(SoundID.Item14, position);
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            //Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}