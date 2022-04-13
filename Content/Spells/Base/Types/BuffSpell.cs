using Spellwright.Common.Players;
using Spellwright.Data;
using Spellwright.Network;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using static Spellwright.Common.Players.SpellwrightBuffPlayer;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class BuffSpell : PlayerAoeSpell
    {
        protected readonly List<BuffSpellEffect> effects;

        public BuffSpell()
        {
            UseType = SpellType.Invocation;
            effects = new List<BuffSpellEffect>();
        }

        protected void AddEffect(int effectId, Func<int, int> durationGetter)
        {
            effects.Add(new BuffSpellEffect(effectId, durationGetter));
        }

        protected override void ApplyEffect(IEnumerable<Player> affectedPlayers, int playerLevel, SpellData spellData)
        {
            if (spellData.HasModifier(SpellModifier.IsDispel))
            {
                RemovePermamentEffect(affectedPlayers);
            }
            else if (spellData.HasModifier(SpellModifier.IsEternal))
            {
                SetBuffLevel(affectedPlayers, playerLevel);
                AddPermamentEffect(affectedPlayers);
            }
            else
            {
                SetBuffLevel(affectedPlayers, playerLevel);
                AddBuffs(affectedPlayers, playerLevel);
            }
        }
        private void RemovePermamentEffect(IEnumerable<Player> affectedPlayers)
        {
            var buffIds = effects.Select(x => x.effectId).ToArray();

            int myPlayer = Main.myPlayer;
            foreach (var affectedPlayer in affectedPlayers)
            {
                int playerId = affectedPlayer.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != myPlayer)
                {
                    ModNetHandler.OtherPlayerRemovePermamentEffectHandler.Send(playerId, myPlayer, buffIds);
                }
                else
                {
                    var modPlayer = affectedPlayer.GetModPlayer<SpellwrightBuffPlayer>();
                    foreach (var buffId in buffIds)
                        modPlayer.PermamentBuffs.Remove(buffId);
                }
            }
        }
        private void AddPermamentEffect(IEnumerable<Player> affectedPlayers)
        {
            var buffIds = effects.Select(x => x.effectId).ToArray();

            int myPlayer = Main.myPlayer;
            foreach (var affectedPlayer in affectedPlayers)
            {
                int playerId = affectedPlayer.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != myPlayer)
                {
                    ModNetHandler.otherPlayerAddPermamentEffectHandler.Send(playerId, myPlayer, buffIds);
                }
                else
                {
                    var modPlayer = affectedPlayer.GetModPlayer<SpellwrightBuffPlayer>();
                    foreach (var buffId in buffIds)
                        modPlayer.PermamentBuffs.Add(buffId);
                }
            }
        }

        private void SetBuffLevel(IEnumerable<Player> affectedPlayers, int playerLevel)
        {
            var buffLevels = new List<BuffLevelData>();
            foreach (var effect in effects)
                buffLevels.Add(new BuffLevelData(effect.effectId, playerLevel));

            if (buffLevels.Count == 0)
                return;

            int myPlayer = Main.myPlayer;
            foreach (var affectedPlayer in affectedPlayers)
            {
                var effectPlayer = affectedPlayer.GetModPlayer<SpellwrightBuffPlayer>();
                effectPlayer.SetBuffLevels(buffLevels);
                int playerId = affectedPlayer.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != myPlayer)
                    ModNetHandler.EffectLevelHandler.Sync(playerId, buffLevels);
            }
        }

        private void AddBuffs(IEnumerable<Player> affectedPlayers, int playerLevel)
        {
            var buffDatas = new BuffData[effects.Count];
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                int buffId = effect.effectId;
                int duration = effect.durationGetter.Invoke(playerLevel);
                buffDatas[i] = new BuffData(buffId, duration);
            }

            int myPlayer = Main.myPlayer;
            foreach (var affectedPlayer in affectedPlayers)
            {
                int playerId = affectedPlayer.whoAmI;
                //if (playerId == myPlayer)
                //    continue;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != myPlayer)
                {
                    ModNetHandler.otherPlayerAddBuffsHandler.Send(playerId, myPlayer, buffDatas);
                }
                else
                {
                    foreach (var buffData in buffDatas)
                        affectedPlayer.AddBuff(buffData.Type, buffData.Duration);
                }
            }
        }

        public override List<SpellParameter> GetDescriptionValues(Player player, int playerLevel, SpellData spellData, bool fullVersion)
        {
            var values = base.GetDescriptionValues(player, playerLevel, spellData, fullVersion);

            var effectDescriptions = new List<string>();
            foreach (var effect in effects)
            {
                var buffName = Lang.GetBuffName(effect.effectId);
                var duration = UtilTime.TicksToString(effect.durationGetter.Invoke(playerLevel));

                effectDescriptions.Add($"{buffName}({duration})");
            }
            var effectList = string.Join(", ", effectDescriptions);
            values.Add(new SpellParameter("AddedBuffs", effectList));

            return values;
        }

        protected struct BuffSpellEffect
        {
            public readonly int effectId;
            public readonly Func<int, int> durationGetter;

            public BuffSpellEffect(int effectId, Func<int, int> durationGetter)
            {
                this.effectId = effectId;
                this.durationGetter = durationGetter;
            }
        }
    }
}
