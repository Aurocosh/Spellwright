using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class StateLockSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 8;
            castSound = SoundID.Item4;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.LightShard, 5)
                .WithCost(ItemID.DarkShard, 5);

            //CastCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 3);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            buffPlayer.StateLockCount = 5;
            Main.NewText(GetTranslation("Stable"));
            return true;
        }
    }
}
