using Spellwright.Content.Buffs.Spells;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base.SpellCosts;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Network;
using Spellwright.Util;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Spellwright.Content.Buffs.Spells.ReactiveArmorBuff;

namespace Spellwright.Content.Spells.BuffSpells
{
    internal class ReactiveArmorSpell : BuffSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;

            int buff = ModContent.BuffType<ReactiveArmorBuff>();
            AddEffect(buff, (playerLevel) => UtilTime.MinutesToTicks(10 + 3 * playerLevel));

            spellCost = new SingleItemSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
        }
        protected override void DoExtraActions(IEnumerable<Player> players, int playerLevel)
        {
            base.DoExtraActions(players, playerLevel);

            int localPlayerId = Main.myPlayer;
            int maxBonusDefense = 4 + 2 * playerLevel;
            foreach (Player player in players)
            {
                var reactiveArmorPlayer = player.GetModPlayer<ReactiveArmorPlayer>();
                reactiveArmorPlayer.BonusDefense = 0;
                reactiveArmorPlayer.MaxBonusDefense = maxBonusDefense;

                int playerId = player.whoAmI;
                if (Main.netMode == NetmodeID.MultiplayerClient && playerId != localPlayerId)
                {
                    ModNetHandler.reactiveArmorDefenseSync.Sync(playerId, 0);
                    ModNetHandler.reactiveArmorMaxDefenseSync.Sync(playerId, maxBonusDefense);
                }
            }
        }
    }
}
