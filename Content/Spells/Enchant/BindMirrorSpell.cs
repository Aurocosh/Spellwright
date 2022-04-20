using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts;
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
            SpellLevel = 8;
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<BoundMagicMirror>();
            var itemSpellCost = new MultipleItemSpellCost();
            itemSpellCost.AddItemCost(ModContent.ItemType<SilverMirror>());
            itemSpellCost.AddItemCost(ModContent.ItemType<RareSpellReagent>(), 3);
            SpellCost = itemSpellCost;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;
            string locationName = spellData.Argument;

            var itemId = Item.NewItem(new EntitySource_Parent(player), player.Center, itemType, 1, false, 0, true);
            Item item = Main.item[itemId];
            var modItem = item.ModItem as BoundMagicMirror;
            modItem.LocationName = locationName;
            modItem.BoundLocation = player.position;

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemId, 1);

            Main.NewText(modItem.Tooltip.Key);

            return true;
        }
    }
}