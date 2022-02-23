using Microsoft.Xna.Framework;
using Spellwright.Data;
using Spellwright.DustSpawners;
using Spellwright.Network;
using Spellwright.Util;
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

        protected virtual void DoExtraActions(IEnumerable<Player> affectedPlayers, int playerLevel)
        {
            var spawner = new AoeCastDustSpawner
            {
                Caster = Main.LocalPlayer,
                AffectedPlayers = affectedPlayers.ToArray(),
                DustType = DustID.TreasureSparkle,
                EffectRadius = (byte)range,
                RingDustCount = 60,
                EffectDustCount = 14
            };

            spawner.Spawn();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.aoeCastDustHandler.Send(spawner);
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
                player.AddBuff(effect.effectId, duration);
            }
            var affectedPlayers = UtilList.Singleton(player);
            DoExtraActions(affectedPlayers, playerLevel);

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
            var affectedPlayers = Main.player.Where(IsAffected);

            var buffDatas = new BuffData[effects.Count];
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                int buffId = effect.effectId;
                int duration = effect.durationGetter.Invoke(playerLevel);
                buffDatas[i] = new BuffData(buffId, duration);
            }

            int myPlayer = player.whoAmI;
            foreach (var affectedPlayer in affectedPlayers)
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
            }
            DoExtraActions(affectedPlayers, playerLevel);

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
