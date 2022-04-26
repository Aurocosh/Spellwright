using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Constants;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Projectiles.Tiles
{
    public class WallShredderProjectile : ModProjectile
    {
        private static readonly HashSet<int> blacklistedItems = new();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Shredder");
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X, Projectile.velocity.Y, 50, Color.DarkBlue, 1.2f);
                dust.noGravity = true;
                int max = Main.rand.Next(1, 3) == 1 ? 1 : 3;
                dust.velocity = Main.rand.NextVector2Circular(1, 1) * max;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            int radius = 8;
            Vector2 position = Projectile.Center;
            var centerPoint = position.ToGridPoint();

            int radiusSq = radius * radius;
            bool IsValid(Point point)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
                    return false;
                if (WorldGen.SolidTile(point.X, point.Y))
                    return false;
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (tile.TileType == TileID.Platforms || tile.TileType == TileID.ClosedDoor || tile.TileType == TileID.TrapdoorClosed)
                    return false;

                int distanceToCenterSq = (point - centerPoint).DistanceSq();
                if (distanceToCenterSq > radiusSq)
                    return false;

                return true;
            }

            var circlePoints = UtilCoordinates.FloodFill(new[] { centerPoint }, PointConstants.DirectNeighbours, IsValid, 10000);
            foreach (var point in circlePoints)
            {
                Tile tile = Framing.GetTileSafely(point.X, point.Y);
                if (tile.WallType == 0)
                    continue;

                WorldGen.KillWall(point.X, point.Y);
                if (tile.WallType == 0 && Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, point.X, point.Y);
            }

            UtilDust.SpawnExplosionDust(position, Projectile.velocity, DustID.BlueTorch, Color.DarkBlue, 150, 3);
            SoundEngine.PlaySound(SoundID.Item14, position);
        }
    }
}