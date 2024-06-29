using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base;
using Terraria.ID;
using Terraria;
using Spellwright.Content.Items.Reagents;
using Terraria.ModLoader;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Modifiers;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class CycleOfEternitySpell : ModSpell
    {
        public override bool ConsumeReagentsCast(Player player, int playerLevel, SpellData spellData)
        {
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            if (buffPlayer.CycleOfEternity)
                return true;
            if (spellData.HasModifier(SpellModifier.Dispel))
                return true;

            return base.ConsumeReagentsCast(player, playerLevel, spellData);
        }

        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            castSound = SoundID.Item4;

            UnlockCost = new SingleItemSpellCost(ItemID.DaoofPow);
            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 50);

            AddApplicableModifier(SpellModifier.Dispel);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();

            if (buffPlayer.CycleOfEternity && spellData.HasModifier(SpellModifier.Dispel))
            {
                buffPlayer.CycleOfEternity = false;
                Main.NewText(GetTranslation("Disconnected"));
            }
            else if (!buffPlayer.CycleOfEternity)
            {
                buffPlayer.CycleOfEternity = true;
                Main.NewText(GetTranslation("Connected"));
            }
            return true;
        }
    }
}
