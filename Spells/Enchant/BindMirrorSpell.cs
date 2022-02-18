using Spellwright.Items;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Spells.WarpSpells
{
    internal class BindMirrorSpell : Spell
    {
        protected int itemType;

        public BindMirrorSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
            itemType = ModContent.ItemType<BoundMagicMirror>();
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (!player.ConsumeItem(ModContent.ItemType<SilverMirror>()))
                return false;

            if (spellData == null)
                return false;
            string locationName = spellData.Argument;

            var itemId = Item.NewItem(player.Center, itemType, 1, false, 0, true);
            Item item = Main.item[itemId];
            var modItem = item.ModItem as BoundMagicMirror;
            modItem.LocationName = locationName;
            modItem.BoundLocation = player.position;

            return true;
        }
    }
}