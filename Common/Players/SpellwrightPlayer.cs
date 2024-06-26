﻿using Microsoft.Xna.Framework;
using Spellwright.Common.System;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.BuffSpells;
using Spellwright.Core.Spells;
using Spellwright.Extensions;
using Spellwright.Network.Sync;
using Spellwright.UI.States;
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
using Terraria.UI;

namespace Spellwright.Common.Players
{
    public class SpellwrightPlayer : ModPlayer //, ILoader
    {
        private const int dataVersion = 1;

        private int playerLevel = 0;
        private int nextCantripDelay = 0;

        public bool LearnedBasics = false;
        public bool CanCastSpells = false;

        public ModSpell CurrentCantrip = null;
        public SpellData CantripData = null;

        public Point LastDeathPoint = Point.Zero;
        public Point VoidMarkPoint = Point.Zero;

        public readonly HashSet<int> FavoriteSpells = new();
        public readonly HashSet<int> KnownSpells = new();
        public readonly HashSet<int> UnlockedSpells = new();

        protected override bool CloneNewInstances => false;

        public static SpellwrightPlayer Instance => Main.LocalPlayer.GetModPlayer<SpellwrightPlayer>();

        public int PlayerLevel
        {
            get => playerLevel;
            set => playerLevel = Math.Clamp(value, 0, 10);
        }

        public override void Initialize()
        {
        }

        public void SetCantrip(ModSpell cantrip, SpellData spellData)
        {
            CurrentCantrip = cantrip;
            CantripData = spellData;
            nextCantripDelay = 0;
        }

        public bool IsSpellUnlocked(ModSpell spell)
        {
            return spell.UnlockCost == null || UnlockedSpells.Contains(spell.Type);
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            LastDeathPoint = Player.position.ToGridPoint();
            Spellwright.Instance.userInterface.SetState(null);
        }

        public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
        {
            var clone = clientClone as SpellwrightPlayer;
            clone.PlayerLevel = PlayerLevel;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            int playerId = Player.whoAmI;
            new PlayerLevelSyncAction(playerId, toWho, PlayerLevel).Execute();
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightPlayer;
            if (clone.PlayerLevel != PlayerLevel)
                new PlayerLevelSyncAction(Player.whoAmI, PlayerLevel).Execute();
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("DataVersion", dataVersion);
            tag.Add("LearnedBasics", LearnedBasics);
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

            var fvTags = SerializeSpellIds(FavoriteSpells).ToList();
            tag.Add("FavoriteSpells", fvTags);
            var ksTags = SerializeSpellIds(KnownSpells).ToList();
            tag.Add("KnownSpells", ksTags);
            var usTags = SerializeSpellIds(UnlockedSpells).ToList();
            tag.Add("UnlockedSpells", usTags);
        }

