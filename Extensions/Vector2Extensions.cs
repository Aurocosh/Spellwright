using Microsoft.Xna.Framework;

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
    }
}
