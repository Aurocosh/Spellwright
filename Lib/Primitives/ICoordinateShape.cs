using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Spellwright.Lib.Primitives
{
    public interface ICoordinateShape : IEnumerable<Point>
    {
        bool IsInBounds(int x, int y);
        bool IsInBounds(Point point);
    }
}
