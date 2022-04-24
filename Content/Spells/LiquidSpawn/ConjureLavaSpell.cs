using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.LiquidSpawn
{
    internal class ConjureLavaSpell : LiquidSpawnSpell
    {
        public override int GetGuaranteedUses(int playerLevel) => 20 + 2 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 8;
            UseType = SpellType.Spell;

            liquidType = LiquidID.Lava;
            useTimeMultiplier = 7f;

            UnlockCost = new SingleItemSpellCost(ItemID.LavaBucket, 10);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 20);
        }
    }
}