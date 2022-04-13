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
            DoAction(player);
        }

        public static void DoAction(Player player)
        {
            player.noFallDmg = true;

            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<KissOfCloverBuff>());

            if (buffPlayerLevel >= 4)
            {
                player.luck += .1f;
                player.luckMaximumCap += .1f;
            }

            if (buffPlayerLevel >= 6)
            {
                player.GetCritChance(DamageClass.Throwing) += 10;
                player.GetCritChance(DamageClass.Summon) += 10;
                player.GetCritChance(DamageClass.Ranged) += 10;
                player.GetCritChance(DamageClass.Melee) += 10;
                player.GetCritChance(DamageClass.Magic) += 10;
            }

            if (buffPlayerLevel >= 8)
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