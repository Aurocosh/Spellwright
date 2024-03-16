using Spellwright.Content.Dusts;
using Spellwright.Util;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles
{
    public class BoltOfConfusionProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.FrostBoltStaff);

            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.damage = 10;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.light = .3f;
            Projectile.alpha = 255;
            Projectile.aiStyle = 0;
        }
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int type = ModContent.DustType<DustOfConfusion>();
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, UtilTime.SecondsToTicks(15));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Confused, UtilTime.SecondsToTicks(15));
        }

        //public override bool PreDraw(ref Color lightColor)
        //{
        //    Main.instance.LoadProjectile(Projectile.type);
        //    Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

        //    // Redraw the projectile with the color not influenced by light
        //    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
        //    for (int k = 0; k < Projectile.oldPos.Length; k++)
        //    {
        //        Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
        //        Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
        //        Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
        //    }

        //    return true;
        //}

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            int type = ModContent.DustType<DustOfConfusion>();
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type);
                if (!Main.rand.NextBool(3))
                {
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.scale *= 1.75f;
                }
                else
                {
                    dust.scale *= 0.5f;
                }
            }
        }
    }
}