using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles.Tiles
{
    public class WallSpitterProjectile : ModProjectile
    {
        private static readonly HashSet<int> blacklistedItems = new();

        public override void SetStaticDefaults()
        {
            blacklistedItems.Clear();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 600;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;

            AIType = ProjectileID.Shuriken;
        }
        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X, Projectile.velocity.Y, 50, Color.DarkGray, 1.2f);
                dust.noGravity = true;
                int max = Main.rand.Next(1, 3) == 1 ? 1 : 3;
                dust.velocity = Main.rand.NextVector2Circular(1, 1) * max;
            }
        }

        public static Item GetValidItem(Player player)
        {
            foreach (var i in player.GetInventoryIndexes(InventoryArea.Hotbar))
            {
                var item = player.inventory[i];
                if (item.stack > 0 && item.createWall != -1 && !blacklistedItems.Contains(item.type))
                    return item;
            }
            return null;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            var owner = Main.player[Projectile.owner];
            Item selectedItem = GetValidItem(owner);
            if (selectedItem == null)
                return;

            int itemCount = owner.CountItems(selectedItem.type);
            int itemsLeft = itemCount;

            int radius = 8;
            Vector2 position = Projectile.Center;
            var centerPoint = position.ToGridPoint();

            var explosionArea = new SolidCircle(centerPoint, radius);
            bool IsValid(Point point)
            {
                if (!explosionArea.IsInBounds(point))
                    return false;
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;
                if (WorldGen.SolidTile(point.X, point.Y))
                    return false;
                return true;
            }

            var wallType = selectedItem.createWall;
            var circlePoints = UtilCoordinates.FloodFill(new[] { centerPoint }, PointConstants.DirectNeighbours, IsValid, 10000);
            foreach (var point in circlePoints)
            {
                if (itemsLeft <= 0)
                    break;

                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (tile.WallType > 0)
                    continue;

                WorldGen.PlaceWall(point.X, point.Y, wallType);
                if (tile.WallType == wallType)
                {
                    itemsLeft--;
                    WallLoader.PlaceInWorld(point.X, point.Y, selectedItem);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 3, point.X, point.Y, wallType);
                }
            }

            int itemsConsumed = itemCount - itemsLeft;
            owner.ConsumeItems(selectedItem.type, itemsConsumed, InventoryArea.All, true);

            UtilDust.SpawnExplosionDust(position, Projectile.velocity, DustID.Stone, Color.DarkGray, 150, 3);
            SoundEngine.PlaySound(SoundID.Item14, position);
        }
    }
}