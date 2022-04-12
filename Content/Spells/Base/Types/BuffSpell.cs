using Spellwright.Data;
using Spellwright.Network;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

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
                string playerName = affectedPlayer.name;

                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != myPlayer)
                {
                    ModNetHandler.OtherPlayerAddBuffsHandler.Send(playerId, myPlayer, buffDatas);
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
