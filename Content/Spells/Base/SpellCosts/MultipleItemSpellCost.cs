using Spellwright.Extensions;
using System;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Content.Spells.Base.Reagents
{
    internal class MultipleItemSpellCost : SpellCost
    {
        private readonly List<int> itemTypes;
        private readonly List<int> costs;

        public MultipleItemSpellCost()
        {
            itemTypes = new List<int>();
            costs = new List<int>();
        }

        public void AddItemCost(int itemType, int cost = 1)
        {
            if (itemType <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemType));
            if (cost <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemType));

            itemTypes.Add(itemType);
            costs.Add(cost);
        }

        public void ClearCosts()
        {
            itemTypes.Clear();
            costs.Clear();
        }

        public bool CanConsume(Player player, float costModifier)
        {
            for (int i = 0; i < itemTypes.Count; i++)
            {
                int itemType = itemTypes[i];
                int cost = costs[i];

                int realCost = (int)Math.Floor(cost * costModifier);
                if (realCost <= 0)
                    return true;

                if (!player.HasItems(itemType, realCost))
                    return false;
            }
            return true;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            if (!CanConsume(player, spellData.CostModifier))
            {
                var costParts = new List<string>();
                for (int i = 0; i < itemTypes.Count; i++)
                {
                    int itemType = itemTypes[i];
                    int cost = costs[i];

                    int realCost = (int)Math.Floor(cost * spellData.CostModifier);
                    if (realCost <= 0)
                        continue;

                    var itemName = Lang.GetItemNameValue(itemType);
                    var costPart = $"{realCost} {itemName}";
                    costParts.Add(costPart);
                }

                var costText = string.Join(", ", costParts);
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughReagents").Format(costText);
                return false;
            }

            for (int i = 0; i < itemTypes.Count; i++)
            {
                int itemType = itemTypes[i];
                int cost = costs[i];

                int realCost = (int)Math.Floor(cost * spellData.CostModifier);
                if (realCost <= 0)
                    return true;

                player.ConsumeItems(itemType, realCost);
            }

            return true;
        }
        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            var separatorWord = Spellwright.GetTranslation("General", "And").Value.ToLower();
            var separator = $" {separatorWord} ";

            var descriptions = new List<string>();
            for (int i = 0; i < itemTypes.Count; i++)
            {
                int itemType = itemTypes[i];
                int cost = costs[i];

                int realCost = (int)Math.Floor(cost * spellData.CostModifier);
                if (realCost <= 0)
                    continue;

                var itemName = Lang.GetItemNameValue(itemType);
                var description = $"{itemName}-{realCost}";
                descriptions.Add(description);
            }

            if (descriptions.Count == 0)
                return null;
            else
                return string.Join(separator, descriptions);
        }
    }
}
