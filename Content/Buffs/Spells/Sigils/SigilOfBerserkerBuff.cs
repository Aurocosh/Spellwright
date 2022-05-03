using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Sigils
{
    public class SigilOfBerserkerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Berserker");
            Description.SetDefault("Your fury knows no bounds.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[ModContent.BuffType<SigilOfLegionBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfSageBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfSniperBuff>()] = true;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<SigilOfBerserkerBuff>());

            player.autoReuseGlove = true;
            player.statDefense += 2 * buffPlayerLevel;

            float maxBonus = 3 * (buffPlayerLevel / 10f);
            float bonusCoeff = 1 - player.statLife / (float)player.statLifeMax2;

            player.GetAttackSpeed(DamageClass.Melee) += 12;
            player.GetCritChance(DamageClass.Melee) += (int)(100 * bonusCoeff);
            player.GetDamage(DamageClass.Melee) += maxBonus * bonusCoeff;
        }
    }
}