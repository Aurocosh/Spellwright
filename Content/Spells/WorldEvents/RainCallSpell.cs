using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Network.ServerPackets.WorldEvents.RainEvents;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.WorldEvents
{
    internal class RainCallSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;

            AddApplicableModifier(SpellModifier.Dispel);

            UnlockCost = new SingleItemSpellCost(ItemID.Umbrella);
            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 30);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.Dispel))
            {
                new StartRainAction().Execute();
            }
            else
            {
                new StopRainAction().Execute();
            }

            return true;
        }
    }
}