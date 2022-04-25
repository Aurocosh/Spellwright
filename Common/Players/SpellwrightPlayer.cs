using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using Spellwright.Network;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool CanCastSpells = false;

        public ModSpell CurrentCantrip = null;
        public SpellData CantripData = null;

        public Point LastDeathPoint = Point.Zero;
        public Point VoidMarkPoint = Point.Zero;

        public readonly HashSet<int> KnownSpells = new();
        public readonly HashSet<int> UnlockedSpells = new();

        public override bool CloneNewInstances => false;

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
            Spellwright.Instance.userInterface.SetState(null);
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
            tag.Add("CanCastSpells", CanCastSpells);
            tag.Add("PlayerLevel", PlayerLevel);
            tag.Add("LastDeathPointX", LastDeathPoint.X);
            tag.Add("LastDeathPointY", LastDeathPoint.Y);
            tag.Add("VoidMarkPointX", VoidMarkPoint.X);
            tag.Add("VoidMarkPointY", VoidMarkPoint.Y);

            if (CurrentCantrip != null && CantripData != null)
            {
                tag.Add("CurrentCantrip", CurrentCantrip.Name ?? "");
                tag.Add("CurrentCantripData", CurrentCantrip.SerializeData(CantripData));
            }

            var ksTags = SerializeSpellIds(KnownSpells).ToList();
            tag.Add("KnownSpells", ksTags);
            var usTags = SerializeSpellIds(UnlockedSpells).ToList();
            tag.Add("UnlockedSpells", usTags);
        }

        public override void LoadData(TagCompound tag)
        {
            CanCastSpells = tag.GetBool("CanCastSpells");
            PlayerLevel = tag.GetInt("PlayerLevel");
            LastDeathPoint.X = tag.GetInt("LastDeathPointX");
            LastDeathPoint.Y = tag.GetInt("LastDeathPointY");
            VoidMarkPoint.X = tag.GetInt("VoidMarkPointX");
            VoidMarkPoint.Y = tag.GetInt("VoidMarkPointY");

            string cantripName = tag.GetString("CurrentCantrip");
            if (ModContent.TryFind(Spellwright.Instance.Name, cantripName, out CurrentCantrip))
            {
                TagCompound spellDataTag = tag.GetCompound("CurrentCantripData");
                CantripData = CurrentCantrip.DeserializeData(spellDataTag);
            }

            var knownSpellsNames = tag.GetList<TagCompound>("KnownSpells");
            KnownSpells.Clear();
            KnownSpells.UnionWith(DeserializeSpellIds(knownSpellsNames));


            var unlockedSpellNames = tag.GetList<TagCompound>("UnlockedSpells");
            UnlockedSpells.Clear();
            UnlockedSpells.UnionWith(DeserializeSpellIds(unlockedSpellNames));
        }

        private static IEnumerable<TagCompound> SerializeSpellIds(IEnumerable<int> spellIds)
        {
            foreach (var spellId in spellIds)
            {
                ModSpell modSpell = SpellLibrary.GetSpellById(spellId);
                if (modSpell != null)
                {
                    yield return new TagCompound
                    {
                        ["mod"] = modSpell.Mod.Name,
                        ["name"] = modSpell.Name
                    };
                }
            }
        }

        private static IEnumerable<int> DeserializeSpellIds(IEnumerable<TagCompound> spellTags)
        {
            foreach (var spellTag in spellTags)
            {
                var modName = spellTag.GetString("mod");
                string spellName = spellTag.GetString("name");
                if (ModContent.TryFind(modName, spellName, out ModSpell spell))
                    yield return spell.Type;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //if (Player.active && !Player.dead)
            {
                var spellPlayer = Player.GetModPlayer<SpellwrightPlayer>();
                if (spellPlayer.CanCastSpells)
                {
                    bool canAutoReuseCantrip = false;
                    if (CurrentCantrip != null)
                        canAutoReuseCantrip = CurrentCantrip.CanAutoReuse(PlayerLevel);

                    if (nextCantripDelay == 1 && !canAutoReuseCantrip)
                    {
                        var sound = SoundID.Item25.WithVolume(0.05f).WithPitchVariance(0.5f);
                        SoundEngine.PlaySound(sound, Player.Center);
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
                            Vector2 center = Player.Center;
                            Vector2 velocity = center.DirectionTo(mousePosition);
                            var projectileSource = new EntitySource_Parent(Player);
                            if (CurrentCantrip.ConsumeReagentsUse(Player, playerLevel, CantripData))
                                CurrentCantrip.Cast(Player, PlayerLevel, CantripData, projectileSource, center, velocity);

                            nextCantripDelay = CurrentCantrip.GetUseDelay(PlayerLevel);
                        }
                    }
                }
            }
        }
    }
}