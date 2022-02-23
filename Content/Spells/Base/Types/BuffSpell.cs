using Microsoft.Xna.Framework;
using Spellwright.Data;
using Spellwright.Extensions;
using Spellwright.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class BuffSpell : ModSpell
    {
        protected int range = 10;
        protected readonly List<BuffSpellEffect> effects;
        protected virtual int GetRange(Player player) => range;
        protected virtual bool CanApplyToPlayer(Player player) => true;

        public BuffSpell()
        {
            UseType = SpellType.Invocation;

            effects = new List<BuffSpellEffect>();
            AddApplicableModifier(SpellModifier.IsAoe);
        }

        protected virtual void DoExtraActions(Player player, int playerLevel)
        {
            Vector2 position = player.Center;
            for (int i = 0; i < 7; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2CircularEdge(1f, 1f).ScaleRandom(10, 40);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, 1.5f);

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, DustID.TreasureSparkle, 0f, 0f, 100, default, 2.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }

            for (int i = 0; i < 7; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2CircularEdge(1f, 1f).ScaleRandom(10, 40);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, .5f);

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
            Vector2 castPosition = player.Center;
            int radiusSquared = aoeRange * aoeRange;

            bool IsAffected(Player otherPlayer)
            {
                if (otherPlayer == null)
                    return false;
                if (!otherPlayer.active)
                    return false;
                if (!CanApplyToPlayer(otherPlayer))
                    return false;

                float distanceSquared = Vector2.DistanceSquared(otherPlayer.Center, castPosition);
                return distanceSquared <= radiusSquared;
            }
            var playersAffected = Main.player.Where(IsAffected);

            var buffDatas = new BuffData[effects.Count];
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                int buffId = effect.effectId;
                int duration = effect.durationGetter.Invoke(playerLevel);
                buffDatas[i] = new BuffData(buffId, duration);
            }

            int myPlayer = player.whoAmI;
            foreach (var affectedPlayer in playersAffected)
            {
                int playerId = affectedPlayer.whoAmI;
                string playerName = affectedPlayer.name;

                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != myPlayer)
                {
                    ModNetHandler.OtherPlayerBuffsHandler.Send(playerId, myPlayer, buffDatas);
                }
                else
                {
                    foreach (var buffData in buffDatas)
                        affectedPlayer.AddBuff(buffData.Type, buffData.Duration);
                }

                DoExtraActions(affectedPlayer, playerLevel);
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
