﻿using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Buffs.Spells
{
    public class SelfDefenseHexBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public class SelfDefenseHexPlayer : ModPlayer
        {
            public override void PostHurt(Player.HurtInfo info)
            {
                if (!Player.HasBuff(ModContent.BuffType<SelfDefenseHexBuff>()))
                    return;

                int time = UtilTime.SecondsToTicks(30);
                var npcs = UtilNpc.GetNpcInRadius(Player.Center, 18);
                foreach (var npc in npcs)
                {
                    if (npc.townNPC)
                        continue;
                    if (npc.friendly)
                        continue;

                    if (Main.rand.NextFloat() < .2f)
                        npc.AddBuff(BuffID.OnFire, time);
                    if (Main.rand.NextFloat() < .2f)
                        npc.AddBuff(BuffID.Poisoned, time);
                    if (Main.rand.NextFloat() < .2f)
                        npc.AddBuff(BuffID.Venom, time);
                    if (Main.rand.NextFloat() < .2f)
                        npc.AddBuff(BuffID.CursedInferno, time);
                    if (Main.rand.NextFloat() < .2f)
                        npc.AddBuff(BuffID.Frostburn, time);
                    if (Main.rand.NextFloat() < .2f)
                        npc.AddBuff(BuffID.ShadowFlame, time);
                }
            }
        }
    }
}