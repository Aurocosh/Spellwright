using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class BurningSoulBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Soul");
            Description.SetDefault("Your soul is set ablaze.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Regeneration] = true;
            player.buffImmune[BuffID.RapidHealing] = true;
            player.buffImmune[ModContent.BuffType<PulseHealingBuff>()] = true;
            player.buffImmune[ModContent.BuffType<SurgeOfLifeBuff>()] = true;

            player.manaRegenBuff = true;
            player.manaRegenDelay--;
            player.GetCritChance(DamageClass.Magic) += 50;
            player.GetDamage(DamageClass.Magic) += 100;

            var soulPlayer = player.GetModPlayer<BurningSoulPlayer>();
            soulPlayer.noLifeRegen = true;

            Vector2 position = player.Center;
            position.X -= 3;

            Vector2 dustPosition = position + Main.rand.NextVector2CircularEdge(0.5f, 1).ScaleRandom(0, 50);
            var dust = Dust.NewDustDirect(dustPosition, 0, 0, DustID.GreenTorch, 0f, 0f, 100, default, 1.5f);
            dust.noLightEmittence = true;
            dust.noGravity = true;
        }

        public class BurningSoulPlayer : ModPlayer
        {
            public bool noLifeRegen;

            public override void ResetEffects()
            {
                noLifeRegen = false;
            }

            public override void UpdateBadLifeRegen()
            {
                if (noLifeRegen)
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 4;
                }
            }
        }
    }
}