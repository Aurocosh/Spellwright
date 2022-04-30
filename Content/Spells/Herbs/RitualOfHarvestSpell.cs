using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Network;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Herbs
{
    internal class RitualOfHarvestSpell : ModSpell
    {
        private static readonly Dictionary<int, Item> seedItems = new();
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.DirtRod, 1)
                .WithCost(ItemID.WaterCandle, 2);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 2);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();

            int radius = 3 * spellPlayer.PlayerLevel;
            var centerPoint = player.Center.ToGridPoint();
            var area = new SolidCircle(centerPoint, radius);
            bool IsValid(Point point)
            {
                if (!area.IsInBounds(point))
                    return false;
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;
                return true;
            }

            var circlePoints = UtilCoordinates.FloodFill(new[] { centerPoint }, PointConstants.DirectNeighbours, IsValid, 10000);
            foreach (var point in circlePoints)
            {
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (!IsHarvestablePlant(tile))
                    continue;

                var location = point.ToWorldVector2();

                WorldGen.KillTile_GetItemDrops(point.X, point.Y, tile, out int firstItemType, out int firstItemCount, out int secondItemType, out int secondItemCount, false);
                bool canReplant = secondItemCount > 0;
                secondItemCount--;
                if (firstItemType > 0 && firstItemCount > 0)
                {
                    var source = new EntitySource_Parent(player);
                    int itemIndex = Item.NewItem(source, player.Center, 16, 16, firstItemType, firstItemCount, false, -1, false, false);
                    Main.item[itemIndex].TryCombiningIntoNearbyItems(itemIndex);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1);
                }
                if (secondItemType > 0 && secondItemCount > 0)
                {
                    var source = new EntitySource_Parent(player);
                    int itemIndex = Item.NewItem(source, player.Center, 16, 16, secondItemType, secondItemCount, false, -1, false, false);
                    Main.item[itemIndex].TryCombiningIntoNearbyItems(itemIndex);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1);
                }

                WorldGen.KillTile(point.X, point.Y);
                if (!Main.tile[point.X, point.Y].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, point.X, point.Y);

                if (canReplant)
                {
                    var item = GetSeedItem(secondItemType);
                    var createdTile = item.createTile;
                    var tileStyle = item.placeStyle;

                    if (createdTile == TileID.ImmatureHerbs)
                        if (WorldGen.PlaceTile(point.X, point.Y, createdTile, mute: false, false, player.whoAmI, tileStyle) && Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, point.X, point.Y, createdTile, tileStyle, 0, 0);
                }
            }

            var spawner = new HerbAoeDustSpawner(player, radius);
            spawner.Execute();
            if (Main.netMode == NetmodeID.Server)
                ModNetHandler.HerbAoeDustHandler.Send(spawner);

            return true;
        }

        private static bool IsHarvestablePlant(Tile tile)
        {
            int tileType = tile.TileType;
            if (tileType == TileID.Plants || tileType == TileID.Plants2 || tileType == TileID.BloomingHerbs)
                return true;
            if (tileType == TileID.MatureHerbs)
            {
                int frameX = tile.TileFrameX / 18;
                if (frameX == 0 && Main.dayTime)
                    return true;
                if (frameX == 1 && !Main.dayTime)
                    return true;
                if (frameX == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
                    return true;
                if (frameX == 4 && (Main.raining || Main.cloudAlpha > 0f))
                    return true;
                if (frameX == 5 && !Main.raining && Main.dayTime && Main.time > 40500.0)
                    return true;
            }

            return false;
        }

        private static Item GetSeedItem(int type)
        {
            if (seedItems.TryGetValue(type, out var seedItem))
                return seedItem;

            seedItem = new Item();
            seedItem.SetDefaults(type);
            seedItems.Add(type, seedItem);
            return seedItem;
        }
    }
}