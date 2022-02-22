using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Spellwright.Content.Spells
{
    internal class MaterialShellSpell : ModSpell
    {
        public bool FillBombFalling = false;

        private int _radius;
        private ushort _tileType;
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;

            _radius = 3;
            _tileType = TileID.BorealWood;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var position = player.position;
            position.Y -= 300;

            DoBlockBomb(position, _tileType, _radius, DustID.Smoke, 0.5f, 2000);

            return true;
        }

        public int DoBlockBomb(Vector2 position, ushort tileType, int radius, int dustType, float dustFactor, int tileCount = -1)
        {
            SoundEngine.PlaySound(SoundID.Item14, position);

            for (int i = 0; i < 20 * dustFactor; i++)
            {
                int dustIndex = Dust.NewDust(position, 22, 22, DustID.Smoke, 0f, 0f, 100, new Color(), 1.5f);
                Main.dust[dustIndex].velocity *= 1.5f;
            }
            for (int i = 0; i < 10 * dustFactor; i++)
            {
                int dustIndex = Dust.NewDust(position, 22, 22, dustType, 0f, 0f, 100, new Color(), 2.5f);
                Main.dust[dustIndex].velocity.X = (1.0f - Main.rand.NextFloat() * 2.0f) * 5.0f;
                Main.dust[dustIndex].velocity.Y = (1.0f - Main.rand.NextFloat() * 2.0f) * 5.0f;

                dustIndex = Dust.NewDust(position, 22, 22, dustType, 0f, 0f, 100, new Color(), 1.5f);
                Main.dust[dustIndex].velocity.X = (1.0f - Main.rand.NextFloat() * 2.0f) * 3.0f;
                Main.dust[dustIndex].velocity.Y = (1.0f - Main.rand.NextFloat() * 2.0f) * 3.0f;
            }

            //start iterating from bottom left to top right
            //so the find bottom algorithm works properly

            int tileCounter = 0;

            for (int x = -radius; x <= radius; x++)
                for (int y = radius; y >= -radius; y--)
                {
                    int xPos = (int)(x + position.X / 16.0f);
                    int yPos = (int)(y + position.Y / 16.0f);

                    if (!WorldGen.InWorld(xPos, yPos))
                        continue;

                    if (x * x + y * y <= (radius + 0.5) * (radius + 0.5))
                    {
                        Tile tile = Framing.GetTileSafely(xPos, yPos);

                        //change yPos to account for falling bombs
                        //################################################

                        int index = x + radius;

                        //Y at which it shouldn't search for lowest tile
                        int originalY = (int)(y + position.Y / 16.0f);

                        if (FillBombFalling)
                            //if (!tile.HasTile || tile.TileType != 0 && PlantTypes.Contains(tile.TileType)) //if current tile already filled, skip the search
                            if (!tile.HasTile || tile.TileType != 0 && false) //if current tile already filled, skip the search
                            {
                                bool atleastOnce = false;
                                Tile tile2 = Framing.GetTileSafely(xPos, yPos);
                                while ((!tile2.HasTile || TileID.Sets.Platforms[tile2.TileType]) && yPos <= Main.maxTilesY)
                                {
                                    atleastOnce = true;
                                    yPos++; //go one down
                                    tile2 = Framing.GetTileSafely(xPos, yPos);
                                }
                                //here, found first proper tile, need to go up one tile again
                                if (atleastOnce) yPos--;
                                tile2 = Framing.GetTileSafely(xPos, yPos);

                                //fix for platforms
                                while (TileID.Sets.Platforms[tile2.TileType] && yPos >= originalY)
                                {
                                    yPos--; //go up one
                                    tile2 = Framing.GetTileSafely(xPos, yPos);
                                }
                            }

                        //new tile
                        if (!WorldGen.InWorld(xPos, yPos))
                            continue;
                        tile = Framing.GetTileSafely(xPos, yPos);

                        bool placed = false;
                        if (!tile.HasTile)
                        {
                            placed = WorldGen.PlaceTile(xPos, yPos, tileType, false, false, Main.myPlayer);
                            if (placed && Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, tileType, 0);
                        }
                        //else if (tile.TileType != 0 && PlantTypes.Contains(tile.TileType))
                        else if (tile.TileType != 0 && false)
                        {
                            placed = WorldGen.PlaceTile(xPos, yPos, tileType, false, true, Main.myPlayer);
                            if (placed && Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xPos, yPos, tileType, 0);
                        }

                        if (placed)
                        {
                            tileCounter++;
                            if (tileCount != -1 && tileCounter >= tileCount)
                                return tileCounter;
                        }

                        //NEW
                        //if a tile is placed, check its surrounding tiles for slopeness
                        //if (Config.Instance.UnslopeTiles && placed && tile.HasTile)
                        if (true && placed && tile.HasTile)
                        {
                            int xEdge = xPos;
                            int yEdge = yPos;
                            var offsets = new List<Point>()
                            {
                                new Point(-1, 0),
                                new Point(1, 0),
                                new Point(0, -1),
                                new Point(0, 1),
                            };

                            for (int i = 0; i < offsets.Count; i++)
                            {
                                xEdge = xPos + offsets[i].X;
                                yEdge = yPos + offsets[i].Y;
                                if (WorldGen.InWorld(xEdge, yEdge))
                                {
                                    Tile edge = Framing.GetTileSafely(xEdge, yEdge);
                                    //if (edge.HasTile && !tile.HasSameSlope(edge))
                                    if (edge.HasTile && edge.BlockType != tile.BlockType)

                                    {
                                        WorldGen.SlopeTile(xEdge, yEdge);
                                        //WorldGen.TileFrame(xEdge, yEdge, false, false);
                                        if (Main.netMode == NetmodeID.MultiplayerClient)
                                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, xEdge, yEdge, (float)edge.BlockType, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            //Main.NewText(lastPlacedY[0]);
            return tileCounter;
        }
    }
}
