using Microsoft.Xna.Framework;
using Spellwright.Core.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class ShockwaveDartProjectile : ModProjectile
    {
        private static LegacySoundStyle deathSound = SoundID.Item14.WithPitchVariance(.3f).WithVolume(.8f);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shockwave Dart");
        }

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;

            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            var shockwaveHandler = new ShockwaveHandler();
            Vector2 centerPosition = Projectile.Center;
            shockwaveHandler.DoShockwave(centerPosition, 15);
            SoundEngine.PlaySound(deathSound, centerPosition);
        }
    }
}