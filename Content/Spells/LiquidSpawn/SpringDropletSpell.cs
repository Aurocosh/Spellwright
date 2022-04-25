using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.LiquidSpawn
{
    internal class SpringDropletSpell : LiquidSpawnSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 20 * playerLevel;

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