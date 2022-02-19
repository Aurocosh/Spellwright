using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Spellwright.Other
{
    internal class PointConstants
    {
        public static readonly Point Up = new(0, -1);
        public static readonly Point Down = new(0, 1);
        public static readonly Point Left = new(-1, 0);
        public static readonly Point Right = new(1, 0);

        public static readonly IReadOnlyList<Point> DirectNeighbours = new List<Point>()
        {
            Up,
            Right,
            Down,
            Left,
        };

        public static readonly IReadOnlyList<Point> AllNeighbours = new List<Point>()
        {
            Up,
            Up + Right,
            Right,
            Right + Down,
            Down,
            Down + Left,
            Left,
            Left + Up
        };
    }
}
