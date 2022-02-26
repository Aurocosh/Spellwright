using Spellwright.Common.Players;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class KissOfCloverBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kiss of clover");
            Description.SetDefault("Luck itself gave you a kiss.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noFallDmg = true;

            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            int playerLevel = modPlayer.PlayerLevel;

            if (playerLevel >= 4)
            {
                player.luck += .1f;
                player.luckMaximumCap += .1f;
            }

            if (playerLevel >= 6)
            {
                player.GetCritChance(DamageClass.Throwing) += 10;
                player.GetCritChance(DamageClass.Summon) += 10;
                player.GetCritChance(DamageClass.Ranged) += 10;
                player.GetCritChance(DamageClass.Melee) += 10;
                player.GetCritChance(DamageClass.Magic) += 10;
            }

            if (playerLevel >= 8)
            {
                player.luck += .1f;
                player.luckMaximumCap += .1f;

                if (Main.moonPhase == (int)MoonPhase.Full)
                {
                    player.luck += .3f;
                    player.luckMaximumCap += .3f;
                }
            }
        }
    }
}