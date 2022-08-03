using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Network.ServerPackets.WorldEvents.SandstormEvents;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.WorldEvents
{
    internal class DesertRiteSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            UseType = SpellType.Invocation;

            AddApplicableModifier(SpellModifier.Dispel);

            UnlockCost = new SingleItemSpellCost(ItemID.SandBlock, 200);
            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 30);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.Dispel))
                new StartSandstormAction().Execute();
            else
                new StopSandstormAction().Execute();
            return true;
        }
    }
}