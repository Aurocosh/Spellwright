using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Spellwright.Lib.PointShapes
{
    public interface ICoordinateShape : IEnumerable<Point>
    {
        bool IsInBounds(int x, int y);
        bool IsInBounds(Point point);

        IEnumerable<Point> Intersect(ICoordinateShape otherShape)
        {
            foreach (var point in otherShape)
            {
                if (IsInBounds(point))
                    yield return point;
            }
        }
    }
}
