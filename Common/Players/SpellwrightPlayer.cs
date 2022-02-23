using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Network;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
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
            ModNetHandler.PlayerLevelSync.Send(toWho, fromWho, PlayerLevel);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightPlayer;
            if (clone.PlayerLevel != PlayerLevel)
                ModNetHandler.PlayerLevelSync.Send(PlayerLevel);
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
            if (nextCantripDelay > 0)
                nextCantripDelay--;

            if (Spellwright.OpenIncantationUIHotKey.JustPressed)
                Spellwright.Instance.spellInputState.Activate();
            else if (Spellwright.CastCantripHotKey.Current && nextCantripDelay == 0 && CurrentCantrip != null && CantripData != null)
            {
                Player player = Main.LocalPlayer;
                Vector2 mousePosition = Main.MouseWorld;
                Vector2 center = player.Center;
                Vector2 velocity = center.DirectionTo(mousePosition);
                var projectileSource = new ProjectileSource_Item(player, null);
                if (CurrentCantrip.ConsumeReagents(player, playerLevel, CantripData))
                {
                    CurrentCantrip.Cast(player, PlayerLevel, CantripData, projectileSource, center, velocity);
                    nextCantripDelay = CurrentCantrip.GetUseDelay(PlayerLevel);
                }
            }
        }
    }
}