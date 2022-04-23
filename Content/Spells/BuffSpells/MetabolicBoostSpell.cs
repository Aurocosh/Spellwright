using Spellwright.Common.Players;
using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Stats;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class MetabolicBoostSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 10;

            int buff = ModContent.BuffType<MetabolicBoostBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(2 + playerLevel));

            UnlockCost = new MaxHealthSpellCost(400);
            SpellCost = new SingleItemSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 2);
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int boostCount = (int)(.4f * playerLevel);

            int localPlayerId = Main.myPlayer;
            foreach (Player player in players)
            {
                var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
                statPlayer.MetaBoostCount = boostCount;
                int playerId = player.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != localPlayerId)
                    ModNetHandler.MetaBoostCountSetHandler.Send(playerId, localPlayerId, boostCount);
            }
        }
    }
}
