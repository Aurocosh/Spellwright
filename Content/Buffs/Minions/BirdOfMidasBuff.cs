﻿using Spellwright.Content.Minions;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Minions
{
    internal class BirdOfMidasBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bird of midas");
            Description.SetDefault("Golden raven will bring you riches");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minions exist reset the buff time, otherwise remove the buff from the player
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BirdOfMidasMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}