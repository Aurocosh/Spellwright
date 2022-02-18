using Microsoft.Xna.Framework;
using Spellwright.Util;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class BlastPebbleProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blast pebble");
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 600;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            //Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;

            AIType = ProjectileID.Shuriken;
        }
        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 50, Color.Red, 1.2f);
                dust.noGravity = true;
                int max = UtilRandom.NextInt(0, 3) == 1 ? 1 : 3;
                dust.velocity = UtilVector2.RandomVector(max);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Explode();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Explode();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }

        private void Explode()
        {
            Vector2 position = Projectile.Center;

            int radius = 8;
            int damage = 80;

            UtilExplosion.ExplodeTiles(position, radius, false);
            UtilExplosion.DealExplosionDamage(Projectile, damage, radius);
            Projectile.Kill();

            SoundEngine.PlaySound(SoundID.Item14, position);
            UtilDust.SpawnExplosionDust(position, Projectile.velocity, DustID.Torch, Color.Red, 150, 3);
        }
        public override void Kill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}