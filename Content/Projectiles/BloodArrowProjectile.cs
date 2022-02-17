using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class BloodArrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood arrow");
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 200;

            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100);
        }
    }
}