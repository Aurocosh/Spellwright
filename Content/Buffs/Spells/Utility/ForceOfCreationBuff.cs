﻿using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Utility
{
    public class ForceOfCreationBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Creation");
            Description.SetDefault("You have boundless urge to create.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.tileSpeed += 1f;
            player.wallSpeed += 1f;
            player.blockRange += 2;
        }
    }
}