using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Lib.Primitives;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.TileSpawn
{
    internal class ShellOfIceSpell : TileSpawnSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
            tileType = TileID.BreakableIce;
        }

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            return new RingedCircle(center, 3, 7);
        }
    }
}