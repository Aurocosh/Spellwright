using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class StateLockBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("State Lock");
            Description.SetDefault("Your buffs will transcend death");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 10000;
        }
    }
}