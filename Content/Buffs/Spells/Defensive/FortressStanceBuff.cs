﻿using Spellwright.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells.Defensive
{
    public class FortressStanceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            player.statDefense += 20 * spellPlayer.PlayerLevel;
        }
    }
}