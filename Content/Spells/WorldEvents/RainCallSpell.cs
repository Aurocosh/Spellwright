using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Network;
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
            SpellCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 30);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (!spellData.HasModifier(SpellModifier.Dispel))
            {
                Main.StartRain();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StartRainHandler.Send(true);
            }
            else
            {
                Main.StopRain();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StopRainHandler.Send(true);
            }

            return true;
        }
    }
}