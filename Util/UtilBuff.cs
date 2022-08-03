using Spellwright.Common.Players;
using Spellwright.Data;
using Spellwright.Network.RoutedHandlers.Buffs;
using Spellwright.Network.Sync.EffectLevels;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Spellwright.Common.Players.SpellwrightBuffPlayer;

namespace Spellwright.Util
{
    internal class UtilBuff
    {
        public static bool IsModBuff(int type) => type >= BuffID.Count;

        public static TagCompound SerializeBuff(int buffId)
        {
            if (IsModBuff(buffId))
            {
                var modBuff = BuffLoader.GetBuff(buffId);
                return new TagCompound
                {
                    ["mod"] = modBuff.Mod.Name,
                    ["name"] = modBuff.Name
                };
            }
            else
            {
                return new TagCompound
                {
                    ["mod"] = "Terraria",
                    ["id"] = buffId
                };
            }
        }

        public static int DeserializeBuff(TagCompound tag)
        {
            var modName = tag.GetString("mod");
            return modName == "Terraria" ? tag.GetInt("id") : ModContent.TryFind(modName, tag.GetString("name"), out ModBuff buff) ? buff.Type : 0;
        }

        public static void SetBuffLevel(IEnumerable<Player> affectedPlayers, int playerLevel, int[] buffIds)
        {
            var buffLevels = new List<BuffLevelData>();
            foreach (var buffId in buffIds)
                buffLevels.Add(new BuffLevelData(buffId, playerLevel));

            if (buffLevels.Count == 0)
                return;

            foreach (var affectedPlayer in affectedPlayers)
            {
                var effectPlayer = affectedPlayer.GetModPlayer<SpellwrightBuffPlayer>();
                effectPlayer.SetBuffLevels(buffLevels);
                new EffectLevelSyncNetwork(affectedPlayer.whoAmI, buffLevels).Execute();
            }
        }

        public static void AddBuffs(IEnumerable<Player> affectedPlayers, BuffData[] buffDatas)
        {
            foreach (var affectedPlayer in affectedPlayers)
                new PlayerAddBuffsAction(affectedPlayer.whoAmI, buffDatas).Execute();
        }
    }
}
