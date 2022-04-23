using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Utility
{
    public class FishermanBlessingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fisherman Blessing");
            Description.SetDefault("See and fishies smile upon you.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var buffPlayer = player.GetModPlayer<SpellwrightBuffPlayer>();
            int buffPlayerLevel = buffPlayer.GetBuffLevel(ModContent.BuffType<FishermanBlessingBuff>());

            player.fishingSkill += 15;

            if (buffPlayerLevel >= 2)
                player.accFishingLine = true;
            if (buffPlayerLevel >= 3)
                player.accTackleBox = true;
            if (buffPlayerLevel >= 4)
                player.sonarPotion = true;
            if (buffPlayerLevel >= 5)
                player.fishingSkill += 15;
            if (buffPlayerLevel >= 6)
                player.cratePotion = true;
            if (buffPlayerLevel >= 7)
                player.accLavaFishing = true;
        }
    }
}