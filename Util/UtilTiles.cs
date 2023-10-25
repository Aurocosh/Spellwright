using Spellwright.Lib.Constants;
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

        public static void GrowHerbsAndTrees(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            ushort tileType = tile.TileType;
            if (Main.tileAlch[tileType])
            {
                bool forceBloom = Main.rand.Next(100) < 65;
                GrowAlch(x, y, forceBloom);
            }
            else if (tileType == TileID.GemSaplings)
            {
                int param = tile.TileFrameX / 54;
                int treeTileType = TileID.TreeRuby;
                switch (param)
                {
                    case 0:
                        treeTileType = TileID.TreeTopaz;
                        break;
                    case 1:
                        treeTileType = TileID.TreeAmethyst;
                        break;
                    case 2:
                        treeTileType = TileID.TreeSapphire;
                        break;
                    case 3:
                        treeTileType = TileID.TreeEmerald;
                        break;
                    case 4:
                        treeTileType = TileID.TreeRuby;
                        break;
                    case 5:
                        treeTileType = TileID.TreeDiamond;
                        break;
                    case 6:
                        treeTileType = TileID.TreeAmber;
                        break;
                }

                if (WorldGen.TryGrowingTreeByType(treeTileType, x, y) && WorldGen.PlayerLOS(x, y))
                    WorldGen.TreeGrowFXCheck(x, y);
            }
            else if (tileType == TileID.Saplings)
            {
                bool flag2 = WorldGen.PlayerLOS(x, y);
                bool flag3 = (tile.TileFrameX < 324 || tile.TileFrameX >= 540) ? WorldGen.GrowTree(x, y) : WorldGen.GrowPalmTree(x, y);
                if (flag3 && flag2)
                    WorldGen.TreeGrowFXCheck(x, y);
            }
            else if (tileType == TileID.VanityTreeSakuraSaplings)
            {
                int treeTileType = TileID.VanityTreeSakura;
                if (WorldGen.TryGrowingTreeByType(treeTileType, x, y) && WorldGen.PlayerLOS(x, y))
                    WorldGen.TreeGrowFXCheck(x, y);
            }
            else if (tileType == TileID.VanityTreeWillowSaplings)
            {
                int treeTileType = TileID.VanityTreeYellowWillow;
                if (WorldGen.TryGrowingTreeByType(treeTileType, x, y) && WorldGen.PlayerLOS(x, y))
                    WorldGen.TreeGrowFXCheck(x, y);
            }
            else if (tileType == TileID.Pumpkins)
            {
                WorldGen.GrowPumpkin(x, y, TileID.Pumpkins);
            }
        }

        public static void GrowAlch(int x, int y, bool forceBloom)
        {
            Tile tile = Main.tile[x, y];
            if (!tile.HasTile)
                return;

            bool isImmatureHerb = tile.TileType == TileID.ImmatureHerbs;
            bool isMatureHerb = tile.TileType == TileID.MatureHerbs;
            if (!isImmatureHerb && !isMatureHerb)
                return;

            ushort newState = TileID.MatureHerbs;
            if (isMatureHerb || forceBloom)
                newState = TileID.BloomingHerbs;

            tile.TileType = newState;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, x, y, TileChangeType.None);
            WorldGen.SquareTileFrame(x, y, true);
        }
    }
}
