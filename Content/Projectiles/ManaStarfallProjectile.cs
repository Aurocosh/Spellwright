using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class ManaStarfallProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana starfall");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = ProjAIStyleID.FallingStar;
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
            Item.NewItem(new EntitySource_DropAsItem(Projectile), Projectile.position, ItemID.Star, 1, false, 0, true);
        }

        private void DoDeathEffects()
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            Color dustColor = Color.CornflowerBlue;
            if (Main.tenthAnniversaryWorld)
            {
                dustColor = Color.HotPink;
                dustColor.A /= 2;
            }

            for (int i = 0; i < 7; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Pink, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 0.8f);
            }

            for (float i = 0f; i < 1f; i += 0.125f)
                Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, Vector2.UnitY.RotatedBy(i * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, dustColor).noGravity = true;

            for (float i = 0f; i < 1f; i += 0.25f)
                Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, Vector2.UnitY.RotatedBy(i * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;

            var mainSreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + mainSreenSize / 2f, mainSreenSize + new Vector2(400f))))
            {
                var source = new EntitySource_Parent(Projectile);
                for (int i = 0; i < 7; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length();
                    int type = Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17);
                    //Gore.NewGore(source, Projectile.position, velocity, type); // Preview
                    Gore.NewGore(Projectile.position, velocity, type);
                }
            }
        }
    }
}