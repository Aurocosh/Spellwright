﻿using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Sigils
{
    public class SigilOfSageBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Sage");
            Description.SetDefault("Your wisdom resonates with the world.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[ModContent.BuffType<SigilOfBerserkerBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfLegionBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfSniperBuff>()] = true;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<SigilOfSageBuff>());

            float maxBonus = 1.5f * (buffPlayerLevel / 10f);
            float bonusCoeff = player.statMana / (float)player.statManaMax;

            player.GetCritChance(DamageClass.Magic) += maxBonus * bonusCoeff;
            player.GetDamage(DamageClass.Magic) += maxBonus * bonusCoeff;
        }
    }
}