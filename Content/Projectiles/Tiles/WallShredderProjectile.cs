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
    public class WallShredderProjectile : ModProjectile
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
        public override void OnKill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            int radius = 8;
            Vector2 position = Projectile.Center;
            var centerPoint = position.ToGridPoint();

            var explosionArea = new SolidCircle(centerPoint, radius);
            (bool, bool) IsValid(Point nextPoint, bool previousPassable)
            {
                if (explosionArea.IsInBounds(nextPoint) && WorldGen.InWorld(nextPoint.X, nextPoint.Y))
                {
                    Tile nextTile = Framing.GetTileSafely(nextPoint.X, nextPoint.Y);
                    bool currentPassable = !WorldGen.SolidTile(nextPoint.X, nextPoint.Y) && nextTile.TileType != TileID.Platforms && nextTile.TileType != TileID.ClosedDoor && nextTile.TileType != TileID.TrapdoorClosed;
                    if (previousPassable && (previousPassable || currentPassable))
                        return (true, currentPassable);
                }

                return (false, false);
            }

            var circlePoints = UtilCoordinates.LookBackFloodFill(new[] { centerPoint }, true, PointConstants.DirectNeighbours, IsValid, 10000);
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