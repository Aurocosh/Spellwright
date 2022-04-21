using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Lib.Constants;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class TorchMufflerSpell : TileBreakSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            UseType = SpellType.Invocation;

            tileType = TileID.Torches;
            noItem = false;
        }

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            static bool IsValid(Point point)
            {
                if (!WorldGen.InWorld(point.X, point.Y))
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