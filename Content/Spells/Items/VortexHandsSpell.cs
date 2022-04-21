using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Extensions;
using Spellwright.Network;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Items
{
    internal class VortexHandsSpell : ModSpell
    {
        public static int defaultItemGrabRange = 480;
        public override void SetStaticDefaults()
        {
            SpellLevel = 8;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int itemGrabRange = 60 * 16;
            //var pickupRectangle = new Rectangle((int)player.position.X - itemGrabRange, (int)player.position.Y - itemGrabRange, player.width + itemGrabRange * 2, player.height + itemGrabRange * 2);
            var pickupRectangle = player.GetAreaRect(itemGrabRange);

            for (int i = 0; i < 400; i++)
            {
                Item item = Main.item[i];
                if (!item.active || item.noGrabDelay != 0 || item.playerIndexTheItemIsReservedFor != player.whoAmI || !player.CanAcceptItemIntoInventory(item))
                    continue;
                if (!ItemLoader.CanPickup(item, player))
                    continue;

                Rectangle hitbox = item.Hitbox;

                if (pickupRectangle.Intersects(hitbox))
                {
                    item.position = new Vector2(player.Center.X, player.Center.Y - 40);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
                }
            }

            var spawner = new VortexDustSpawner
            {
                Caster = player,
                DustType = DustID.Cloud,
                DustCount = 95,
                Radius = 40
            };
            spawner.Execute();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.vortexDustHandler.Send(spawner);

            return true;
        }
    }
}