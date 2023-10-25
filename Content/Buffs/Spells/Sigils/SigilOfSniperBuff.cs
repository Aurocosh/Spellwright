using Spellwright.Common.Players;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Sigils
{
    public class SigilOfSniperBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sigil of Sniper");
            // Description.SetDefault("Your can see beyound the horizon");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[ModContent.BuffType<SigilOfBerserkerBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfLegionBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SigilOfSageBuff>()] = true;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<SigilOfSniperBuff>());

            player.scope = true;

            float velocityX = Math.Abs(player.velocity.X);
            float velocityY = Math.Abs(player.velocity.Y);
            float largestVelocity = Math.Max(velocityX, velocityY);
            if (largestVelocity < 0.1f)
            {
                float maxBonus = 1 * (buffPlayerLevel / 10f);
                player.GetCritChance(DamageClass.Ranged) += (int)(100 * maxBonus);
                player.GetDamage(DamageClass.Ranged) *= 1 + maxBonus;
            }
        }
    }
}