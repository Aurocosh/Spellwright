using Spellwright.Content.Spells.Base.Description;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Data;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

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
            var buffIds = effects.Select(x => x.effectId).ToArray();
            if (spellData.HasModifier(SpellModifier.IsDispel))
            {
                UtilBuff.RemovePermamentEffect(affectedPlayers, buffIds);
            }
            else if (spellData.HasModifier(SpellModifier.IsEternal))
            {
                UtilBuff.SetBuffLevel(affectedPlayers, playerLevel, buffIds);
                UtilBuff.AddPermamentEffect(affectedPlayers, buffIds);
            }
            else
            {
                UtilBuff.SetBuffLevel(affectedPlayers, playerLevel, buffIds);

                var buffDatas = new BuffData[effects.Count];
                for (int i = 0; i < effects.Count; i++)
                {
                    var effect = effects[i];
                    int buffId = effect.effectId;
                    int duration = effect.durationGetter.Invoke(playerLevel);
                    buffDatas[i] = new BuffData(buffId, duration);
                }
                UtilBuff.AddBuffs(affectedPlayers, buffDatas);
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
