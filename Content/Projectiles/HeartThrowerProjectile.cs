using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class HeartThrowerProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart Thrower");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 600;
            Projectile.light = 1f;
            Projectile.alpha = 50;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            DoDeathEffects();
            Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.position, ItemID.Heart, 1, false, 0, true);
        }

        private void DoDeathEffects()
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}