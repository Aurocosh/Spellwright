﻿using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Network;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    internal class SpellwrightPlayer : ModPlayer
    {
        private int nextCantripDelay = 0;

        public int GuaranteedUsesLeft = 0;
        public ModSpell CurrentSpell = null;
        public ModSpell CurrentCantrip = null;
        public SpellData SpellData = null;
        public SpellData CantripData = null;
        private int playerLevel = 0;

        public static SpellwrightPlayer Instance => Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();

        public int PlayerLevel
        {
            get => playerLevel;
            set => playerLevel = Math.Clamp(value, 0, 10);
        }

        public override void Initialize()
        {
        }

        public override void clientClone(ModPlayer clientClone)
        {
            var clone = clientClone as SpellwrightPlayer;
            clone.PlayerLevel = PlayerLevel;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            int playerId = Player.whoAmI;
            ModNetHandler.PlayerLevelSync.Sync(toWho, playerId, playerId, PlayerLevel);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightPlayer;
            if (clone.PlayerLevel != PlayerLevel)
                ModNetHandler.PlayerLevelSync.Sync(Player.whoAmI, PlayerLevel);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("PlayerLevel", PlayerLevel);
            tag.Add("GuaranteedUsesLeft", GuaranteedUsesLeft);

            if (CurrentSpell != null && SpellData != null)
            {
                tag.Add("CurrentSpell", CurrentSpell.Name ?? "");
                tag.Add("CurrentSpellData", CurrentSpell.SerializeData(SpellData));
            }
            if (CurrentCantrip != null && CantripData != null)
            {
                tag.Add("CurrentCantrip", CurrentCantrip.Name ?? "");
                tag.Add("CurrentCantripData", CurrentCantrip.SerializeData(CantripData));
            }
        }

        public override void LoadData(TagCompound tag)
        {
            PlayerLevel = tag.GetInt("PlayerLevel");
            GuaranteedUsesLeft = tag.GetInt("GuaranteedUsesLeft");

            string spellName = tag.GetString("CurrentSpell");
            if (ModContent.TryFind(Spellwright.Instance.Name, spellName, out CurrentSpell))
            {
                TagCompound spellDataTag = tag.GetCompound("CurrentSpellData");
                SpellData = CurrentSpell.DeserializeData(spellDataTag);
            }

            string cantripName = tag.GetString("CurrentCantrip");
            if (ModContent.TryFind(Spellwright.Instance.Name, cantripName, out CurrentCantrip))
            {
                TagCompound spellDataTag = tag.GetCompound("CurrentCantripData");
                CantripData = CurrentSpell.DeserializeData(spellDataTag);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Player player = Main.LocalPlayer;

            bool canAutoReuseCantrip = false;
            if (CurrentCantrip != null)
                canAutoReuseCantrip = CurrentCantrip.CanAutoReuse(PlayerLevel);

            if (nextCantripDelay == 1 && !canAutoReuseCantrip)
            {
                var sound = SoundID.Item25.WithVolume(0.2f).WithPitchVariance(0.7f);
                SoundEngine.PlaySound(sound, player.Center);
            }

            if (nextCantripDelay > 0)
                nextCantripDelay--;

            if (Spellwright.OpenIncantationUIHotKey.JustPressed)
                Spellwright.Instance.userInterface.SetState(Spellwright.Instance.spellInputState);
            else if (Spellwright.CastCantripHotKey.JustPressed || Spellwright.CastCantripHotKey.Current)
            {
                bool singleCasted = Spellwright.CastCantripHotKey.JustPressed && !canAutoReuseCantrip;
                bool continuosCast = Spellwright.CastCantripHotKey.Current && canAutoReuseCantrip;

                if ((singleCasted || continuosCast) && nextCantripDelay == 0 && CurrentCantrip != null && CantripData != null)
                {
                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 center = player.Center;
                    Vector2 velocity = center.DirectionTo(mousePosition);
                    var projectileSource = new EntitySource_Parent(player);
                    if (CurrentCantrip.ConsumeReagents(player, playerLevel, CantripData))
                        CurrentCantrip.Cast(player, PlayerLevel, CantripData, projectileSource, center, velocity);

                    nextCantripDelay = CurrentCantrip.GetUseDelay(PlayerLevel);
                }
            }
        }
    }
}