using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Spellwright.Util
{
    internal class UtilCoordinates
    {
        public static IEnumerable<Point> FloodFill(Point start, IEnumerable<Point> expansionDirs, Func<Point, bool> predicate, int limit)
        {
            yield return start;

            var expansionFront = new LinkedList<Point>();
            expansionFront.AddFirst(start);
            var explored = new HashSet<Point>() { start };

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
