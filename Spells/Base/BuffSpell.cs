using System;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Spells.Base
{
    internal abstract class BuffSpell : Spell
    {
        private readonly List<BuffSpellEffect> effects;

        public BuffSpell(string name, string incantation, SpellType spellType = SpellType.Invocation) : base(name, incantation, spellType)
        {
            effects = new List<BuffSpellEffect>();
        }

        protected void AddEffect(int effectId, Func<int, int> durationGetter)
        {
            effects.Add(new BuffSpellEffect(effectId, durationGetter));
        }
        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            foreach (BuffSpellEffect effect in effects)
            {
                int duration = effect.durationGetter.Invoke(playerLevel);
                player.AddBuff(effect.effectId, duration);
            }

            return true;
        }

        private struct BuffSpellEffect
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
