using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.TileBreak
{
    internal class EvaporateSpell : LiquidDestroySpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Invocation;
            liquidType = LiquidID.Water;

            SpellCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            return base.Cast(player, playerLevel, spellData);
        }
    }
}