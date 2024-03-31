using Spellwright.Extensions;
using Spellwright.Util;
using System;
using System.Linq;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts.Items
{
    internal class SingleItemSpellCost : SpellCost
    {
        public int ItemType { get; }
        public int Cost { get; }

        public SingleItemSpellCost(int itemType, int cost = 1)
        {
            ItemType = itemType;
            Cost = cost;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            if (ItemType <= 0)
                return true;

            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return true;

            var allItems = player.GetInventoryItems().Concat(player.IterateAllVacuumBagItems());
            if (!UtilInventory.ConsumeItems(allItems, ItemType, realCost))
            {
                var itemName = Lang.GetItemName(ItemType);
                var costText = $"{realCost} {itemName}";
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughReagents").Format(costText);
                return false;
            }
            return true;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            if (ItemType <= 0)
                return null;

            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return null;

            var itemName = Lang.GetItemNameValue(ItemType);
            return $"{realCost} {itemName}";
        }
    }
}
