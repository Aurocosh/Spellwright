using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Buffs.Spells
{
    public class PulseHealingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulse healing");
            Description.SetDefault("Powerful healing that occures occasionally");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var pulsePlayer = player.GetModPlayer<PulseHealingPlayer>();
            if (pulsePlayer.HealingDelay > 0)
            {
                pulsePlayer.HealingDelay--;
                return;
            }
            pulsePlayer.HealingDelay = UtilTime.MinutesToTicks(3);
            //pulsePlayer.HealingDelay = UtilTime.SecondsToTicks(15);

            player.statLife += pulsePlayer.HealingValue;
            player.HealEffect(pulsePlayer.HealingValue);
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;
        }
    }

    public class PulseHealingPlayer : ModPlayer
    {
        public int HealingDelay { get; set; } = 0;
        public int HealingValue { get; set; } = 0;

        public override void SaveData(TagCompound tag)
        {
            tag.Add("HealingDelay", HealingDelay);
            tag.Add("HealingValue", HealingValue);
        }

        public override void LoadData(TagCompound tag)
        {
            HealingDelay = tag.GetInt("HealingDelay");
            HealingValue = tag.GetInt("HealingValue");
        }
    }
}
