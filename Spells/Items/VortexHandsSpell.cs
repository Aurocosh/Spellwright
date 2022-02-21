using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Spells.WarpSpells
{
    internal class VortexHandsSpell : Spell
    {
        public static int defaultItemGrabRange = 480;

        public VortexHandsSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int itemGrabRange = 1000;
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

            SpawnVortex(DustID.Cloud, player.Center, 95, 20, 600, 1);

            return true;
        }

        private static void SpawnVortex(int dustType, Vector2 position, int dustCount, int minRadius, int maxRadius, int direction = 1)
        {
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dustPosition = position + Main.rand.NextVector2CircularEdge(1, 1).ScaleRandom(minRadius, maxRadius);
                Vector2 velocity = position.DirectionTo(dustPosition).ScaleRandom(.1f, 2.5f);
                velocity = velocity.PerpendicularClockwise();
                velocity *= direction;

                var dust = Dust.NewDustDirect(dustPosition, 22, 22, dustType, 0f, 0f, 100, default, 1.5f);
                dust.velocity = velocity;
                dust.noLightEmittence = true;
            }
        }
    }
}