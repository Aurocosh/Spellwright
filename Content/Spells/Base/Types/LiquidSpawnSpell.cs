using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.PointShapes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class LiquidSpawnSpell : ModSpell
    {
        protected int radius = 15;
        protected int liquidType;

        protected virtual int GetLiquidType(int playerLevel) => liquidType;
        protected virtual bool CanReplaceTile(Tile tile, int x, int y, int playerLevel) => !tile.HasTile;

        protected virtual IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            yield return center;
        }

        public LiquidSpawnSpell()
        {
            UseType = SpellType.Echo;
            liquidType = LiquidID.Water;
            canAutoReuse = true;
            useTimeMultiplier = 9f;
            useSound = SoundID.Item19;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var center = player.Center.ToGridPoint();
            return SpawnLiquidTiles(center, player, playerLevel, spellData);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            var center = Main.MouseWorld.ToGridPoint();
            return SpawnLiquidTiles(center, player, playerLevel, spellData);
        }

        private bool SpawnLiquidTiles(Point center, Player player, int playerLevel, SpellData spellData)
        {
            var positioins = GetTilePositions(center, player, playerLevel, spellData);
            var validArea = new SolidCircle(player.Center.ToGridPoint(), radius);
            bool spawnedAtLeastOne = false;
            foreach (var point in positioins)
            {
                if (validArea.IsInBounds(point) && SpawnLiquid(point, player, playerLevel))
                    spawnedAtLeastOne = true;
            }

            return spawnedAtLeastOne;
        }

        private bool SpawnLiquid(Point center, Player player, int playerLevel)
        {
            if (!WorldGen.InWorld(center.X, center.Y, 0))
                return false;

            int currentLiquidType = GetLiquidType(playerLevel);
            Tile tile = Framing.GetTileSafely(center.X, center.Y);

            bool canPlaceTile = CanReplaceTile(tile, center.X, center.Y, playerLevel);
            if (!canPlaceTile)
                return false;

            int tileLiquidType = tile.LiquidType;
            if (tile.LiquidAmount == 0 || tileLiquidType == currentLiquidType)
            {
                tile.LiquidType = currentLiquidType;
                tile.LiquidAmount = byte.MaxValue;
                WorldGen.SquareTileFrame(center.X, center.Y, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.sendWater(center.X, center.Y);
            }
            else
            {
                if (tile.LiquidAmount <= 32)
                    return false;

                tile.LiquidAmount = 0;
                WorldGen.SquareTileFrame(center.X, center.Y, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.sendWater(center.X, center.Y);
                else
                    Liquid.AddWater(center.X, center.Y);

                int firstLiquidType = Math.Min(tileLiquidType, currentLiquidType);
                int secondLiquidType = Math.Max(tileLiquidType, currentLiquidType);
                if (firstLiquidType == LiquidID.Water && secondLiquidType == LiquidID.Lava)
                    if (WorldGen.PlaceTile(center.X, center.Y, 56, false, false, player.whoAmI, 0) && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, center.X, center.Y, TileID.Stone, 0, 0, 0);
                    else if (firstLiquidType == LiquidID.Water && secondLiquidType == LiquidID.Honey)
                        if (WorldGen.PlaceTile(center.X, center.Y, TileID.HoneyBlock, false, false, player.whoAmI, 0) && Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, center.X, center.Y, TileID.HoneyBlock, 0, 0, 0);
                        else if (firstLiquidType == LiquidID.Lava && secondLiquidType == LiquidID.Honey)
                            if (WorldGen.PlaceTile(center.X, center.Y, TileID.CrispyHoneyBlock, false, false, player.whoAmI, 0) && Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, center.X, center.Y, TileID.CrispyHoneyBlock, 0, 0, 0);
            }

            return true;
        }
    }
}
