using Microsoft.Xna.Framework;
using Spellwright.DustSpawners;
using Spellwright.Network;
using Spellwright.Util;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class PlayerAoeSpell : ModSpell
    {
        protected int range = 10;
        protected virtual int GetRange(int playerLevel) => range;
        protected virtual bool CanApplyToPlayer(Player player) => true;

        public PlayerAoeSpell()
        {
            UseType = SpellType.Invocation;
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

        protected abstract void ApplyEffect(IEnumerable<Player> affectedPlayers, int playerLevel, SpellData spellData);

        public sealed override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData.HasModifier(SpellModifier.IsAoe))
                return AoeCast(player, playerLevel, spellData);
            else
                return PersonalCast(player, playerLevel, spellData);
        }

        public sealed override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            if (spellData.HasModifier(SpellModifier.IsAoe))
                return AoeCast(player, playerLevel, spellData);
            else
                return PersonalCast(player, playerLevel, spellData);
        }

        private bool PersonalCast(Player player, int playerLevel, SpellData spellData)
        {
            var affectedPlayers = UtilList.Singleton(player);
            ApplyEffect(affectedPlayers, playerLevel, spellData);
            DoExtraActions(affectedPlayers, playerLevel);
            return true;
        }

        private bool AoeCast(Player player, int playerLevel, SpellData spellData)
        {
            int aoeRange = GetRange(playerLevel) * 16;
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
            ApplyEffect(affectedPlayers, playerLevel, spellData);
            DoExtraActions(affectedPlayers, playerLevel);
            return true;
        }

        public override List<SpellParameter> GetDescriptionValues(Player player, int playerLevel, SpellData spellData, bool fullVersion)
        {
            var values = base.GetDescriptionValues(player, playerLevel, spellData, fullVersion);
            int aoeRange = GetRange(playerLevel) * 16;
            values.Add(new SpellParameter("AoeRange", aoeRange.ToString()));
            return values;
        }
    }
}
