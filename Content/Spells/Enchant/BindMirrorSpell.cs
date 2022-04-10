using Spellwright.Content.Items;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Reagents;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Enchant
{
    internal class BindMirrorSpell : ModSpell
    {
        protected int itemType;

        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<BoundMagicMirror>();
            spellCost = new SingleItemSpellCost(ModContent.ItemType<SilverMirror>());
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;
            string locationName = spellData.Argument;

            var itemId = Item.NewItem(new EntitySource_ItemUse(player, null), player.Center, itemType, 1, false, 0, true);
            Item item = Main.item[itemId];
            var modItem = item.ModItem as BoundMagicMirror;
            modItem.LocationName = locationName;
            modItem.BoundLocation = player.position;

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemId, 1);

            return true;
        }
    }
}