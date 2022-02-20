using Microsoft.Xna.Framework;
using Spellwright.Constants;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Spells.WarpSpells
{
    internal class SnuffOutSpell : TileBreakSpell
    {
        public SnuffOutSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
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

            return UtilCoordinates.FloodFill(center, PointConstants.DirectNeighbours, IsValid, 3000);
        }
    }
}