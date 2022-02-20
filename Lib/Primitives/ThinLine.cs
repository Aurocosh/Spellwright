using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Spellwright.Lib.Primitives
{
    public class ThinLine : IEnumerable<Point>
    {
        public Point Start { get; }
        public Point Stop { get; }

        public ThinLine(Point start, Point stop)
        {
            Start = start;
            Stop = stop;
        }

        public ThinLine(int startX, int startY, int stopX, int stopY) : this(new Point(startX, startY), new Point(stopX, stopY))
        {
        }

        public IEnumerable<Point> Iterator => Iterate(Start, Stop);

        private static IEnumerable<Point> Iterate(Point from, Point to)
        {
            int x = from.X;
            int y = from.Y;

            int dx = to.X - from.X;
            int dy = to.Y - from.Y;

            var inverted = false;
            int step = Math.Sign(dx);
            int gradientStep = Math.Sign(dy);

            int longest = Math.Abs(dx);
            int shortest = Math.Abs(dy);

            if (longest < shortest)
            {
                inverted = true;
                longest = Math.Abs(dy);
                shortest = Math.Abs(dx);

                step = Math.Sign(dy);
                gradientStep = Math.Sign(dx);
            }

            int gradientAccumulation = longest / 2;
            for (var i = 0; i < longest; i++)
            {
                yield return new Point(x, y);

                if (inverted)
                    y += step;
                else
                    x += step;

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                        x += gradientStep;
                    else
                        y += gradientStep;
                    gradientAccumulation -= longest;
                }
            }
        }

        public IEnumerator<Point> GetEnumerator() => Iterator.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
