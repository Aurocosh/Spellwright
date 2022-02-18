using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellwright.Util;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class DragonSpitProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon spit");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
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
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;

            AIType = ProjectileID.Shuriken;
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

            int radius = 4;
            int damage = 20;

            UtilExplosion.ExplodeTiles(position, radius, false);
            UtilExplosion.DealExplosionDamage(Projectile, damage, radius);
            Projectile.Kill();

            SoundEngine.PlaySound(SoundID.Item14, position);

            int width = Projectile.width;
            int height = Projectile.height;
            float dustWidth = Projectile.width / 2f;

            for (int i = 0; i < 4; i++)
            {
                var dust = Dust.NewDustDirect(position, width, height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                dust.position = position + UtilVector2.RandomVector(0, dustWidth);
            }

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(position, width, height, DustID.Torch, 0f, 0f, 200, default, 2.7f);
                dust.position = position + UtilVector2.RandomVector(0, dustWidth);
                dust.noGravity = true;
                dust.velocity *= 3f;

                var dust2 = Dust.NewDustDirect(position, width, height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                dust2.position = position + UtilVector2.RandomVector(0, dustWidth);
                dust2.noGravity = true;
                dust2.velocity *= 2f;
                dust2.fadeIn = 2.5f;
            }

            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(position, width, height, DustID.Torch, 0f, 0f, 0, default, 2.7f);
                dust.position = position + UtilVector2.RandomVector(dustWidth).RotatedBy(Projectile.velocity.ToRotation());
                dust.noGravity = true;
                dust.velocity *= 3f;
            }

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(position, width, height, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                dust.position = position + UtilVector2.RandomVector(dustWidth).RotatedBy(Projectile.velocity.ToRotation());
                dust.noGravity = true;
                dust.velocity *= 3f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            var drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void Kill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}