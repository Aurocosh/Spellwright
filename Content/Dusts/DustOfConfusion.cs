using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Dusts
{
    public class DustOfConfusion : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
            dust.noGravity = true; // Makes the dust have no gravity.
            dust.noLight = true; // Makes the dust emit no light.
            dust.scale *= 0.8f; // Multiplies the dust's initial scale by 1.5.
        }

        public override bool Update(Dust dust)
        { // Calls every frame the dust is active
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.99f;




            //float light = 0.35f * dust.scale;

            //Lighting.AddLight(dust.position, light, light, light);

            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }


            if (dust.customData != null && dust.customData is Player)
            {
                Player player = (Player)dust.customData;
                dust.position += player.position - player.oldPosition;
            }
            else if (dust.customData != null && dust.customData is Projectile)
            {
                Projectile projectile = (Projectile)dust.customData;
                if (projectile.active)
                    dust.position += projectile.position - projectile.oldPosition;
            }




            float num4 = dust.scale * 0.6f;
            if (num4 > 1f)
                num4 = 1f;

            float num6 = num4 * num4;

            Lighting.AddLight(dust.position, num6, num6, num6);



            float num93 = dust.scale;
            if (num93 > 1f)
                num93 = 1f;

            if (dust.noLight)
                num93 *= 0.1f;

            Lighting.AddLight(dust.position, 0f, num93 * 0.8f, num93);


            return false; // Return false to prevent vanilla behavior.
        }
    }
}
