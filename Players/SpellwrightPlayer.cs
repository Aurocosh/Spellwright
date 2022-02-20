using Microsoft.Xna.Framework;
using Spellwright.Spells;
using Spellwright.Spells.SpellExtraData;
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

        public override void SaveData(TagCompound tag)
        {
            tag.Add("PlayerLevel", PlayerLevel);
            tag.Add("GuaranteedUsesLeft", GuaranteedUsesLeft);

            if (CurrentSpell != null && SpellData != null)
            {
                tag.Add("CurrentSpell", CurrentSpell.InternalName ?? "");
                tag.Add("CurrentSpellData", CurrentSpell.SerializeData(SpellData));
            }
            if (CurrentCantrip != null && CantripData != null)
            {
                tag.Add("CurrentCantrip", CurrentCantrip.InternalName ?? "");
                tag.Add("CurrentCantripData", CurrentCantrip.SerializeData(CantripData));
            }
        }

        public override void LoadData(TagCompound tag)
        {
            PlayerLevel = tag.GetInt("PlayerLevel");
            GuaranteedUsesLeft = tag.GetInt("GuaranteedUsesLeft");

            SpellLibrary spellLibrary = Spellwright.instance.spellLibrary;

            string spellName = tag.GetString("CurrentSpell");
            CurrentSpell = spellLibrary.GetSpellByName(spellName);
            if (CurrentSpell != null)
            {
                TagCompound spellDataTag = tag.GetCompound("CurrentSpellData");
                SpellData = CurrentSpell.DeserializeData(spellDataTag);
            }

            string cantripName = tag.GetString("CurrentCantrip");
            CurrentCantrip = spellLibrary.GetSpellByName(cantripName);
            if (CurrentSpell != null)
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
            {
                Spellwright.instance.spellInput.Show();
            }
            else if (Spellwright.CastCantripHotKey.Current && nextCantripDelay == 0 && CurrentCantrip != null && CantripData != null)
            {
                Player player = Main.LocalPlayer;

                Vector2 mousePosition = Main.MouseWorld;
                Vector2 spawnPosition = player.position;
                spawnPosition.Y += player.height / 2f;

                Vector2 velocity = mousePosition - spawnPosition;

                var projectileSource = new ProjectileSource_Item(player, null);

                if (CurrentCantrip.ConsumeReagents(player, playerLevel, CantripData))
                {
                    CurrentCantrip.Cast(player, PlayerLevel, CantripData, projectileSource, spawnPosition, velocity);
                    nextCantripDelay = CurrentCantrip.GetUseDelay(PlayerLevel);
                }
            }
        }
    }
}