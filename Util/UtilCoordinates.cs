using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Spellwright.Util
{
    internal class UtilCoordinates
    {
        public static IEnumerable<Point> FloodFill(IEnumerable<Point> start, IEnumerable<Point> expansionDirs, Func<Point, bool> predicate, int limit)
        {
            var expansionFront = new LinkedList<Point>(start);
            var explored = new HashSet<Point>(start);

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

        public static IEnumerable<Point> LookBackFloodFill<T>(IEnumerable<Point> start, T startingData, IEnumerable<Point> expansionDirs, Func<Point, T, (bool, T)> predicate, int limit)
        {
            var expansionFront = new LinkedList<(Point point, T data)>();
            foreach (var point in start)
                expansionFront.AddLast((point, startingData));
            var explored = new HashSet<Point>(start);

            while (expansionFront.Count > 0 && explored.Count < limit)
            {
                (Point point, T prevData) = expansionFront.Last.Value;
                expansionFront.RemoveLast();

                (bool valid, T data) = predicate.Invoke(point, prevData);
                if (!valid)
                    continue;

                yield return point;

                foreach (var expansionDir in expansionDirs)
                {
                    var neighbour = point + expansionDir;
                    if (!explored.Contains(neighbour))
                    {
                        expansionFront.AddFirst((neighbour, data));
                        explored.Add(neighbour);
                    }
                }
            }
        }
    }
}
