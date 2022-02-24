﻿using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class SurgeOfLifeSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            int buff = ModContent.BuffType<SurgeOfLifeBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 2 * playerLevel));

            reagentType = ModContent.ItemType<RareSpellReagent>();
            SetExtraReagentCost(SpellModifier.IsAoe, 1);
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int localPlayerId = Main.myPlayer;
            int regenRate = 2 + playerLevel;
            foreach (var player in players)
            {
                var surgeOfLifePlayer = player.GetModPlayer<SurgeOfLifePlayer>();
                surgeOfLifePlayer.LifeRegenValue = regenRate;

                int playerId = player.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != localPlayerId)
                    ModNetHandler.surgeOfLifeHandler.Sync(playerId, regenRate);
            }
        }
    }
}