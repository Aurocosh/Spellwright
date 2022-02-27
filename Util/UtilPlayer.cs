﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.Player;

namespace Spellwright.Util
{
    internal static class UtilPlayer
    {
        public static Vector2 CheckForGoodTeleportationSpot(ref bool canSpawn, Player player, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY, RandomTeleportationAttemptSettings settings)
        {
            int attemptCount = 0;
            int width = player.width;
            Vector2 vector = new Vector2(0, 0) * 16f + new Vector2(-width / 2 + 8, -player.height);
            while (!canSpawn && attemptCount < settings.attemptsBeforeGivingUp)
            {
                attemptCount++;
                int currentX = teleportStartX + Main.rand.Next(teleportRangeX);
                int currentY = teleportStartY + Main.rand.Next(teleportRangeY);

                int distanceFromEdge = 5;
                currentX = (int)MathHelper.Clamp(currentX, distanceFromEdge, Main.maxTilesX - distanceFromEdge);
                currentY = (int)MathHelper.Clamp(currentY, distanceFromEdge, Main.maxTilesY - distanceFromEdge);
                vector = new Vector2(currentX, currentY) * 16f + new Vector2(-width / 2 + 8, -player.height);
                if (Collision.SolidCollision(vector, width, player.height))
                    continue;

                if (Main.tile[currentX, currentY] == null)
                    continue;

                if ((settings.avoidWalls && Main.tile[currentX, currentY].WallType > 0) || (Main.tile[currentX, currentY].WallType == 87 && currentY > Main.worldSurface && !NPC.downedPlantBoss) || (Main.wallDungeon[Main.tile[currentX, currentY].WallType] && currentY > Main.worldSurface && !NPC.downedBoss3))
                    continue;

                int distanceFromOrigin = 0;
                if (settings.maximumFallDistanceFromOrignalPoint > 0)
                {
                    while (distanceFromOrigin < settings.maximumFallDistanceFromOrignalPoint)
                    {
                        if (Main.tile[currentX, currentY + distanceFromOrigin] == null)
                            break;

                        Tile tile = Main.tile[currentX, currentY + distanceFromOrigin];
                        vector = new Vector2(currentX, currentY + distanceFromOrigin) * 16f + new Vector2(-width / 2 + 8, -player.height);
                        Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir);
                        if (!Collision.SolidCollision(vector, width, player.height))
                        {
                            distanceFromOrigin++;
                            continue;
                        }

                        if (tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType])
                            break;

                        distanceFromOrigin++;
                    }
                }

                vector.Y -= 16f;
                int i = (int)vector.X / 16;
                int j = (int)vector.Y / 16;
                int num6 = (int)(vector.X + width * 0.5f) / 16;
                int j2 = (int)(vector.Y + player.height) / 16;
                Tile tileSafely = Framing.GetTileSafely(i, j);
                Tile tileSafely2 = Framing.GetTileSafely(num6, j2);
                if (settings.avoidAnyLiquid && tileSafely2.LiquidAmount > 0)
                    continue;

                if (settings.mostlySolidFloor)
                {
                    Tile tileSafely3 = Framing.GetTileSafely(num6 - 1, j2);
                    Tile tileSafely4 = Framing.GetTileSafely(num6 + 1, j2);
                    if (!tileSafely3.HasTile || tileSafely3.IsActuated || !Main.tileSolid[tileSafely3.TileType] || Main.tileSolidTop[tileSafely3.TileType] || !tileSafely4.HasTile || tileSafely4.IsActuated || !Main.tileSolid[tileSafely4.TileType] || Main.tileSolidTop[tileSafely4.TileType])
                        continue;
                }

                if (settings.avoidWalls && tileSafely.WallType > 0)
                    continue;
                if (settings.avoidAnyLiquid && Collision.WetCollision(vector, width, player.height))
                    continue;
                if (settings.avoidLava && Collision.LavaCollision(vector, width, player.height))
                    continue;
                if (settings.avoidHurtTiles && Collision.HurtTiles(vector, player.velocity, width, player.height).Y > 0f)
                    continue;
                if (Collision.SolidCollision(vector, width, player.height))
                    continue;
                if (settings.maximumFallDistanceFromOrignalPoint > 0 && distanceFromOrigin >= settings.maximumFallDistanceFromOrignalPoint - 1)
                    continue;

                Vector2 vector2 = Vector2.UnitX * 16f;
                if (Collision.TileCollision(vector - vector2, vector2, player.width, player.height, fallThrough: false, fall2: false, (int)player.gravDir) != vector2)
                    continue;

                vector2 = -Vector2.UnitX * 16f;
                if (Collision.TileCollision(vector - vector2, vector2, player.width, player.height, fallThrough: false, fall2: false, (int)player.gravDir) != vector2)
                    continue;

                vector2 = Vector2.UnitY * 16f;
                if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, fallThrough: false, fall2: false, (int)player.gravDir) != vector2))
                {
                    vector2 = -Vector2.UnitY * 16f;
                    if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, fallThrough: false, fall2: false, (int)player.gravDir) != vector2))
                    {
                        canSpawn = true;
                    }
                }
            }

            return vector;
        }
        public static void Teleport(Player player, Vector2 position, bool canTeleport, int teleportStyle, bool resetVelocity)
        {
            int noTeleportSign = 0;
            if (!canTeleport)
            {
                noTeleportSign = 1;
                position = player.position;
            }

            player.Teleport(position, teleportStyle);
            if (resetVelocity)
                player.velocity = Vector2.Zero;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position.X, position.Y, teleportStyle, noTeleportSign);
        }
    }
}
