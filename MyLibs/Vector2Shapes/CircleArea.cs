using Microsoft.Xna.Framework;

namespace Spellwright.Lib.Vector2Shapes
{
    internal class CircleArea
    {
        private readonly double radiusSquared;

        public float Radius { get; }
        public Vector2 Center { get; }

        public CircleArea(Vector2 origin, float radius)
        {
            radiusSquared = radius * radius;

            Center = origin;
            Radius = radius;
        }

        public CircleArea(float x, float y, float radius) : this(new Vector2(x, y), radius)
        {
        }

        public bool IsInBounds(float x, float y)
        {
            return x * x + y * y < radiusSquared;
        }

        public bool IsInBounds(Vector2 point)
        {
            return Vector2.DistanceSquared(point, Center) < radiusSquared;
        }
    }
}
