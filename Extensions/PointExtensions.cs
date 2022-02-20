using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Spellwright.Extensions
{
    internal static class PointExtensions
    {
        public static Point PerpendicularClockwise(this Point point)
        {
            return new Point(point.Y, -point.X);
        }

        public static Point PerpendicularCounterClockwise(this Point point)
        {
            return new Point(-point.Y, point.X);
        }

        public static double Distance(this Point point)
        {
            return Math.Sqrt(point.DistanceSq());
        }

        public static int DistanceSq(this Point point)
        {
            return point.X * point.X + point.Y * point.Y;
        }
        public static Vector2 ToWorldVector2(this Point point)
        {
            return point.ToVector2() * 16f;
        }
    }
}
