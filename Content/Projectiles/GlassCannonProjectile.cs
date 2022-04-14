using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class GlassCannonProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GlassCannon");
        }

        public override void SetDefaults()
        {
            Projectile.width = 3;
            Projectile.height = 3;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 500;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;

            AIType = ProjectileID.Shuriken;
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);

            int dustType = 206;
            //int dustType = DustID.Torch;
            for (int i = 0; i < 30; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, 0, 0, dustType, 0f, 0f, 0, new Color(255, 255, 255, 255), 1f);
                dust.velocity = Main.rand.NextVector2Unit().ScaleRandom(1f, 5f);
                dust.scale *= 1f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var player = Main.player[Projectile.owner];
            int damage = (int)(player.statLifeMax2 * .8f);
            player.Hurt(PlayerDeathReason.ByCustomReason("Shattered to pieces"), damage, 0, false, true);
            return base.OnTileCollide(oldVelocity);
        }
    }
}