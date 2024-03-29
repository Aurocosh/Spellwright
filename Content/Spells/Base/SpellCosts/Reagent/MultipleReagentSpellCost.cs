﻿using Spellwright.Common.Players;
using Spellwright.Extensions;
using Spellwright.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts.Reagent
{
    internal class MultipleReagentSpellCost : SpellCost
    {
        private readonly List<int> itemTypes;
        private readonly List<int> costs;

        public MultipleReagentSpellCost()
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
            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            for (int i = 0; i < itemTypes.Count; i++)
            {
                int itemType = itemTypes[i];
                int cost = costs[i];

                int realCost = (int)Math.Floor(cost * costModifier);
                if (realCost <= 0)
                    return true;

                var allItems = statPlayer.ReagentItems.Concat(player.GetInventoryItems()).Concat(player.IterateAllVacuumBagItems());
                if (!UtilInventory.HasItems(allItems, itemType, realCost))
                    return false;
            }
            return true;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            if (!CanConsume(player, spellData.CostModifier))
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < itemTypes.Count; i++)
                {
                    int itemType = itemTypes[i];
                    int cost = costs[i];

                    int realCost = (int)Math.Floor(cost * spellData.CostModifier);
                    if (realCost <= 0)
                        continue;

                    var itemName = Lang.GetItemNameValue(itemType);
                    var costPart = $"{realCost} {itemName}";
                    stringBuilder.AppendDelimited(",", costPart);
                }

                var costText = stringBuilder.ToString();
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughReagents").Format(costText);
                return false;
            }

            var statPlayer = player.GetModPlayer<SpellwrightStatPlayer>();
            for (int i = 0; i < itemTypes.Count; i++)
            {
                int itemType = itemTypes[i];
                int cost = costs[i];

                int realCost = (int)Math.Floor(cost * spellData.CostModifier);
                if (realCost <= 0)
                    return true;

                var allItems = statPlayer.ReagentItems.Concat(player.GetInventoryItems()).Concat(player.IterateAllVacuumBagItems());
                UtilInventory.ConsumeItems(allItems, itemType, realCost);
            }

            return true;
        }
        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            //var separatorWord = Spellwright.GetTranslation("General", "And").Value.ToLower();
            //var separator = $" {separatorWord} ";

            var descriptions = new StringBuilder();
            for (int i = 0; i < itemTypes.Count; i++)
            {
                int itemType = itemTypes[i];
                int cost = costs[i];

                int realCost = (int)Math.Floor(cost * spellData.CostModifier);
                if (realCost <= 0)
                    continue;

                var itemName = Lang.GetItemNameValue(itemType);
                var description = $"{realCost} {itemName}";
                descriptions.AppendDelimited(", ", description);
            }
            return descriptions.ToString();
        }

        public MultipleReagentSpellCost WithCost(int itemType, int cost = 1)
        {
            AddItemCost(itemType, cost);
            return this;
        }
    }
}
