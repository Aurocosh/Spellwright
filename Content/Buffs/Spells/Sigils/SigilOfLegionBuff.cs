using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Sigils
{
    public class SigilOfLegionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sigil of Legion");
            // Description.SetDefault("Your mighty army quakes the earth.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[ModContent.BuffType<SigilOfBerserkerBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfSageBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfSniperBuff>()] = true;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<SigilOfLegionBuff>());

            int extraMinions = buffPlayerLevel / 2;
            player.maxMinions += extraMinions;
            player.maxTurrets += extraMinions;
        }
    }
}