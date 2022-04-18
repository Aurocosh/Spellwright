﻿using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Storage
{
    internal class VoidPopSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            if (statPlayer.StoredItems.Count == 0)
                return false;

            foreach (var item in statPlayer.StoredItems)
            {
                item.position = player.Center;
                if (item.stack > 0)
                {
                    var source = new EntitySource_Parent(player);
                    int itemIndex = Item.NewItem(source, player.Center, player.width, player.height, item.type, item.stack, noBroadcast: false, item.prefix, noGrabDelay: true);
                    Main.item[itemIndex] = item.Clone();
                    Main.item[itemIndex].newAndShiny = false;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1f);
                }
            }

            statPlayer.StoredItems.Clear();
            return true;
        }
    }
}