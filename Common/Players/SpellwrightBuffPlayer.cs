using Microsoft.Xna.Framework.Input;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Core.Buffs;
using Spellwright.Data;
using Spellwright.Network.Sync;
using Spellwright.Network.Sync.EffectLevels;
using Spellwright.Util;
using System;
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
        public int StateLockCount = 0;
        public bool CycleOfEternity = false;

        private readonly List<BuffData> respawnBuffs = new();
        public readonly HashSet<int> PermanentBuffs = new();
        public readonly Dictionary<int, int> BuffLevels = new();

        public bool HasPermanentBuff(int buffId) => PermanentBuffs.Contains(buffId);

        protected override bool CloneNewInstances => false;

        public void SetPermanentBuffs(IEnumerable<int> buffIds)
        {
            PermanentBuffs.Clear();
            PermanentBuffs.UnionWith(buffIds);
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

        public override void OnRespawn()
        {
            foreach (var buffData in respawnBuffs)
                Player.AddBuff(buffData.Type, buffData.Duration);
            respawnBuffs.Clear();
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (StateLockCount > 0)
            {
                StateLockCount--;
                if (StateLockCount == 0)
                {
                    var message = Spellwright.GetTranslation("Spells", "StateLockSpell", "Unstable");
                    Main.NewText(message);
                }

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
            else if (CycleOfEternity)
            {
                int cost = 2 * PermanentBuffs.Count;
                var buffSaveCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), cost);
                if (!buffSaveCost.Consume(Player, 0, SpellData.EmptyData))
                {
                    PermanentBuffs.Clear();
                    CycleOfEternity = false;
                    Main.NewText(Spellwright.GetTranslation("Spells", "CycleOfEternitySpell", "Disconnected"));
                }
            }
            else
            {
                PermanentBuffs.Clear();
            }
            return true;
        }

        public override void PreUpdateBuffs()
        {
        }

        public override void PostUpdateBuffs()
        {
            foreach (var buffId in PermanentBuffs)
            {
                Player.buffImmune[buffId] = true;
                BuffHandler.UpdateBuff(buffId, Player);
            }
        }


        public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
        {
            var clone = clientClone as SpellwrightBuffPlayer;
            clone.SetPermanentBuffs(PermanentBuffs);

            clone.BuffLevels.Clear();
            foreach (var effectLevel in BuffLevels)
                clone.BuffLevels.Add(effectLevel.Key, effectLevel.Value);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            int playerId = Player.whoAmI;
            var effectIds = PermanentBuffs.Cast<int>().ToArray();

            new PermanentPlayerEffectsSyncAction(playerId, toWho, effectIds).Execute();

            var levelData = new List<BuffLevelData>();
            foreach (var effectLevel in BuffLevels)
                levelData.Add(new BuffLevelData(effectLevel.Key, effectLevel.Value));
            new EffectLevelSyncNetwork(playerId, toWho, levelData).Execute();
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightBuffPlayer;
            if (!clone.PermanentBuffs.SetEquals(PermanentBuffs))
            {
                var effectIds = PermanentBuffs.Cast<int>().ToArray();
                new PermanentPlayerEffectsSyncAction(Player.whoAmI, effectIds).Execute();
            }

            foreach (var effectLevel in BuffLevels)
            {
                int buffId = effectLevel.Key;
                int level = effectLevel.Value;
                if (!clone.BuffLevels.TryGetValue(buffId, out int cloneLevel) || level != cloneLevel)
                    new SingleEffectLevelSyncNetwork(Player.whoAmI, new BuffLevelData(buffId, level)).Execute();
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("StateLockCount", StateLockCount);
            tag.Add("CycleOfEternity", CycleOfEternity);

            var permanentEffectTags = new List<TagCompound>();
            foreach (var buffId in PermanentBuffs)
                permanentEffectTags.Add(UtilBuff.SerializeBuff(buffId));
            tag.Add("PermanentBuffs", permanentEffectTags);

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
            StateLockCount = tag.GetInt("StateLockCount");
            CycleOfEternity = tag.GetBool("CycleOfEternity");

            PermanentBuffs.Clear();
            var permanentEffectTags = tag.GetList<TagCompound>("PermanentBuffs");
            foreach (var elementTag in permanentEffectTags)
            {
                int buffId = UtilBuff.DeserializeBuff(elementTag);
                if (buffId > 0)
                    PermanentBuffs.Add(buffId);
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


        [Serializable]
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
