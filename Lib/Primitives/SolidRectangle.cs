using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Spellwright.Lib.Primitives
{
    public class SolidRectangle : ICoordinateShape
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public int LimitX => X + Width;
        public int LimitY => Y + Height;

        public Rectangle Rectangle => new(X, Y, Width, Height);

        public SolidRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public SolidRectangle(Point origin, int width, int height) : this(origin.X, origin.Y, width, height)
        {
        }

        public SolidRectangle(Rectangle rectangle) : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
        {
        }

        public SolidRectangle(int x, int y, int radius)
        {
            X = x - radius;
            Y = y - radius;
            Width = radius * 2 + 1;
            Height = radius * 2 + 1;
        }

        public SolidRectangle(Point origin, int radius) : this(origin.X, origin.Y, radius)
        {
        }

        public IEnumerable<Point> Iterator => IterateRows();

        public IEnumerable<Point> IterateColumns()
        {
            var maxX = LimitX;
            var maxY = LimitY;

            for (int x = X; x < maxX; x++)
                for (int y = Y; y < maxY; y++)
                    yield return new Point(x, y);
        }

        public IEnumerable<Point> IterateRows()
        {
            var maxX = LimitX;
            var maxY = LimitY;

            for (int y = Y; y < maxY; y++)
                for (int x = X; x < maxX; x++)
                    yield return new Point(x, y);
        }

        public IEnumerator<Point> GetEnumerator() => Iterator.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsInBounds(int x, int y)
        {
            return x >= X && y >= Y && x < LimitX && y < LimitY;
        }

        public bool IsInBounds(Point point)
        {
            return point.X >= X && point.Y >= Y && point.X < LimitX && point.Y < LimitY;
        }
    }
}
