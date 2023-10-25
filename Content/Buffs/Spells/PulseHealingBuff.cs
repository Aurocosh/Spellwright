using Spellwright.Util;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Buffs.Spells
{
    public class PulseHealingBuff : ModBuff
    {
        private static int healingDelay = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pulse healing");
            // Description.SetDefault("Powerful healing that occures occasionally");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // I think i dont need to sync HealingValue to other clients, and only need to run this code on client side
            // Healing is already synchronized across clients and i cannot sync HealingDelay precisely, so health changes can be chaotic
            if (player.whoAmI != Main.myPlayer)
                return;

            var pulsePlayer = player.GetModPlayer<PulseHealingPlayer>();
            if (healingDelay > 0)
            {
                healingDelay--;
                return;
            }
            healingDelay = UtilTime.MinutesToTicks(3);
            //healingDelay = UtilTime.SecondsToTicks(15);

            player.statLife += pulsePlayer.HealingValue;
            player.HealEffect(pulsePlayer.HealingValue);
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;
        }
    }

    public class PulseHealingPlayer : ModPlayer
    {
        public int HealingValue { get; set; } = 0;

        public override void SaveData(TagCompound tag)
        {
            tag.Add("HealingValue", HealingValue);
        }

        public override void LoadData(TagCompound tag)
        {
            HealingValue = tag.GetInt("HealingValue");
        }
    }
}
