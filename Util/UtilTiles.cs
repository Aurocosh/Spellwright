using Spellwright.Other;
using Terraria;
using Terraria.ID;

namespace Spellwright.Util
{
    internal static class UtilTiles
    {
        public static TilePlaceAction CanReplaceTile(Tile tile)
        {
            if (tile == null)
                return TilePlaceAction.CanPlace;
            if (!tile.HasTile)
                return TilePlaceAction.CanPlace;

            //if (tile.TileType == TileID.Plants)
            //{
            //    int a = 2;
            //}

            if (Main.tileCut[tile.TileType] || TileID.Sets.BreakableWhenPlacing[tile.TileType])
                return TilePlaceAction.CanReplace;
            return TilePlaceAction.CannotPlace;
        }

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
