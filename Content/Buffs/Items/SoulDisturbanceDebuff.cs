using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.DustSpawners;
using Spellwright.Extensions;
using Spellwright.Network;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace Spellwright.Content.Buffs.Items
{
    public class SoulDisturbanceDebuff : ModBuff
    {
        private static readonly int MinBuffChangeDelay = UtilTime.SecondsToTicks(10);
        private static readonly int MaxBuffChangeDelay = UtilTime.SecondsToTicks(25);

        private static readonly int MinHealthChangeDelay = UtilTime.SecondsToTicks(2);
        private static readonly int MaxHealthChangeDelay = UtilTime.SecondsToTicks(10);

        private static readonly int MinEventChangeDelay = UtilTime.SecondsToTicks(5);
        private static readonly int MaxEventChangeDelay = UtilTime.SecondsToTicks(10);

        private static readonly int[] damageDealingDebuffs = new int[] { BuffID.OnFire, BuffID.Frostburn, BuffID.CursedInferno, BuffID.Bleeding };
        private static readonly int[] generalDebuffs = new int[] { BuffID.Confused, BuffID.Silenced, BuffID.Blackout, BuffID.Cursed, BuffID.Slow, BuffID.Darkness, BuffID.Weak, BuffID.WitheredArmor, BuffID.WitheredWeapon };
        private static readonly int[] availableBuffs = new int[] { BuffID.Dangersense, BuffID.Calm, BuffID.Featherfall, BuffID.Gravitation, BuffID.Ironskin, BuffID.Lucky, BuffID.ObsidianSkin, BuffID.Rage, BuffID.Wrath, BuffID.Regeneration };
        private static readonly int[] allAvailableDebuffs;

        static SoulDisturbanceDebuff()
        {
            allAvailableDebuffs = new int[damageDealingDebuffs.Length + generalDebuffs.Length];
            damageDealingDebuffs.CopyTo(allAvailableDebuffs, 0);
            generalDebuffs.CopyTo(allAvailableDebuffs, damageDealingDebuffs.Length);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul disturbance");
            Description.SetDefault("Your soul is in turmoil");
            //Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI != Main.myPlayer)
                return;

            int soulDisturbanceId = ModContent.BuffType<SoulDisturbanceDebuff>();
            int disturbanceIndex = player.FindBuffIndex(soulDisturbanceId);
            var buffTime = player.buffTime[disturbanceIndex];

            if (buffTime < UtilTime.SecondsToTicks(10))
                HandleLevel(player);
            else
            {
                var modPlayer = player.GetModPlayer<SoulDisturbancePlayer>();

                HandleDamageDealingBuffs(player);
                HandleBuffs(player, modPlayer);
                HandleHealth(player, modPlayer);
                HandleEvents(player, modPlayer);
            }
        }

        private static void HandleLevel(Player player)
        {
            var spellwrightPlayer = player.GetModPlayer<SpellwrightPlayer>();

            if (spellwrightPlayer.PlayerLevel == 0)
            {
                spellwrightPlayer.PlayerLevel = 1;
                player.ClearBuff(ModContent.BuffType<SoulDisturbanceDebuff>());

                var spawner = new SoulDisturbanceSpawner(player);
                spawner.Spawn();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.soulDisturbanceHandler.Send(spawner);
            }
        }

        private static void HandleDamageDealingBuffs(Player player)
        {
            if (player.statLife > player.statLifeMax2 * 0.15f)
                return;

            if (player.HasBuff(BuffID.OnFire))
                player.ClearBuff(BuffID.OnFire);
            if (player.HasBuff(BuffID.Frostburn))
                player.ClearBuff(BuffID.Frostburn);
            if (player.HasBuff(BuffID.Bleeding))
                player.ClearBuff(BuffID.Bleeding);
            if (player.HasBuff(BuffID.CursedInferno))
                player.ClearBuff(BuffID.CursedInferno);
        }

        private static void HandleBuffs(Player player, SoulDisturbancePlayer modPlayer)
        {
            if (--modPlayer.BuffChangesDelay > 0)
                return;

            modPlayer.BuffChangesDelay = Main.rand.Next(MinBuffChangeDelay, MaxBuffChangeDelay);

            int soulDisturbanceId = ModContent.BuffType<SoulDisturbanceDebuff>();
            var buffIds = new List<int>();
            for (int i = 0; i < MaxBuffs; i++)
            {
                int buffTime = player.buffTime[i];
                int buffType = player.buffType[i];
                if (buffType != soulDisturbanceId)
                    if (buffTime > 0)
                        buffIds.Add(buffType);
            }

            if (buffIds.Count > 0)
                player.ClearBuffs(buffIds);

            int debuffCount = Main.rand.Next(2, 6);
            int buffCount = Main.rand.Next(1, 2);

            var debuffs = generalDebuffs;
            if (player.statLife > player.statLifeMax2 * .5f)
                debuffs = allAvailableDebuffs;

            var selectedDebuffs = new HashSet<int>();
            int limit = 200;
            while (selectedDebuffs.Count < debuffCount && limit-- > 0)
            {
                int buffId = debuffs.GetRandom();
                selectedDebuffs.Add(buffId);
            }

            var selectedBuffs = new HashSet<int>();
            limit = 200;
            while (selectedBuffs.Count < buffCount && limit-- > 0)
            {
                int buffId = availableBuffs.GetRandom();
                selectedBuffs.Add(buffId);
            }

            var finalBuffIds = selectedDebuffs.Concat(selectedBuffs).ToArray();
            if (finalBuffIds.Length > 0)
            {
                finalBuffIds.Shuffle();
                int time = Main.rand.Next(MinBuffChangeDelay, MaxBuffChangeDelay * 2);
                foreach (var debuff in finalBuffIds)
                    player.AddBuff(debuff, time);
            }

            var spawner = new SoulDisturbanceSpawner(player);
            spawner.Spawn();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.soulDisturbanceHandler.Send(spawner);
        }

        private static void HandleHealth(Player player, SoulDisturbancePlayer modPlayer)
        {
            if (--modPlayer.HealthChangesDelay > 0)
                return;

            modPlayer.HealthChangesDelay = Main.rand.Next(MinHealthChangeDelay, MaxHealthChangeDelay);

            var possibleOptions = new List<int>();
            if (player.statLife < player.statLifeMax2 * .95f)
                possibleOptions.Add(0);
            if (player.statLife > player.statLifeMax2 * .25f)
                possibleOptions.Add(1);

            int action = possibleOptions.GetRandom();
            if (action == 0)
            {
                int maxHeal = player.statLifeMax2 - player.statLife;
                int minHeal = (int)(maxHeal * .3f);
                int heal = Main.rand.Next(minHeal, maxHeal);
                player.statLife += heal;
                player.HealEffect(heal);
            }
            else if (action == 1)
            {
                int maxDamage = Math.Max(player.statLife - (int)(player.statLifeMax2 * .15f), 0);
                int minDamage = (int)(maxDamage * .2f);

                int damage = Main.rand.Next(minDamage, maxDamage);
                player.Hurt(PlayerDeathReason.ByCustomReason("Soul could not hold out"), damage, 0, false, true);
            }

            var spawner = new SoulDisturbanceSpawner(player);
            spawner.Spawn();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.soulDisturbanceHandler.Send(spawner);
        }

        private static void HandleEvents(Player player, SoulDisturbancePlayer modPlayer)
        {
            if (--modPlayer.EventsDelay > 0)
                return;

            modPlayer.EventsDelay = Main.rand.Next(MinEventChangeDelay, MaxEventChangeDelay);

            int action = Main.rand.Next(12);

            if (action == 0)
            {
                bool canTeleport = false;
                int teleportStartX = 100;
                int teleportRangeX = Main.maxTilesX - 200;
                int teleportStartY = 100;
                int underworldLayer = Main.UnderworldLayer;
                var attemptSetting = new RandomTeleportationAttemptSettings
                {
                    avoidLava = true,
                    avoidHurtTiles = true,
                    maximumFallDistanceFromOrignalPoint = 100,
                    attemptsBeforeGivingUp = 1000
                };
                Vector2 position = player.CheckForGoodTeleportationSpot(ref canTeleport, teleportStartX, teleportRangeX, teleportStartY, underworldLayer, attemptSetting);
                UtilPlayer.Teleport(player, position, canTeleport, 2, true);
            }
            else
            {
                SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
                if (dashPlayer.CanUseDash())
                {
                    Vector2 velocity = Main.rand.NextVector2Unit();
                    velocity.Normalize();
                    velocity *= Main.rand.NextFloat(12, 20);
                    dashPlayer.Dash(velocity, 30);
                }
            }

            var spawner = new SoulDisturbanceSpawner(player);
            spawner.Spawn();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.soulDisturbanceHandler.Send(spawner);
        }
    }

    public class SoulDisturbancePlayer : ModPlayer
    {
        public int BuffChangesDelay { get; set; } = 0;
        public int HealthChangesDelay { get; set; } = 0;
        public int EventsDelay { get; set; } = 0;
    }
}