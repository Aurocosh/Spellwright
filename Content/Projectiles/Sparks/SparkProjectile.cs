using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles.Sparks
{
    public abstract class SparkProjectile : ModProjectile
    {
        protected int dustType;

        protected SparkProjectile(int dustType)
        {
            this.dustType = dustType;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spark");
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.damage = 10;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.light = .3f;
            Projectile.alpha = 255;
            Projectile.penetrate = 2;
            //Projectile.aiStyle = 2;
            Projectile.noEnchantmentVisuals = true;
        }

        public override void AI()
        {
            Projectile.alpha = 255;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3f)
            {
                int dustProbability = 100;
                if (Projectile.ai[0] > 20f)
                {
                    int idk = 40;
                    float idk2 = Projectile.ai[0] - 20f;
                    dustProbability = (int)(100f * (1f - idk2 / (float)idk));
                    if (idk2 >= (float)idk)
                        Projectile.Kill();
                }

                if (Projectile.ai[0] <= 10f)
                    dustProbability = (int)Projectile.ai[0] * 10;

                if (Main.rand.Next(100) < dustProbability)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 150);
                    dust.position = (dust.position + Projectile.Center) / 2f;
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust.scale *= 1.2f;
                    dust.velocity += Projectile.velocity;
                }
            }
        }
    }
}