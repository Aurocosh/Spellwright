using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Core.Buffs;
using Spellwright.Data;
using Spellwright.Network;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Common.Players
{
    internal class SpellwrightBuffPlayer : ModPlayer
    {
        private readonly List<BuffData> respawnBuffs = new();
        public readonly HashSet<int> PermamentBuffs = new();
        public readonly Dictionary<int, int> BuffLevels = new();

        public bool HasPermamentBuff(int buffId) => PermamentBuffs.Contains(buffId);

        public override bool CloneNewInstances => false;

        public void SetPermamentBuffs(IEnumerable<int> buffIds)
        {
            PermamentBuffs.Clear();
            PermamentBuffs.UnionWith(buffIds);
        }

        public void SetBuffLevel(BuffLevelData buffLevel)
        {
            BuffLevels[buffLevel.BuffId] = buffLevel.Level;
        }

        public void SetBuffLevels(List<BuffLevelData> buffLevels)
        {
            foreach (var buffLevel in buffLevels)
                BuffLevels[buffLevel.BuffId] = buffLevel.Level;
        }

        public int GetBuffLevel(int buffId)
        {
            if (BuffLevels.TryGetValue(buffId, out var level))
                return level;
            return 0;
        }

        public override void OnRespawn(Player player)
        {
            foreach (var buffData in respawnBuffs)
                player.AddBuff(buffData.Type, buffData.Duration);
            respawnBuffs.Clear();
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player.HasBuff(ModContent.BuffType<StateLockBuff>()))
            {
                respawnBuffs.Clear();

                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    int buffType = Player.buffType[i];
                    int buffTime = Player.buffTime[i];
                    if (buffType > 0 && buffTime > 0)
                    {
                        if (!Main.debuff[buffType] && !Main.buffNoSave[buffType])
                        {
                            var buffData = new BuffData(buffType, buffTime);
                            respawnBuffs.Add(buffData);
                        }
                    }
                }
            }
            else
            {
                int cost = 3 * PermamentBuffs.Count;
                var buffSaveCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), cost);
                if (!buffSaveCost.Consume(Player, 0, SpellData.EmptyData))
                    PermamentBuffs.Clear();
            }
            return true;
        }

        public override void PreUpdateBuffs()
        {
        }

        public override void PostUpdateBuffs()
        {
            foreach (var buffId in PermamentBuffs)
            {
                Player.buffImmune[buffId] = true;
                BuffHandler.UpdateBuff(buffId, Player);
            }

            //if (IsPermament(BuffID.Shine))
            //    Lighting.AddLight((int)(Player.position.X + (float)(Player.width / 2)) / 16, (int)(Player.position.Y + (float)(Player.height / 2)) / 16, 0.8f, 0.95f, 1f);
            //if (IsPermament(BuffID.Spelunker))
            //    Player.findTreasure = true;
            //if (IsPermament(BuffID.Hunter))
            //    Player.detectCreature = true;
            //if (IsPermament(ModContent.BuffType<GaleForceBuff>()))
            //    GaleForceBuff.DoAction(Player);
            //if (IsPermament(ModContent.BuffType<ReturnToFishBuff>()))
            //    ReturnToFishBuff.DoAction(Player);
            //if (IsPermament(ModContent.BuffType<KissOfCloverBuff>()))
            //    KissOfCloverBuff.DoAction(Player);
        }


        public override void clientClone(ModPlayer clientClone)
        {
            var clone = clientClone as SpellwrightBuffPlayer;
            clone.SetPermamentBuffs(PermamentBuffs);

            clone.BuffLevels.Clear();
            foreach (var effectLevel in BuffLevels)
                clone.BuffLevels.Add(effectLevel.Key, effectLevel.Value);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            int playerId = Player.whoAmI;
            var effectIds = PermamentBuffs.Cast<int>().ToArray();
            ModNetHandler.permamentPlayerEffectsHandler.Sync(toWho, playerId, playerId, effectIds);

            var levelData = new List<BuffLevelData>();
            foreach (var effectLevel in BuffLevels)
                levelData.Add(new BuffLevelData(effectLevel.Key, effectLevel.Value));
            ModNetHandler.EffectLevelHandler.Sync(toWho, playerId, playerId, levelData);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightBuffPlayer;
            if (!clone.PermamentBuffs.SetEquals(PermamentBuffs))
            {
                var effectIds = PermamentBuffs.Cast<int>().ToArray();
                ModNetHandler.permamentPlayerEffectsHandler.Sync(Player.whoAmI, effectIds);
            }

            foreach (var effectLevel in BuffLevels)
            {
                int buffId = effectLevel.Key;
                int level = effectLevel.Value;
                if (!clone.BuffLevels.TryGetValue(buffId, out int cloneLevel) || level != cloneLevel)
                    ModNetHandler.SingleEffectLevelHandler.Sync(Player.whoAmI, new BuffLevelData(buffId, level));
            }
        }

        public override void SaveData(TagCompound tag)
        {
            var permamentEffectTags = new List<TagCompound>();
            foreach (var buffId in PermamentBuffs)
                permamentEffectTags.Add(UtilBuff.SerializeBuff(buffId));
            tag.Add("PermamentEffects", permamentEffectTags);

            var effectLevelTags = new List<TagCompound>();
            foreach (var effectLevel in BuffLevels)
            {
                int buffId = effectLevel.Key;
                int level = effectLevel.Value;

                var effectTag = UtilBuff.SerializeBuff(buffId);
                effectTag["Level"] = level;
                effectLevelTags.Add(effectTag);
            }
            tag.Add("EffectLevels", effectLevelTags);
        }

        public override void LoadData(TagCompound tag)
        {
            PermamentBuffs.Clear();
            var permamentEffectTags = tag.GetList<TagCompound>("PermamentEffects");
            foreach (var elementTag in permamentEffectTags)
            {
                int buffId = UtilBuff.DeserializeBuff(elementTag);
                if (buffId > 0)
                    PermamentBuffs.Add(buffId);
            }

            BuffLevels.Clear();
            var effectLevelTags = tag.GetList<TagCompound>("EffectLevels");
            foreach (var elementTag in effectLevelTags)
            {
                int buffId = UtilBuff.DeserializeBuff(elementTag);
                if (buffId > 0)
                {
                    int level = elementTag.GetInt("Level");
                    BuffLevels.Add(buffId, level);
                }
            }
        }


        internal class BuffLevelData
        {
            public int BuffId { get; set; }
            public int Level { get; set; }

            public BuffLevelData()
            {
                BuffId = 0;
                Level = 0;
            }

            public BuffLevelData(int buffId, int level)
            {
                BuffId = buffId;
                Level = level;
            }
        }
    }
}
