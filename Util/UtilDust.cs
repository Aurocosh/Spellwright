using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Spellwright.Util
{
    internal static class UtilDust
    {
        public static void SpawnExplosionDust(Vector2 position, Vector2 projectileVelocity, int dustType, Color color, float radius, int particleCoeff)
        {
            for (int i = 0; i < 4 * particleCoeff; i++)
            {
                var dust = Dust.NewDustDirect(position, 0, 0, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                dust.position = position + UtilVector2.RandomVector(0, radius);
            }

            for (int i = 0; i < 10 * particleCoeff; i++)
            {
                var dust = Dust.NewDustDirect(position, 0, 0, dustType, 0f, 0f, 200, color, 2.7f);
                dust.position = position + UtilVector2.RandomVector(0, radius);
                dust.noGravity = true;
                dust.velocity *= 3f;

                var dust2 = Dust.NewDustDirect(position, 0, 0, dustType, 0f, 0f, 100, color, 1.5f);
                dust2.position = position + UtilVector2.RandomVector(0, radius);
                dust2.noGravity = true;
                dust2.velocity *= 2f;
                dust2.fadeIn = 2.0f;
            }

            for (int i = 0; i < 5 * particleCoeff; i++)
            {
                var dust = Dust.NewDustDirect(position, 0, 0, dustType, 0f, 0f, 0, color, 2.7f);
                dust.position = position + UtilVector2.RandomVector(radius).RotatedBy(projectileVelocity.ToRotation());
                dust.noGravity = true;
                dust.velocity *= 3f;
            }

            for (int i = 0; i < 10 * particleCoeff; i++)
            {
                var dust = Dust.NewDustDirect(position, 0, 0, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                dust.position = position + UtilVector2.RandomVector(radius).RotatedBy(projectileVelocity.ToRotation());
                dust.noGravity = true;
                dust.velocity *= 3f;
            }
        }
    }
}
