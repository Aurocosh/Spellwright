using Spellwright.Content.Items.Mirrors;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
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
            SpellLevel = 9;
            UseType = SpellType.Invocation;
            itemType = ModContent.ItemType<BoundMagicMirror>();

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ModContent.ItemType<SilverMirror>(), 1)
                .WithCost(ItemID.TeleportationPotion, 10);

            CastCost = new MultipleReagentSpellCost()
                .WithCost(ModContent.ItemType<SilverMirror>(), 1)
                .WithCost(ModContent.ItemType<MythicalSpellReagent>(), 3);
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