        public override void LoadData(TagCompound tag)
        {
            int oldDataVersion = tag.GetInt("DataVersion");
            LearnedBasics = tag.GetBool("LearnedBasics");
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

            var favSpellsNames = tag.GetList<TagCompound>("FavoriteSpells");
            FavoriteSpells.Clear();
            FavoriteSpells.UnionWith(DeserializeSpellIds(favSpellsNames));

            var knownSpellsNames = tag.GetList<TagCompound>("KnownSpells");
            KnownSpells.Clear();
            KnownSpells.UnionWith(DeserializeSpellIds(knownSpellsNames));


            var unlockedSpellNames = tag.GetList<TagCompound>("UnlockedSpells");
            UnlockedSpells.Clear();
            UnlockedSpells.UnionWith(DeserializeSpellIds(unlockedSpellNames));

            if (oldDataVersion < 1 && LearnedBasics && CanCastSpells && PlayerLevel >= 4)
            {
                // Add spell Cycle of Eternity to the learned spells. It is apart of Advanced Spell Tome, but higher level players will not be aware of its existance and unlikely to randomly use this tome.
                var spell = SpellLibrary.GetSpellByType<CycleOfEternitySpell>();
                if (spell != null)
                    KnownSpells.Add(spell.Type);
            }
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
            if (Player.active && !Player.dead)
            {
                var spellPlayer = Player.GetModPlayer<SpellwrightPlayer>();
                if (spellPlayer.CanCastSpells)
                {
                    bool canAutoReuseCantrip = false;
                    if (CurrentCantrip != null)
                        canAutoReuseCantrip = CurrentCantrip.CanAutoReuse(PlayerLevel);

                    if (nextCantripDelay == 1 && !canAutoReuseCantrip)
                    {
                        var sound = SoundID.Item25;
                        sound.Volume = 0.05f;
                        sound.PitchVariance = 0.5f;

                        SoundEngine.PlaySound(sound, Player.Center);
                    }

                    if (nextCantripDelay > 0)
                        nextCantripDelay--;

                    UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
                    UserInterface spellInterface = Spellwright.Instance.userInterface;

                    if (PlayerInput.Triggers.JustReleased.Inventory && spellInterface.CurrentState == uiMessageState)
                    {
                        spellInterface.SetState(null);
                    }
                    else if (KeybindSystem.OpenIncantationUIHotKey.JustPressed)
                    {
                        if (spellInterface.CurrentState == uiMessageState)
                        {
                            spellInterface.SetState(null);
                        }
                        else if (PlayerInput.Triggers.Current.SmartSelect)
                        {
                            if (!uiMessageState.HasText())
                                uiMessageState.GoHome();

                            if (spellInterface.CurrentState == uiMessageState)
                            {
                                spellInterface.SetState(null);
                            }
                            else
                            {
                                spellInterface.SetState(uiMessageState);
                                if (Main.playerInventory)
                                    Player.ToggleInv();
                                if (Main.mapFullscreen)
                                    Main.mapFullscreen = false;
                            }
                        }
                        else
                        {
                            Spellwright.Instance.userInterface.SetState(Spellwright.Instance.spellInputState);
                            if (Main.mapFullscreen)
                                Main.mapFullscreen = false;
                        }
                    }
                    else if (KeybindSystem.CastCantripHotKey.JustPressed || KeybindSystem.CastCantripHotKey.Current)
                    {
                        bool singleCasted = KeybindSystem.CastCantripHotKey.JustPressed && !canAutoReuseCantrip;
                        bool continuosCast = KeybindSystem.CastCantripHotKey.Current && canAutoReuseCantrip;

                        if ((singleCasted || continuosCast) && nextCantripDelay == 0 && CurrentCantrip != null && CantripData != null)
                        {
                            Vector2 mousePosition = Main.MouseWorld;
                            Vector2 center = Player.Center + new Vector2(Player.width * .2f * Player.direction, -Player.height * .4f);
                            Vector2 velocity = center.DirectionTo(mousePosition);
                            var projectileSource = new EntitySource_Parent(Player);
                            if (CurrentCantrip.ConsumeReagentsUse(Player, playerLevel, CantripData))
                            {
                                CurrentCantrip.PlayUseSound(Player.Center);
                                CurrentCantrip.Cast(Player, PlayerLevel, CantripData, projectileSource, center, velocity);
                            }

                            nextCantripDelay = CurrentCantrip.GetUseDelay(PlayerLevel);
                        }
                    }
                }
                else if (spellPlayer.LearnedBasics && KeybindSystem.OpenIncantationUIHotKey.JustPressed)
                {
                    UIMessageState uiMessageState = Spellwright.Instance.uiMessageState;
                    UserInterface spellInterface = Spellwright.Instance.userInterface;

                    if (!uiMessageState.HasText())
                        uiMessageState.GoHome();

                    if (spellInterface.CurrentState == uiMessageState)
                    {
                        spellInterface.SetState(null);
                    }
                    else
                    {
                        spellInterface.SetState(uiMessageState);
                        if (Main.playerInventory)
                            Player.ToggleInv();
                        if (Main.mapFullscreen)
                            Main.mapFullscreen = false;
                    }
                }
            }
        }
    }
}