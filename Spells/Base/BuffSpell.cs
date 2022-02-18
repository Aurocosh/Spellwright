using Microsoft.Xna.Framework;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.Spells.Base
{
    internal abstract class BuffSpell : Spell
    {
        protected int range = 10;
        protected readonly List<BuffSpellEffect> effects;
        protected virtual int GetRange(Player player) => range;
        protected virtual bool CanApplyToPlayer(Player player) => true;

        public BuffSpell(string name, string incantation, SpellType spellType = SpellType.Invocation) : base(name, incantation, spellType)
        {
            effects = new List<BuffSpellEffect>();
            AddApplicableModifier(SpellModifier.IsAoe);
        }
        protected virtual void DoExtraActions(Player player, int playerLevel)
        {
            Vector2 position = player.Center;
            for (int i = 0; i < 7; i++)
            {
                Vector2 dustPosition = UtilVector2.GetPointOnRing(position, 10, 40);
                Vector2 velocity = UtilVector2.RandomVector(position, dustPosition, .1f, 1.5f);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, DustID.TreasureSparkle, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }

            for (int i = 0; i < 7; i++)
            {
                Vector2 dustPosition = UtilVector2.GetPointOnRing(position, 10, 40);
                Vector2 velocity = UtilVector2.RandomVector(.1f, .5f);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, DustID.TreasureSparkle, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }

        protected void AddEffect(int effectId, Func<int, int> durationGetter)
        {
            effects.Add(new BuffSpellEffect(effectId, durationGetter));
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData.HasModifier(SpellModifier.IsAoe))
                return AoeCast(player, playerLevel);
            else
                return PersonalCast(player, playerLevel);
        }

        private bool PersonalCast(Player player, int playerLevel)
        {
            foreach (BuffSpellEffect effect in effects)
            {
                int duration = effect.durationGetter.Invoke(playerLevel);
                DoExtraActions(player, playerLevel);
                player.AddBuff(effect.effectId, duration);
            }

            return true;
        }

        private bool AoeCast(Player player, int playerLevel)
        {
            int aoeRange = GetRange(player) * 16;
            Vector2 castPosition = player.position;
            int radiusSquared = aoeRange * aoeRange;

            bool IsAffected(Player otherPlayer)
            {
                if (otherPlayer == null)
                    return false;
                if (!otherPlayer.active)
                    return false;
                if (!CanApplyToPlayer(otherPlayer))
                    return false;

                Vector2 distanceVector = player.position - castPosition;
                float distanceSquared = distanceVector.LengthSquared();

                return distanceSquared <= radiusSquared;
            }
            var playersAffected = Main.player.Where(IsAffected);

            var effectValues = new List<Tuple<int, int>>(effects.Count);
            foreach (var effect in effects)
            {
                int buffId = effect.effectId;
                int duration = effect.durationGetter.Invoke(playerLevel);
                effectValues.Add(new Tuple<int, int>(buffId, duration));
            }

            foreach (var affectedPlayer in playersAffected)
            {
                foreach (var effectValue in effectValues)
                {
                    int buffId = effectValue.Item1;
                    int duration = effectValue.Item2;

                    DoExtraActions(affectedPlayer, playerLevel);
                    affectedPlayer.AddBuff(buffId, duration);
                }
            }

            return true;
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
