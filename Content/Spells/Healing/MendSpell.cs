using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Healing
{
    internal class MendSpell : PlayerAoeSpell
    {
        protected override int GetDamage(int playerLevel) => damage + 10 * playerLevel;

        public override void SetStaticDefaults()
        {
            SpellLevel = 0;
            UseType = SpellType.Invocation;
            damage = 50;

            UnlockCost = new SingleItemSpellCost(ItemID.LesserHealingPotion, 20);
            CastCost = new ReagentSpellCost(ModContent.ItemType<CommonSpellReagent>(), 1);
        }

        protected override void ApplyEffect(IEnumerable<Player> affectedPlayers, int playerLevel, SpellData spellData)
        {
            int localPlayerId = Main.myPlayer;
            foreach (Player player in affectedPlayers)
            {
                int playerHealth = player.statLife;
                int maxPlayerHealth = player.statLifeMax2;
                int maxAllowedHealth = (int)(maxPlayerHealth * 0.5f);
                if (playerHealth > maxAllowedHealth)
                    continue;

                int healValue = maxAllowedHealth - playerHealth;

                player.statLife += healValue;
                player.HealEffect(healValue);
                player.ClearBuff(BuffID.Bleeding);

                int playerId = player.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != localPlayerId)
                {
                    ModNetHandler.otherPlayerHealHandler.Send(playerId, localPlayerId, healValue);
                    ModNetHandler.otherPlayerClearBuffsHandler.Send(playerId, localPlayerId, new int[] { BuffID.Bleeding });
                }
            }
        }
    }
}