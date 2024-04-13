using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.TileSpawn
{
    internal class WillOfTorchGodSpell : ModSpell
    {
        private static readonly int radius = 14;
        private static readonly int radiusSq = radius * radius;
        private static Point lastPlayerPoint = Point.Zero;
        private static HashSet<int> lightTilesTypes = new();

        public override void SetStaticDefaults()
        {
            SpellLevel = 1;

            RemoveApplicableModifier(SpellModifier.Area);

            UnlockCost = new TorchGodSpellCost();

            lightTilesTypes.Clear();
            lightTilesTypes.Add(TileID.Torches);
            lightTilesTypes.Add(TileID.Fireplace);
            lightTilesTypes.Add(TileID.Lampposts);
            lightTilesTypes.Add(TileID.Lamps);
            lightTilesTypes.Add(TileID.Candles);
            lightTilesTypes.Add(TileID.Chandeliers);
            lightTilesTypes.Add(TileID.Candelabras);
            lightTilesTypes.Add(TileID.ChineseLanterns);
            lightTilesTypes.Add(TileID.HangingLanterns);
            lightTilesTypes.Add(TileID.SkullLanterns);
            lightTilesTypes.Add(TileID.FireflyinaBottle);
            lightTilesTypes.Add(TileID.LightningBuginaBottle);
            lightTilesTypes.Add(TileID.LavaflyinaBottle);
            lightTilesTypes.Add(TileID.SoulBottles);
            lightTilesTypes.Add(TileID.Jackolanterns);
            lightTilesTypes.Add(TileID.DiscoBall);
            lightTilesTypes.Add(TileID.LavaLamp);
            lightTilesTypes.Add(TileID.HolidayLights);
            lightTilesTypes.Add(TileID.WireBulb);
            lightTilesTypes.Add(TileID.Crystals);
            lightTilesTypes.Add(TileID.Campfire);
            lightTilesTypes.Add(TileID.Furnaces);
            lightTilesTypes.Add(TileID.LihzahrdFurnace);
            lightTilesTypes.Add(TileID.GlassKiln);
            lightTilesTypes.Add(TileID.LihzahrdAltar);
            lightTilesTypes.Add(TileID.MushroomGrass);
            lightTilesTypes.Add(TileID.MushroomPlants);
            lightTilesTypes.Add(TileID.MushroomTrees);
            lightTilesTypes.Add(TileID.MushroomVines);
            lightTilesTypes.Add(TileID.LavaMoss);
            lightTilesTypes.Add(TileID.KryptonMoss);
            lightTilesTypes.Add(TileID.XenonMoss);
            lightTilesTypes.Add(TileID.ArgonMoss);
            lightTilesTypes.Add(TileID.Teleporter);
            lightTilesTypes.Add(TileID.PlanteraBulb);
            lightTilesTypes.Add(TileID.DemonAltar);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            if (statPlayer.AutoTorches)
            {
                statPlayer.AutoTorches = false;
                var message = GetTranslation("AutoTorchDisabled").Value;
                Main.NewText(message);
            }
            else
            {
                statPlayer.AutoTorches = true;
                var message = GetTranslation("AutoTorchEnabled").Value;
                Main.NewText(message);
            }

            return true;
        }

        private static bool IsTorchItemValid(Item item) => item.createTile == TileID.Torches;

        public static void AutoPlaceTorches(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            var centerPoint = player.Center.ToGridPoint();
            bool moved = centerPoint != lastPlayerPoint;
            if (!moved)
                return;

            var allItems = player.GetInventoryItems().Concat(player.IterateAllVacuumBagItems());
            bool hasTorch = UtilInventory.HasItems(allItems, IsTorchItemValid, 1);
            if (!hasTorch)
                return;

            var feetPoint = new Point(centerPoint.X, centerPoint.Y + 1);
            TryPlaceTorch(player, allItems, feetPoint);
            PlaceTorch(player, allItems, centerPoint, lastPlayerPoint);

            lastPlayerPoint = centerPoint;
        }

        private static void PlaceTorch(Player player, IEnumerable<Item> relevantItems, Point centerPoint, Point oldCentralPoint)
        {
            bool IsValid(Point point)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;
                if (point.DistanceToSq(centerPoint) > radiusSq)
                    return false;

                Tile tile = Main.tile[point.X, point.Y];
                if (WorldGen.SolidTile(tile))
                    return false;

                return true;
            }

            var circlePoints = UtilCoordinates.FloodFill(new[] { centerPoint }, PointConstants.DirectNeighbours, IsValid, 400);
            foreach (var point in circlePoints)
                if (point.DistanceToSq(oldCentralPoint) >= radiusSq)
                    TryPlaceTorch(player, relevantItems, point);
        }

        private static void TryPlaceTorch(Player player, IEnumerable<Item> relevantItems, Point point)
        {
            if (!WorldGen.InWorld(point.X, point.Y))
                return;

            Tile tile = Main.tile[point.X, point.Y];
            if (lightTilesTypes.Contains(tile.TileType) || tile.LiquidAmount > 0)
                return;

            if (tile.HasTile && !TileID.Sets.BreakableWhenPlacing[tile.TileType] && (!Main.tileCut[tile.TileType] || tile.TileType == TileID.ImmatureHerbs || tile.TileType == TileID.MatureHerbs))
                return;

            var nearbyPoints = new SolidRectangle(point, 8);
            foreach (var nearbyPoint in nearbyPoints)
            {
                Tile nearbyTile = Main.tile[nearbyPoint.X, nearbyPoint.Y];
                if (nearbyTile != null && lightTilesTypes.Contains(nearbyTile.TileType))
                    return;
            }

            if (tile.WallType > 0 || ValidSideTile(point.X - 1, point.Y, 1) || ValidSideTile(point.X + 1, point.Y, 0) || ValidBottomTile(point.X, point.Y + 1))
            {
                bool consumedTorch = false;
                Item torchItem = UtilInventory.InventoryFindItem(relevantItems, IsTorchItemValid);
                if (torchItem != null)
                {
                    if (torchItem.ModItem != null && torchItem.ModItem.Mod.Name == "miningcracks_take_on_luiafk") // Do not consume endless versions of torches
                    {
                        consumedTorch = true;
                    }
                    else
                    {
                        torchItem.Consume(1);
                        consumedTorch = true;
                    }
                }

                if (consumedTorch)
                {
                    int torchStyle = BiomeTorchPlaceStyle(player);
                    if (WorldGen.PlaceTile(point.X, point.Y, TileID.Torches, mute: false, false, player.whoAmI, torchStyle) && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, point.X, point.Y, TileID.Torches, torchStyle, 0, 0);
                }
            }
        }

        private static bool ValidSideTile(int x, int y, int slopeNum) // slopeNum: Left = 1, Right = 0
        {
            Tile tile2 = Main.tile[x, y];
            if (!tile2.HasTile)
                return false;
            if (tile2.Slope != 0 && (int)tile2.Slope % 2 == slopeNum)
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

            return !Main.tileSolidTop[tileType] || TileID.Sets.Platforms[tileType] && tile.Slope == 0;
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
            else if (player.ZoneDesert && player.position.Y < Main.worldSurface * 16.0 || player.ZoneUndergroundDesert)
                return 16;
            return 0;
        }
    }
}
