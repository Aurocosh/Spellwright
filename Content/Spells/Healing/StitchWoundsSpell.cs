using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellwright.Content.Spells.Healing
{
    internal class StitchWoundsSpell : PlayerAoeSpell
    {
        protected override int GetDamage(int playerLevel) => damage + 10 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Invocation;
            damage = 50;
        }

        protected override void ApplyEffect(IEnumerable<Player> affectedPlayers, int playerLevel, SpellData spellData)
        {
            int localPlayerId = Main.myPlayer;
            foreach (Player player in affectedPlayers)
            {
                int playerHealth = player.statLife;
                int maxPlayerHealth = player.statLifeMax2;
                int maxAllowedHealth = (int)(maxPlayerHealth * 0.35f);
                if (playerHealth > maxAllowedHealth)
                    continue;

                int healValue = GetDamage(playerLevel);
                int maxAllowedHeal = maxAllowedHealth - playerHealth;
                int actualHeal = Math.Min(healValue, maxAllowedHeal);

                player.statLife += actualHeal;
                player.HealEffect(actualHeal);
                player.ClearBuff(BuffID.Bleeding);

                int playerId = player.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != localPlayerId)
                {
                    ModNetHandler.otherPlayerHealHandler.Send(playerId, localPlayerId, actualHeal);
                    ModNetHandler.otherPlayerClearBuffsHandler.Send(playerId, localPlayerId, new int[] { BuffID.Bleeding });
                }
            }
        }
    }
}