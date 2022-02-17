using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Util
{
    internal static class UtilExplosion
    {
        public static void DealExplosionDamage(Projectile projectile, int damage, int radius)
        {
            foreach (NPC npc in Main.npc)
            {
                float distance = Vector2.Distance(npc.Center, projectile.Center);
                if (distance / 16f <= radius)
                {
                    int direction = distance > 0 ? 1 : -1;
                    npc.StrikeNPC(damage, projectile.knockBack, direction, false);
                }
            }

            foreach (Player player in Main.player)
            {
                if (player == null || player.whoAmI == 255 || !player.active)
                    return;
                if (!ProjectileLoader.CanHitPlayer(projectile, player) || !PlayerLoader.CanBeHitByProjectile(player, projectile))
                    continue;

                float distance = Vector2.Distance(player.Center, projectile.Center);
                int dir = distance > 0 ? 1 : -1;
                if (distance / 16f <= radius && Main.netMode == NetmodeID.SinglePlayer)
                {
                    player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, projectile.whoAmI), damage, dir);
                    player.hurtCooldowns[0] += 20;
                }
                else if (Main.netMode != NetmodeID.MultiplayerClient && distance / 16f <= radius && player.whoAmI == projectile.owner)
                {
                    NetMessage.SendPlayerHurt(projectile.owner, PlayerDeathReason.ByProjectile(player.whoAmI, projectile.whoAmI), damage, dir, false, pvp: true, 0);
                }
            }
        }


        public static void ExplodeTiles(Vector2 position, int radius, bool wallSplode)
        {
            int minI = (int)(position.X / 16f - (float)radius);
            int maxI = (int)(position.X / 16f + (float)radius);
            int minJ = (int)(position.Y / 16f - (float)radius);
            int maxJ = (int)(position.Y / 16f + (float)radius);
            if (minI < 0)
                minI = 0;

            ExplodeTiles(position, radius, minI, maxI, minJ, maxJ, wallSplode);
        }

        public static void ExplodeTiles(Vector2 position, int radius, int minI, int maxI, int minJ, int maxJ, bool wallSplode)
        {
            AchievementsHelper.CurrentlyMining = true;
            for (int i = minI; i <= maxI; i++)
            {
                for (int j = minJ; j <= maxJ; j++)
                {
                    float num = Math.Abs((float)i - position.X / 16f);
                    float num2 = Math.Abs((float)j - position.Y / 16f);
                    if (!(Math.Sqrt(num * num + num2 * num2) < (double)radius))
                        continue;

                    bool flag = true;
                    if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                    {
                        flag = CanExplodeTile(i, j);
                        if (flag)
                        {
                            WorldGen.KillTile(i, j);
                            if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                        }
                    }

                    if (!flag)
                        continue;

                    if (wallSplode)
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int l = j - 1; l <= j + 1; l++)
                            {
                                if (Main.tile[k, l] != null && Main.tile[k, l].WallType > 0 && wallSplode && WallLoader.CanExplode(k, l, Main.tile[k, l].WallType))
                                {
                                    WorldGen.KillWall(k, l);
                                    if (Main.tile[k, l].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer)
                                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, k, l);
                                }
                            }
                        }
                    }
                }
            }

            AchievementsHelper.CurrentlyMining = false;
        }

        public static bool CanExplodeTile(int x, int y)
        {
            if (Main.tileDungeon[Main.tile[x, y].TileType] || TileID.Sets.BasicChest[Main.tile[x, y].TileType])
                return false;

            if (!TileLoader.CanExplode(x, y))
                return false;

            switch (Main.tile[x, y].TileType)
            {
                case TileID.DemonAltar:
                case TileID.Dressers:
                case TileID.Cobalt:
                case TileID.Mythril:
                case TileID.Adamantite:
                case TileID.Chlorophyte:
                case TileID.Palladium:
                case TileID.Orichalcum:
                case TileID.Titanium:
                case TileID.LihzahrdBrick:
                case TileID.LihzahrdAltar:
                case TileID.DisplayDoll:
                case TileID.HatRack:
                    return false;
                case TileID.Meteorite:
                case TileID.Hellstone:
                    if (!Main.hardMode)
                        return false;
                    break;
                case TileID.Hellforge:
                    if (!Main.hardMode && y >= Main.UnderworldLayer)
                        return false;
                    break;
                case TileID.Traps:
                    if (!NPC.downedGolemBoss)
                    {
                        int num = Main.tile[x, y].TileFrameY / 18;
                        if ((uint)(num - 1) <= 3u)
                            return false;
                    }
                    break;
            }

            return true;
        }
    }
}
