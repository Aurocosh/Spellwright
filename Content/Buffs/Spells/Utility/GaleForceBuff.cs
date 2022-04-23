using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Utility
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
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<GaleForceBuff>());

            float maxSpeedMult = 1;

            if (buffPlayerLevel >= 3)
                maxSpeedMult = 1.3f;
            if (buffPlayerLevel >= 5)
            {
                Player.jumpHeight += 15;
                player.jumpSpeedBoost += 2.4f;
                player.extraFall += 10;
            }
            if (buffPlayerLevel >= 7)
                player.runAcceleration *= 2.0f;
            if (buffPlayerLevel >= 9)
            {
                maxSpeedMult *= 1.4f;
                Player.jumpHeight += 15;
                player.jumpSpeedBoost += 2.4f;
                player.extraFall += 10;
            }

            player.maxRunSpeed *= maxSpeedMult;
        }
    }
}