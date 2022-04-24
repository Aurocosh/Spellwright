using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Network;
using Terraria;
using Terraria.GameContent.Events;
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
            {
                Sandstorm.StartSandstorm();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StartSandstormHandler.Send(true);
            }
            else
            {
                Sandstorm.StopSandstorm();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.StopSandstormHandler.Send(true);
            }

            return true;
        }
    }
}