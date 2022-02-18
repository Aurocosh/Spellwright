using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Spellwright.Util
{
    internal static class UtilRandom
    {
        //private static readonly Random Random = new Random();
        private static readonly UnifiedRandom Random = new UnifiedRandom();

        public static int NextInt(int min, int max)
        {
            return Random.Next(max - min + 1) + min;
        }

        public static double NextDouble(double min, double max)
        {
            return min + (max - min) * Random.NextDouble();
        }

        public static float NextFloat(float min, float max)
        {
            return (float)(min + (max - min) * Random.NextDouble());
        }

        public static int NextSign()
        {
            return Random.Next(2) - 1;
        }

        public static int NextIntExclusive(int min, int max)
        {
            return Random.Next(max - min) + min;
        }

        public static Vector2 NextVector2()
        {
            var x = NextFloat(-1f, 1f);
            var y = NextFloat(-1f, 1f);
            var direction = new Vector2(x, y);
            direction.Normalize();
            return direction;
        }

    }
}
