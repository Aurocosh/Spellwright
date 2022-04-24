using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;

namespace Spellwright.Content.Spells.LiquidSpawn
{
    internal class ConjureWaterSpell : LiquidSpawnSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 1;
            stability = 1;
            liquidType = LiquidID.Water;
            useTimeMultiplier = 7f;

            UnlockCost = new SingleItemSpellCost(ItemID.WaterBucket);
        }
    }
}