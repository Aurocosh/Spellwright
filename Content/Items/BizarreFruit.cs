using Spellwright.Common.Players;
using Spellwright.Content.Buffs.Items;
using Spellwright.ExecutablePackets.Broadcast.DustSpawners;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Items
{
    public class BizarreFruit : ModItem
    {
        public BizarreFruit()
        {
        }

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item4;
            Item.useAnimation = 30;
            Item.value = Item.buyPrice(0, 0, 10);
            Item.rare = ItemRarityID.Red;
        }

        public override bool? UseItem(Player player)
        {
            SpellwrightPlayer spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            if (!spellPlayer.CanCastSpells)
            {
                player.AddBuff(ModContent.BuffType<SoulDisturbanceDebuff>(), UtilTime.MinutesToTicks(3));

                var spawner = new SoulDisturbanceSpawner(player);
                spawner.Execute();

                return true;
            }

            return false;
        }
    }
}