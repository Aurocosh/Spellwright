using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using System;
using System.Collections.Generic;

namespace Spellwright.Util
{
    internal class UtilCoordinates
    {
        public static IEnumerable<Point> GetPointsInCircle(Point center, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int radiusSq = radius * radius;
                    var point = new Point(x, y);
                    if (point.DistanceSq() <= radiusSq)
                        yield return center + point;
                }
            }
        }

        public static IEnumerable<Point> GetPointsInSqRadius(Point center, int radiusX, int radiusY)
        {
            for (int x = -radiusX; x <= radiusX; x++)
                for (int y = -radiusY; y <= radiusY; y++)
                    yield return center + new Point(x, y);
        }

        public static IEnumerable<Point> FloodFill(Point start, IEnumerable<Point> expansionDirs, Func<Point, Boolean> predicate, int limit)
        {
            var expansionFront = new LinkedList<Point>();
            expansionFront.AddFirst(start);
            var explored = new HashSet<Point>() { start };
            //var result = new List<Point>();

            while (expansionFront.Count > 0 && explored.Count < limit)
            {
                var nextPos = expansionFront.Last.Value;
                expansionFront.RemoveLast();
                if (!predicate.Invoke(nextPos))
                    continue;

                yield return nextPos;

                foreach (var expansionDir in expansionDirs)
                {
                    var neighbour = nextPos + expansionDir;
                    if (!explored.Contains(neighbour))
                    {
                        expansionFront.AddFirst(neighbour);
                        explored.Add(neighbour);
                    }
                }
            }
        }
    }
}
