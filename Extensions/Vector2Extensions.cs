using Microsoft.Xna.Framework;
using Terraria;

namespace Spellwright.Extensions
{
    internal static class Vector2Extensions
    {
        public static Vector2 PerpendicularClockwise(this Vector2 vector2)
        {
            return new Vector2(vector2.Y, -vector2.X);
        }

        public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
        {
            return new Vector2(-vector2.Y, vector2.X);
        }

        public static Point ToGridPoint(this Vector2 vector2)
        {
            int x = (int)(vector2.X / 16f);
            int y = (int)(vector2.Y / 16f);

            return new Point(x, y);
        }
        public static Vector2 ScaleRandom(this Vector2 vector, float minScale, float maxScale)
        {
            float scale = Main.rand.NextFloat(minScale, maxScale);
            vector *= scale;
            return vector;
        }
    }
}
