using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Lib.Constants;
using Spellwright.Lib.PointShapes;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class TorchEaterSpell : TileBreakSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            UseType = SpellType.Invocation;
            noItem = false;
            var sound = SoundID.Item14;
            sound.PitchVariance = .4f;
            sound.Volume = .2f;
            castSound = sound;
        }

        protected override bool CanBreakTile(Tile tile, int x, int y, int playerLevel)
        {
            return tile.TileType == TileID.Torches;
        }

        protected override void DoAreaEffect(Point point, Player player)
        {
            if (Main.rand.NextFloat() < .05f)
            {
                var direction = (point.ToWorldCoordinates().DirectionTo(player.Center)) * 10;
                var position = point.ToWorldCoordinates(0, 0);
                var dust = Dust.NewDustDirect(position, 16, 16, DustID.CrimsonPlants, direction.X, direction.Y, 50, Color.White, 1.2f);
                dust.noGravity = true;
            }
        }

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            var circle = new SolidCircle(center, 40);
            Vector2 playerCenter = player.Center;

            bool IsValid(Point point)
            {
                if (!circle.IsInBounds(point) || !WorldGen.InWorld(point.X, point.Y))
                    return false;
                Tile tile = Main.tile[point.X, point.Y];
                if (WorldGen.SolidTile(tile))
                    return false;
                return true;
            }

            return UtilCoordinates.FloodFill(new[] { center }, PointConstants.DirectNeighbours, IsValid, 3000);
        }
    }
}