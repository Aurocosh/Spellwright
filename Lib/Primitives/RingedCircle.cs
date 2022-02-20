using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Spellwright.Lib.Primitives
{
    public class RingedCircle : ICoordinateShape
    {
        private readonly int innerRadiusSquared;
        private readonly int outerRadiusSquared;
        public int InnerRadius { get; }
        public int OuterRadius { get; }
        public Point Center { get; }

        public RingedCircle(Point origin, int innerRadius, int outerRadius)
        {
            if (innerRadius > outerRadius)
                throw new ArgumentException("Inner radius cannot be bigger then outer radius");

            Center = origin;

            InnerRadius = innerRadius;
            OuterRadius = outerRadius;

            innerRadiusSquared = innerRadius * innerRadius;
            outerRadiusSquared = outerRadius * outerRadius;
        }

        public RingedCircle(int x, int y, int innerRadius, int outerRadius) : this(new Point(x, y), innerRadius, outerRadius)
        {
        }

        public IEnumerable<Point> Iterator => Iterate(Center);

        private IEnumerable<Point> Iterate(Point origin)
        {
            for (int x = -OuterRadius; x <= OuterRadius; x++)
                for (int y = -OuterRadius; y <= OuterRadius; y++)
                    if (IsInBounds(x, y))
                        yield return origin + new Point(x, y);
        }

        public IEnumerator<Point> GetEnumerator() => Iterator.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsInBounds(int x, int y)
        {
            int radiusSq = x * x + y * y;
            return innerRadiusSquared < radiusSq && radiusSq < outerRadiusSquared;
        }

        public bool IsInBounds(Point point)
        {
            int radiusSq = point.DistanceSq();
            return innerRadiusSquared < radiusSq && radiusSq < outerRadiusSquared;
        }
    }
}
