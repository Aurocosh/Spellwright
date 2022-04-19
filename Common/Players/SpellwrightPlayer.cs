using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using Spellwright.Network;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    internal class SpellwrightPlayer : ModPlayer, ILoader
    {
        private int playerLevel = 0;
        private int nextCantripDelay = 0;

        public ModSpell CurrentCantrip = null;
        public SpellData CantripData = null;

        public Point LastDeathPoint = Point.Zero;

        public readonly HashSet<int> UnlockedSpells = new();

        public static SpellwrightPlayer Instance => Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();

        public int PlayerLevel
        {
            get => playerLevel;
            set => playerLevel = Math.Clamp(value, 0, 10);
        }

        public override void Initialize()
        {
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            LastDeathPoint = Player.position.ToGridPoint();
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
            tag.Add("LastDeathPointX", LastDeathPoint.X);
            tag.Add("LastDeathPointY", LastDeathPoint.Y);

            if (CurrentCantrip != null && CantripData != null)
            {
                tag.Add("CurrentCantrip", CurrentCantrip.Name ?? "");
                tag.Add("CurrentCantripData", CurrentCantrip.SerializeData(CantripData));
            }

            var unlockedSpellNames = new List<TagCompound>();
            foreach (var id in UnlockedSpells)
            {
                ModSpell modSpell = SpellLibrary.GetSpellById(id);
                if (modSpell != null)
                {
                    var spellTag = new TagCompound
                    {
                        ["mod"] = modSpell.Mod.Name,
                        ["name"] = modSpell.Name
                    };
                    unlockedSpellNames.Add(spellTag);
                }
            }

            tag.Add("UnlockedSpells", unlockedSpellNames);
        }

        public override void LoadData(TagCompound tag)
        {
            PlayerLevel = tag.GetInt("PlayerLevel");
            LastDeathPoint.X = tag.GetInt("LastDeathPointX");
            LastDeathPoint.Y = tag.GetInt("LastDeathPointY");

            string cantripName = tag.GetString("CurrentCantrip");
            if (ModContent.TryFind(Spellwright.Instance.Name, cantripName, out CurrentCantrip))
            {
                TagCompound spellDataTag = tag.GetCompound("CurrentCantripData");
                CantripData = CurrentCantrip.DeserializeData(spellDataTag);
            }

            var unlockedSpellNames = tag.GetList<TagCompound>("UnlockedSpells");
            foreach (var spellTag in unlockedSpellNames)
            {
                var modName = spellTag.GetString("mod");
                string spellName = spellTag.GetString("name");
                if (ModContent.TryFind(modName, spellName, out ModSpell spell))
                    UnlockedSpells.Add(spell.Type);
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