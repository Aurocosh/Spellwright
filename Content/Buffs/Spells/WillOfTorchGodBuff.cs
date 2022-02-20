using Microsoft.Xna.Framework;
using Spellwright.Constants;
using Spellwright.Extensions;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class WillOfTorchGodBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will of Torch god");
            Description.SetDefault("There shall be light and biome appropriate torches!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }

        private static bool IsTorchItemValid(Item item) => item.createTile == TileID.Torches;

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            if (Main.netMode == NetmodeID.Server)
                return;

            WillOfTorchGodPlayer willPlayer = player.GetModPlayer<WillOfTorchGodPlayer>();

            if (willPlayer.SkipValue > 0)
            {
                willPlayer.SkipValue--;
                return;
            }
            willPlayer.SkipValue = 10;

            var centerPoint = player.Center.ToGridPoint();
            if (centerPoint == willPlayer.LastPlayerPoint)
                return;

            bool hasTorch = player.HasItems(IsTorchItemValid, 1);
            if (!hasTorch)
                return;

            int radius = 14;
            PlaceTorch(player, centerPoint, willPlayer.LastPlayerPoint, radius);

            willPlayer.LastPlayerPoint = centerPoint;
        }

        private void PlaceTorch(Player player, Point centerPoint, Point oldCentralPoint, int radius)
        {
            int radiusSq = radius * radius;
            bool IsValid(Point point)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;

                int distanceToCenterSq = (point - centerPoint).DistanceSq();
                if (distanceToCenterSq > radiusSq)
                    return false;

                Tile tile = Main.tile[point.X, point.Y];
                if (WorldGen.SolidTile(tile))
                    return false;

                return true;
            }

            var circlePoints = UtilCoordinates.FloodFill(centerPoint, PointConstants.DirectNeighbours, IsValid, 400);
            foreach (var point in circlePoints)
            {
                int distanceToOldCenterSq = (point - oldCentralPoint).DistanceSq();
                if (distanceToOldCenterSq < radiusSq)
                    continue;

                if (!WorldGen.InWorld(point.X, point.Y))
                    continue;

                Tile tile = Main.tile[point.X, point.Y];

                if (tile.TileType == TileID.Torches)
                    continue;

                if (tile.LiquidAmount > 0)
                    continue;

                if (tile.HasTile && !TileID.Sets.BreakableWhenPlacing[tile.TileType] && (!Main.tileCut[tile.TileType] || tile.TileType == TileID.ImmatureHerbs || tile.TileType == TileID.MatureHerbs))
                    continue;

                bool hasTorchesNearby = false;
                var nearbyPoints = UtilCoordinates.GetPointsInSqRadius(point, 8, 8);
                foreach (var nearbyPoint in nearbyPoints)
                {
                    Tile nearbyTile = Main.tile[nearbyPoint.X, nearbyPoint.Y];
                    if (nearbyTile != null && nearbyTile.TileType == TileID.Torches)
                    {
                        hasTorchesNearby = true;
                        break;
                    }
                }

                if (hasTorchesNearby)
                    continue;

                if (tile.WallType > 0 || ValidSideTile(point.X - 1, point.Y, 1) || ValidSideTile(point.X + 1, point.Y, 0) || ValidBottomTile(point.X, point.Y + 1))
                {
                    bool consumedTorch = player.ConsumeItems(IsTorchItemValid, 1);
                    if (consumedTorch)
                    {
                        int torchStyle = BiomeTorchPlaceStyle(player);
                        if (WorldGen.PlaceTile(point.X, point.Y, TileID.Torches, mute: false, false, player.whoAmI, torchStyle) && Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, point.X, point.Y, TileID.Torches, torchStyle, 0, 0);
                    }
                }
            }
        }

        private static bool ValidSideTile(int x, int y, int slopeNum) // slopeNum: Left = 1, Right = 0
        {
            Tile tile2 = Main.tile[x, y];
            if (!tile2.HasTile)
                return false;
            if ((tile2.Slope != 0 && (int)tile2.Slope % 2 == slopeNum))
                return false;

            if (Main.tileSolid[tile2.TileType] && !Main.tileNoAttach[tile2.TileType] && !Main.tileSolidTop[tile2.TileType] && !TileID.Sets.NotReallySolid[tile2.TileType])
                return true;
            if (TileID.Sets.IsBeam[tile2.TileType])
                return true;

            if (WorldGen.IsTreeType(tile2.TileType))
            {
                Tile tileAbove = Main.tile[x, y - 1];
                Tile tileBelow = Main.tile[x, y + 1];
                if (WorldGen.IsTreeType(tileAbove.TileType) && WorldGen.IsTreeType(tileBelow.TileType))
                    return true;
            }
            return false;
        }
        private static bool ValidBottomTile(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (!tile.HasTile)
                return false;

            ushort tileType = tile.TileType;
            if (!Main.tileSolid[tileType])
                return false;
            if (Main.tileNoAttach[tileType])
                return false;
            if (TileID.Sets.NotReallySolid[tileType])
                return false;
            if (tile.IsHalfBlock)
                return false;
            if (tile.Slope != 0)
                return false;

            return !Main.tileSolidTop[tileType] || (TileID.Sets.Platforms[tileType] && tile.Slope == 0);
        }
        private static int BiomeTorchPlaceStyle(Player player)
        {
            if (player.ZoneDungeon)
                return 13;
            else if (player.position.Y > Main.UnderworldLayer * 16)
                return 7;
            else if (player.ZoneHallow)
                return 20;
            else if (player.ZoneCorrupt)
                return 18;
            else if (player.ZoneCrimson)
                return 19;
            else if (player.ZoneSnow)
                return 9;
            else if (player.ZoneJungle)
                return 21;
            else if ((player.ZoneDesert && player.position.Y < Main.worldSurface * 16.0) || player.ZoneUndergroundDesert)
                return 16;
            return 0;
        }

        public class WillOfTorchGodPlayer : ModPlayer
        {
            public int SkipValue { get; set; } = 0;
            public Point LastPlayerPoint { get; set; } = Point.Zero;
        }
    }
}
