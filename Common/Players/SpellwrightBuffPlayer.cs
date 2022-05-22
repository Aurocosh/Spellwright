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
        public int StateLockCount = 0;

        private readonly List<BuffData> respawnBuffs = new();
        public readonly HashSet<int> PermanentBuffs = new();
        public readonly Dictionary<int, int> BuffLevels = new();

        public bool HasPermanentBuff(int buffId) => PermanentBuffs.Contains(buffId);

        public override bool CloneNewInstances => false;

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

        public override void OnRespawn(Player player)
        {
            foreach (var buffData in respawnBuffs)
                player.AddBuff(buffData.Type, buffData.Duration);
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
            else
            {
                int cost = 3 * PermanentBuffs.Count;
                var buffSaveCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), cost);
                if (!buffSaveCost.Consume(Player, 0, SpellData.EmptyData))
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


        public override void clientClone(ModPlayer clientClone)
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
            ModNetHandler.permanentPlayerEffectsHandler.Sync(toWho, playerId, playerId, effectIds);

            var levelData = new List<BuffLevelData>();
            foreach (var effectLevel in BuffLevels)
                levelData.Add(new BuffLevelData(effectLevel.Key, effectLevel.Value));
            ModNetHandler.EffectLevelHandler.Sync(toWho, playerId, playerId, levelData);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightBuffPlayer;
            if (!clone.PermanentBuffs.SetEquals(PermanentBuffs))
            {
                var effectIds = PermanentBuffs.Cast<int>().ToArray();
                ModNetHandler.permanentPlayerEffectsHandler.Sync(Player.whoAmI, effectIds);
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
            tag.Add("StateLockCount", StateLockCount);

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
