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
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<ReturnToFishBuff>());

            if (buffPlayerLevel >= 2)
            {
                player.accFlipper = true;
            }
            if (buffPlayerLevel >= 4)
            {
                player.gills = true;
            }
            if (buffPlayerLevel >= 6)
            {
                player.ignoreWater = true;
            }
        }
    }
}