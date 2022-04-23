using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Lib.Primitives;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class IceBreakerSpell : TileBreakSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;
            tileType = TileID.BreakableIce;

            UnlockCost = new SingleItemSpellCost(ItemID.IceBlock, 30);
            SpellCost = new SingleItemSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            return new SolidCircle(center, 9);
        }
    }
}