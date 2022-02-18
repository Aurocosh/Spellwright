using Terraria.ID;

namespace Spellwright.Util
{
    internal static class UtilTiles
    {
        public static bool IsTileGrass(int tileType)
        {
            if (tileType == TileID.Grass)
                return true;
            if (tileType == TileID.GolfGrass)
                return true;
            if (tileType == TileID.HallowedGrass)
                return true;
            if (tileType == TileID.GolfGrassHallowed)
                return true;
            return false;
        }
    }
}
