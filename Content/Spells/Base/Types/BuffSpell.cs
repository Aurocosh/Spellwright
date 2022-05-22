using Spellwright.Content.Spells.Base.Description;
using Spellwright.Content.Spells.Base.Modifiers;
using Spellwright.Data;
using Spellwright.Extensions;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class BuffSpell : PlayerAoeSpell
    {
        protected readonly HashSet<int> buffIds;
        protected readonly List<BuffSpellEffect> effects;

        public BuffSpell()
        {
            UseType = SpellType.Invocation;
            buffIds = new HashSet<int>();
            effects = new List<BuffSpellEffect>();
        }

        public bool HasBuff(int buffId)
        {
            return buffIds.Contains(buffId);
        }

        public IReadOnlyCollection<int> BuffIds => buffIds;

        protected void AddEffect(int buffId, Func<int, int> durationGetter)
        {
            buffIds.Add(buffId);
            effects.Add(new BuffSpellEffect(buffId, durationGetter));
        }

        protected override void ApplyEffect(IEnumerable<Player> affectedPlayers, int playerLevel, SpellData spellData)
        {
            var buffIds = effects.Select(x => x.effectId).ToArray();
            if (spellData.HasModifier(SpellModifier.Dispel))
            {
                UtilBuff.RemovePermanentEffect(affectedPlayers, buffIds);
            }
            else if (spellData.HasModifier(SpellModifier.Eternal))
            {
                UtilBuff.SetBuffLevel(affectedPlayers, playerLevel, buffIds);
                UtilBuff.AddPermanentEffect(affectedPlayers, buffIds);
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

            var effectDescriptions = new StringBuilder();
            foreach (var effect in effects)
            {
                var buffName = Lang.GetBuffName(effect.effectId);
                var duration = UtilTime.TicksToString(effect.durationGetter.Invoke(playerLevel));

                effectDescriptions.AppendDelimited(", ", $"{buffName} ({duration})");
            }
            var effectList = effectDescriptions.ToString();
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
