using Spellwright.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class GaleForceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gale force");
            Description.SetDefault("Force of gale bolsters your movement");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var spellwrightPlayer = player.GetModPlayer<SpellwrightPlayer>();
            int playerLevel = spellwrightPlayer.PlayerLevel;

            float maxSpeedMult = 1;

            if (playerLevel >= 3)
            {
                maxSpeedMult = 1.3f;
            }
            if (playerLevel >= 5)
            {
                Player.jumpHeight += 15;
                player.jumpSpeedBoost += 2.4f;
                player.extraFall += 10;
            }
            if (playerLevel >= 7)
            {
                player.runAcceleration *= 1.5f;
            }
            if (playerLevel >= 9)
            {
                maxSpeedMult *= 1.5f;
                Player.jumpHeight += 15;
                player.jumpSpeedBoost += 2.4f;
                player.extraFall += 10;
            }

            player.maxRunSpeed *= maxSpeedMult;
        }
    }
}