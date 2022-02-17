using Microsoft.Xna.Framework;
using Spellwright.Spells;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Players
{
    internal class SpellwrightPlayer : ModPlayer
    {
        private int nextCantripDelay = 0;

        public int GuaranteedUsesLeft = 0;
        public Spell CurrentSpell = null;
        public Spell CurrentCantrip = null;
        public SpellData SpellData = null;
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

        public override void SaveData(TagCompound tag)
        {
            tag.Add("GuaranteedUsesLeft", GuaranteedUsesLeft);
            tag.Add("CurrentSpell", CurrentSpell?.InternalName ?? "");
        }

        public override void LoadData(TagCompound tag)
        {
            GuaranteedUsesLeft = tag.GetInt("GuaranteedUsesLeft");
            string spellName = tag.GetString("CurrentSpell");
            CurrentSpell = Spellwright.instance.spellLibrary.GetSpellByName(spellName);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (nextCantripDelay > 0)
                nextCantripDelay--;

            if (Spellwright.OpenIncantationUIHotKey.JustPressed)
            {
                Spellwright.instance.spellInput.Show();
            }
            else if (Spellwright.CastCantripHotKey.Current && nextCantripDelay == 0 && CurrentCantrip != null)
            {
                Player player = Main.LocalPlayer;

                Vector2 mousePosition = Main.MouseWorld;
                Vector2 spawnPosition = player.position;
                spawnPosition.Y += player.height / 2f;

                Vector2 velocity = mousePosition - spawnPosition;

                var projectileSource = new ProjectileSource_Item(player, null);
                CurrentCantrip.Cast(player, PlayerLevel, null, projectileSource, spawnPosition, velocity);
                nextCantripDelay = CurrentCantrip.GetUseDelay(PlayerLevel);
            }
        }
    }
}