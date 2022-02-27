using Spellwright.Common.Players;
using Spellwright.Content.Buffs.Items;
using Spellwright.DustSpawners;
using Spellwright.Network;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Items
{
    public class BizzareFruit : ModItem
    {
        public BizzareFruit()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bizzare fruit");
        }

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.useStyle = 4;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item4;
            Item.useAnimation = 30;
            Item.rare = 7;
            Item.value = Item.buyPrice(0, 1);
        }

        public override bool? UseItem(Player player)
        {
            SpellwrightPlayer modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            if (modPlayer.PlayerLevel == 0)
            {
                player.AddBuff(ModContent.BuffType<SoulDisturbanceDebuff>(), UtilTime.MinutesToTicks(3));

                var spawner = new SoulDisturbanceSpawner(player);
                spawner.Spawn();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.soulDisturbanceHandler.Send(spawner);

                return true;
            }

            return false;
        }
    }
}