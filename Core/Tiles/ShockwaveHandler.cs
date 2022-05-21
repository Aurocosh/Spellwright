using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Core.Tiles
{
    internal class ShockwaveHandler
    {
        private static readonly HashSet<int> breakableTileTypes = new();

        static ShockwaveHandler()
        {
            breakableTileTypes.Add(TileID.BreakableIce);
            breakableTileTypes.Add(TileID.Plants);
            breakableTileTypes.Add(TileID.Plants2);
            breakableTileTypes.Add(TileID.CorruptPlants);
            breakableTileTypes.Add(TileID.CrimsonPlants);
            breakableTileTypes.Add(TileID.HallowedPlants);
            breakableTileTypes.Add(TileID.HallowedPlants2);
            breakableTileTypes.Add(TileID.JunglePlants);
            breakableTileTypes.Add(TileID.JunglePlants2);
            breakableTileTypes.Add(TileID.MushroomPlants);
            breakableTileTypes.Add(TileID.OasisPlants);
            breakableTileTypes.Add(TileID.Vines);
            breakableTileTypes.Add(TileID.CrimsonVines);
            breakableTileTypes.Add(TileID.HallowedVines);
            breakableTileTypes.Add(TileID.Crystals);
            breakableTileTypes.Add(TileID.JungleVines);
            breakableTileTypes.Add(TileID.MushroomVines);
            breakableTileTypes.Add(TileID.JungleThorns);
            breakableTileTypes.Add(TileID.CorruptThorns);
            breakableTileTypes.Add(TileID.CrimsonThorns);
            breakableTileTypes.Add(TileID.Cobweb);
            breakableTileTypes.Add(TileID.Coral);
            breakableTileTypes.Add(TileID.BeachPiles);
            breakableTileTypes.Add(TileID.Pots);
        }

        public bool DoShockwave(Vector2 centerPosition, int radius)
        {
            bool brokeAtLeastOne = false;
            var tilePositions = GetTilePositions(centerPosition, radius);
            foreach (var point in tilePositions)
            {
                if (Main.rand.NextFloat() < .15f)
                {
                    Vector2 pointPosition = point.ToWorldCoordinates();
                    var direction = (pointPosition.DirectionFrom(centerPosition)) * 10;
                    var dust = Dust.NewDustDirect(pointPosition, 16, 16, DustID.CorruptPlants, direction.X, direction.Y, 50, Color.White, 1.2f);
                    dust.noGravity = true;
                }

                if (!WorldGen.InWorld(point.X, point.Y))
                    continue;
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (tile == null || !breakableTileTypes.Contains(tile.TileType))
                    continue;

                WorldGen.KillTile(point.X, point.Y, false, false, false);
                var tileState = Framing.GetTileSafely(point.X, point.Y);
                if (!tileState.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    brokeAtLeastOne = true;
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, point.X, point.Y);
                }
            }

            return brokeAtLeastOne;
        }

        private IEnumerable<Point> GetTilePositions(Vector2 centerPosition, int radius)
        {
            var center = centerPosition.ToGridPoint();
            var circle = new SolidCircle(center, radius);

            bool IsValid(Point point)
            {
                if (!circle.IsInBounds(point) || !WorldGen.InWorld(point.X, point.Y))
                    return false;
                Tile tile = Main.tile[point.X, point.Y];
                if (WorldGen.SolidTile(tile))
                    return false;
                return true;
            }

            return UtilCoordinates.FloodFill(new[] { center }, PointConstants.DirectNeighbours, IsValid, 3000);
        }
    }
}
