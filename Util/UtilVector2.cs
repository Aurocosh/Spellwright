using Microsoft.Xna.Framework;
using Terraria;

namespace Spellwright.Util
{
    internal static class UtilVector2
    {
        public static Vector2 GetPointOnRing(Vector2 center, int minRadius, int maxRadius)
        {
            return center + RandomVector(minRadius, maxRadius);
        }
        public static Vector2 GetPointOnEllipse(Vector2 center, float width, float height)
        {
            Vector2 vector = RandomVector(1);
            vector.X *= width / 2f;
            vector.Y *= height / 2f;
            return center + vector;
        }

        public static Vector2 RandomVector(float length)
        {
            return UtilRandom.NextVector2() * length;
        }
        public static Vector2 RandomVector(float minLength, float maxLength)
        {
            float length = UtilRandom.NextFloat(minLength, maxLength);
            return UtilRandom.NextVector2() * length;
        }

        public static Vector2 RandomVector(Vector2 from, Vector2 to, float minLength, float maxLength)
        {
            float scale = UtilRandom.NextFloat(.1f, 2.5f);
            Vector2 vector = to - from;
            vector.Normalize();
            vector *= scale;
            return vector;
        }

        public static Vector2 RandomVector(Vector2 from, Vector2 to, float minLength, float maxLength, int minRotation, int maxRotation)
        {
            int rotation = UtilRandom.NextInt(minRotation, maxRotation);
            Vector2 vector = RandomVector(from, to, minLength, maxLength);
            return vector.RotatedBy(MathHelper.ToRadians(rotation));
        }
    }
}
