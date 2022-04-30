using Microsoft.Xna.Framework;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Network.Base.Executable;
using Spellwright.Util;
using Terraria;

namespace Spellwright.ExecutablePackets.ToServer
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
            var area = new SolidCircle(CenterPoint, Radius);
            bool IsValid(Point point)
            {
                if (!area.IsInBounds(point))
                    return false;
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;
                return true;
            }

            var circlePoints = UtilCoordinates.FloodFill(new[] { CenterPoint }, PointConstants.DirectNeighbours, IsValid, 10000);
            foreach (var point in circlePoints)
                UtilTiles.GrowHerbsAndTrees(point.X, point.Y);
        }
    }
}
