using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class ReturnToFishBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Return To Fish");
            Description.SetDefault("Go back to your aquatic roots");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var spellwrightPlayer = player.GetModPlayer<SpellwrightPlayer>();
            int playerLevel = spellwrightPlayer.PlayerLevel;

            if (playerLevel >= 2)
            {
                player.accFlipper = true;
            }
            if (playerLevel >= 4)
            {
                player.gills = true;
            }
            if (playerLevel >= 6)
            {
                player.ignoreWater = true;
            }

            //if (swimTime <= 10)
            //    swimTime = 30;
        }
    }
}