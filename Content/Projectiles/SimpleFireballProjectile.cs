using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Spellwright.Util;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace Spellwright.Content.Projectiles
{
    internal class SimpleFireballProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.aiStyle = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 100;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;

            AIType = ProjectileID.Bullet; // Act exactly like default Bullet
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                int particlesToSpawn = 1;
                if (Projectile.localAI[0] > 5f)
                    particlesToSpawn = 2;

                for (int i = 0; i < particlesToSpawn; i++)
                {
                    var dustPosition = new Vector2(Projectile.position.X, Projectile.position.Y + 2f);
                    Dust dust = Dust.NewDustDirect(dustPosition, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
                    dust.noGravity = true;
                    dust.velocity.X *= 0.3f;
                    dust.velocity.Y *= 0.3f;
                    dust.noLight = true;
                }

                if (Projectile.wet && !Projectile.lavaWet)
                {
                    Projectile.Kill();
                    return;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int burnSeconds = Main.rand.Next(3, 7);
            target.AddBuff(BuffID.OnFire, UtilTime.SecondsToTicks(burnSeconds));
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int burnSeconds = Main.rand.Next(3, 7);
            target.AddBuff(BuffID.OnFire, UtilTime.SecondsToTicks(burnSeconds));
        }
    }
}
