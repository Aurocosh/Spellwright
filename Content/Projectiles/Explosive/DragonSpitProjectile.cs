using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.PointShapes;
using Spellwright.Util;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles.Explosive
{
    public class DragonSpitProjectile : ModProjectile
    {
        private bool _exploded = false;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dragon spit");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 600;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;

            AIType = ProjectileID.Shuriken;
        }
        public override void AI()
        {
            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 50, Color.Yellow, 1.2f);
            dust.noGravity = true;
            int max = Main.rand.Next(0, 3) == 1 ? 1 : 3;
            dust.velocity = Main.rand.NextVector2Circular(1, 1) * max;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Explode();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
            if (_exploded)
                return;
            _exploded = true;

            int radius = 4;
            int damage = 20;

            Vector2 position = Projectile.Center;
            var start = position.ToGridPoint();

            var circlePoints = new SolidCircle(start, radius);
            UtilExplosion.ExplodeTiles(circlePoints, false);
            UtilExplosion.DealExplosionDamage(Projectile, damage, radius);

            UtilDust.SpawnExplosionDust(position, Projectile.velocity, DustID.Torch, Color.Red, 30, 1);
            SoundEngine.PlaySound(SoundID.Item14, position);
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            //Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}