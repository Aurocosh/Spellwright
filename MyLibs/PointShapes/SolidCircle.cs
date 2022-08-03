using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Spellwright.Lib.PointShapes
{
    public class SolidCircle : ICoordinateShape
    {
        private readonly long radiusSquared;

        public int Radius { get; }
        public Point Center { get; }

        public SolidCircle(Point origin, int radius)
        {
            radiusSquared = radius * radius;

            Center = origin;
            Radius = radius;
        }

        public SolidCircle(int x, int y, int radius) : this(new Point(x, y), radius)
        {
        }

        public IEnumerable<Point> Iterator => Iterate(Center);

        private IEnumerable<Point> Iterate(Point origin)
        {
            for (int x = -Radius; x <= Radius; x++)
                for (int y = -Radius; y <= Radius; y++)
                    if (IsInBounds(x, y))
                        yield return origin + new Point(x, y);
        }

        public IEnumerator<Point> GetEnumerator() => Iterator.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsInBounds(int x, int y)
        {
            return x * x + y * y < radiusSquared;
        }

        public bool IsInBounds(Point point)
        {
            return point.DistanceToSq(Center) < radiusSquared;
        }
    }
}
