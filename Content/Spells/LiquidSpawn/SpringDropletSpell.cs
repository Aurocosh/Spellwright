using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Lib.PointShapes;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.LiquidSpawn
{
    internal class SpringDropletSpell : LiquidSpawnSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 20 * playerLevel;

        protected override IEnumerable<Point> GetTilePositions(Point center, Player player, int playerLevel, SpellData spellData)
        {
            return new SolidCircle(center, 2);
        }

        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            stability = 1;
            liquidType = LiquidID.Water;
            useTimeMultiplier = 9f;

            UnlockCost = new SingleItemSpellCost(ItemID.WaterBucket, 10);
            UseCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }
    }
}