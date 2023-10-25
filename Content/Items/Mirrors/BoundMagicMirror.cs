using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellwright.Content.Items.Mirrors
{
    public class BoundMagicMirror : ModItem
    {
        public string WorldName { get; set; }
        public string LocationName { get; set; }
        public Vector2 BoundLocation { get; set; }

        public BoundMagicMirror()
        {
            WorldName = "";
            LocationName = "";
            BoundLocation = new Vector2(0, 0);
        }

        protected virtual Color ParticleColor => Color.Purple;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bound Magic Mirror");
            // Tooltip.SetDefault("Enchanted magical mirror eternally bound to a place in this world.\nLook into the mirror to teleport to this place.");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceMirror); // Copies the defaults from the Ice Mirror.
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Yellow;
        }

        public override ModItem Clone(Item item)
        {
            var clone = (BoundMagicMirror)base.Clone(item);
            clone.WorldName = WorldName;
            clone.LocationName = LocationName;
            clone.BoundLocation = BoundLocation;
            return clone;
        }

        public override void OnCreated(ItemCreationContext context)
        {
            WorldName = "";
            LocationName = "";
            BoundLocation = new Vector2(0, 0);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (WorldName.Length != 0)
            {
                var tooltipLine = new TooltipLine(Mod, "World name", Spellwright.GetTranslation("BoundMirror", "WorldName").Format(WorldName));
                tooltips.Add(tooltipLine);
            }

            if (LocationName.Length != 0)
            {
                var tooltipLine = new TooltipLine(Mod, "Location name", Spellwright.GetTranslation("BoundMirror", "LocationName").Format(LocationName));
                tooltips.Add(tooltipLine);
            }

            //var coordinateLine = new TooltipLine(Mod, "Location coordinates", Spellwright.GetTranslation("BoundMirror", "LocationCoordinates").Format(x, y));
            //tooltips.Add(coordinateLine);

            if (BoundLocation != Vector2.Zero)
            {
                int x = (int)(BoundLocation.X / 16f);
                int y = (int)(BoundLocation.Y / 16f);

                int xConverted = x * 2 - Main.maxTilesX;
                var cardinalCoord = ((xConverted > 0) ? Language.GetTextValue("GameUI.CompassEast", xConverted) : ((xConverted >= 0) ? Language.GetTextValue("GameUI.CompassCenter") : Language.GetTextValue("GameUI.CompassWest", -xConverted)));


                int num22 = (int)(y - Main.worldSurface) * 2;
                float num23 = Main.maxTilesX / 4200;
                num23 *= num23;
                int num24 = 1200;
                float num25 = (float)((y - (65f + 10f * num23)) / (Main.worldSurface / 5.0));

                string text6 = ((y > (float)((Main.maxTilesY - 204) * 16)) ? Language.GetTextValue("GameUI.LayerUnderworld") : ((y > Main.rockLayer * 16.0 + (double)(num24 / 2) + 16.0) ? Language.GetTextValue("GameUI.LayerCaverns") : ((num22 > 0) ? Language.GetTextValue("GameUI.LayerUnderground") : ((!(num25 >= 1f)) ? Language.GetTextValue("GameUI.LayerSpace") : Language.GetTextValue("GameUI.LayerSurface")))));
                num22 = Math.Abs(num22);
                var text7 = ((num22 != 0) ? Language.GetTextValue("GameUI.Depth", num22) : Language.GetTextValue("GameUI.DepthLevel"));
                var text2 = text7 + " " + text6;

                var coords = $"{cardinalCoord}, {text2}";


                var coordinateLine2 = new TooltipLine(Mod, "Location coordinates2", Spellwright.GetTranslation("BoundMirror", "BoundCoordinates").Format(coords));
                tooltips.Add(coordinateLine2);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("WorldName", WorldName);
            tag.Add("LocationName", LocationName);
            tag.Add("BoundLocationX", BoundLocation.X);
            tag.Add("BoundLocationY", BoundLocation.Y);
        }

        public override void LoadData(TagCompound tag)
        {
            WorldName = tag.GetString("WorldName");
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
            writer.Write(WorldName);
            writer.Write(LocationName);
            writer.Write(BoundLocation.X);
            writer.Write(BoundLocation.Y);
        }

        public override void NetReceive(BinaryReader reader)
        {
            WorldName = reader.ReadString();
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

                if (Main.worldName == WorldName && BoundLocation != Vector2.Zero)
                {
                    player.Teleport(BoundLocation, 20);
                    player.velocity = Vector2.Zero;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        RemoteClient.CheckSection(player.whoAmI, player.position);
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, BoundLocation.X, BoundLocation.Y, 20);
                    }
                }
                else
                {
                    player.Spawn(PlayerSpawnContext.RecallFromItem);
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