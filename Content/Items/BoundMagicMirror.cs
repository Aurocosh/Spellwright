using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Items
{
    public class BoundMagicMirror : ModItem
    {
        public string LocationName { get; set; }
        public Vector2 BoundLocation { get; set; }

        public BoundMagicMirror()
        {
            LocationName = "Not set";
            BoundLocation = new Vector2(0, 0);
        }

        protected virtual Color ParticleColor => Color.Purple;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bound Magic Mirror");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceMirror); // Copies the defaults from the Ice Mirror.
        }

        public override ModItem Clone(Item item)
        {
            var clone = (BoundMagicMirror)base.Clone(item);
            clone.LocationName = LocationName;
            clone.BoundLocation = BoundLocation;
            return clone;
        }

        public override void OnCreate(ItemCreationContext context)
        {
            LocationName = "Not set";
            BoundLocation = new Vector2(0, 0);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (LocationName.Length != 0)
            {
                var tooltipLine = new TooltipLine(Mod, "Location name", Spellwright.GetTranslation("Generic", "LocationName") + ":" + LocationName);
                tooltips.Add(tooltipLine);
            }

            int x = (int)(BoundLocation.X / 16f);
            int y = (int)(BoundLocation.Y / 16f);
            var coordinateLine = new TooltipLine(Mod, "Location coordinates", Spellwright.GetTranslation("Generic", "LocationCoordinates") + $"X:{x} Y:{y}");
            tooltips.Add(coordinateLine);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("LocationName", LocationName);
            tag.Add("BoundLocationX", BoundLocation.X);
            tag.Add("BoundLocationY", BoundLocation.Y);
        }

        public override void LoadData(TagCompound tag)
        {
            LocationName = tag.GetString("LocationName");
            float locationX = tag.GetFloat("BoundLocationX");
            float locationY = tag.GetFloat("BoundLocationY");
            BoundLocation = new Vector2(locationX, locationY);
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(LocationName);
            writer.Write(BoundLocation.X);
            writer.Write(BoundLocation.Y);
        }

        public override void NetReceive(BinaryReader reader)
        {
            LocationName = reader.ReadString();
            float locationX = reader.ReadSingle();
            float locationY = reader.ReadSingle();
            BoundLocation = new Vector2(locationX, locationY);
        }

        // UseStyle is called each frame that the item is being actively used.
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
                Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, ParticleColor, 1.1f); // Makes dust from the player's position and copies the hitbox of which the dust may spawn. Change these arguments if needed.

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
                player.ApplyItemTime(Item);
            else if (player.itemTime == player.itemTimeMax / 2)
            {
                // This code runs once halfway through the useTime of the Item. You'll notice with magic mirrors you are still holding the item for a little bit after you've teleported.

                // Make dust 70 times for a cool effect.
                for (int d = 0; d < 30; d++)
                    Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, ParticleColor, 1.5f);

                // This code releases all grappling hooks and kills/despawns them.
                player.grappling[0] = -1;
                player.grapCount = 0;

                for (int p = 0; p < 1000; p++)
                    if (Main.projectile[p].active && Main.projectile[p].owner == player.whoAmI && Main.projectile[p].aiStyle == 7)
                        Main.projectile[p].Kill();

                // The actual method that moves the player back to bed/spawn.
                //player.Spawn(PlayerSpawnContext.RecallFromItem);

                //player.Teleport(BoundLocation, 5);
                player.Teleport(BoundLocation, 20);
                player.velocity = Vector2.Zero;
                if (Main.netMode == NetmodeID.Server)
                {
                    RemoteClient.CheckSection(player.whoAmI, player.position);
                    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, BoundLocation.X, BoundLocation.Y, 20);
                }

                // Make dust 70 times for a cool effect. This dust is the dust at the destination.
                for (int d = 0; d < 30; d++)
                    Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, ParticleColor, 1.5f);

                if (Item.consumable)
                    SoundEngine.PlaySound(SoundID.Shatter, player.position);
            }
        }
    }
}