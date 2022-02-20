using Microsoft.Xna.Framework;
using Spellwright.Lib.Primitives;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Spells.WarpSpells
{
    internal class IceBreakerSpell : TileBreakSpell
    {
        public IceBreakerSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
            tileType = TileID.BreakableIce;
        }

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            return new SolidCircle(center, 9);
        }
    }
}