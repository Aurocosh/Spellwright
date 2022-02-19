using Microsoft.Xna.Framework;
using Spellwright.Spells.SpellExtraData;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Spells.Base
{
    internal abstract class LiquidSpawnSpell : Spell
    {
        protected int liquidType;

        protected virtual int GetLiquidType(int playerLevel) => liquidType;
        protected virtual bool CanReplaceTile(Tile tile, int x, int y, int playerLevel) => !tile.HasTile;

        public LiquidSpawnSpell(string name, string incantation, SpellType spellType = SpellType.Spell) : base(name, incantation, spellType)
        {
            liquidType = LiquidID.Water;
            canAutoReuse = true;
            useTimeMultiplier = 7f;
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            Vector2 mousePosition = Main.MouseWorld;

            int xPos = (int)(mousePosition.X / 16.0f);
            int yPos = (int)(mousePosition.Y / 16.0f);

            if (!WorldGen.InWorld(xPos, yPos, 0))
                return false;

            int currentLiquidType = GetLiquidType(playerLevel);
            Tile tile = Framing.GetTileSafely(xPos, yPos);

            bool canPlaceTile = CanReplaceTile(tile, xPos, yPos, playerLevel);
            if (!canPlaceTile)
                return false;

            int tileLiquidType = tile.LiquidType;
            if (tile.LiquidAmount == 0 || tileLiquidType == currentLiquidType)
            {
                tile.LiquidType = currentLiquidType;
                tile.LiquidAmount = byte.MaxValue;
                WorldGen.SquareTileFrame(xPos, yPos, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.sendWater(xPos, yPos);
            }
            else
            {
                if (tile.LiquidAmount <= 32)
                    return false;

                tile.LiquidAmount = 0;
                WorldGen.SquareTileFrame(xPos, yPos, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.sendWater(xPos, yPos);
                else
                    Liquid.AddWater(xPos, yPos);

                int firstLiquidType = Math.Min(tileLiquidType, currentLiquidType);
                int secondLiquidType = Math.Max(tileLiquidType, currentLiquidType);
                if (firstLiquidType == LiquidID.Water && secondLiquidType == LiquidID.Lava)
                {
                    if (WorldGen.PlaceTile(xPos, yPos, 56, false, false, player.whoAmI, 0) && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, TileID.Stone, 0, 0, 0);
                }
                else if (firstLiquidType == LiquidID.Water && secondLiquidType == LiquidID.Honey)
                {
                    if (WorldGen.PlaceTile(xPos, yPos, TileID.HoneyBlock, false, false, player.whoAmI, 0) && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, TileID.HoneyBlock, 0, 0, 0);
                }
                else if (firstLiquidType == LiquidID.Lava && secondLiquidType == LiquidID.Honey)
                {
                    if (WorldGen.PlaceTile(xPos, yPos, TileID.CrispyHoneyBlock, false, false, player.whoAmI, 0) && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, TileID.CrispyHoneyBlock, 0, 0, 0);
                }
            }

            return true;
        }
    }
}
