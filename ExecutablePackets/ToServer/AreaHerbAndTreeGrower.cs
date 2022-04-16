using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Network.Base;
using Spellwright.Util;
using Terraria;

namespace Spellwright.Manipulators
{
    internal class AreaHerbAndTreeGrower : IExecutablePacket
    {
        public Point CenterPoint { get; set; }
        public int Radius { get; set; }

        public AreaHerbAndTreeGrower()
        {
        }

        public AreaHerbAndTreeGrower(Point centerPoint, int radius)
        {
            CenterPoint = centerPoint;
            Radius = radius;
        }

        public void Execute()
        {
            int radiusSq = Radius * Radius;
            bool IsValid(Point point)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;

                int distanceToCenterSq = (point - CenterPoint).DistanceSq();
                if (distanceToCenterSq > radiusSq)
                    return false;

                return true;
            }

            var circlePoints = UtilCoordinates.FloodFill(new[] { CenterPoint }, PointConstants.DirectNeighbours, IsValid, 10000);
            foreach (var point in circlePoints)
                UtilTiles.GrowHerbsAndTrees(point.X, point.Y);
        }
    }
}
