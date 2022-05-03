using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Extensions;
using System.Collections.Generic;
using System.Text;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts
{
    internal class OptionalSpellCost : SpellCost
    {
        private readonly List<SpellCost> spellCosts;

        public OptionalSpellCost()
        {
            spellCosts = new List<SpellCost>();
        }

        public OptionalSpellCost(params SpellCost[] costs)
        {
            spellCosts = new List<SpellCost>(costs);
        }

        public void AddOptionalCost(SpellCost cost)
        {
            spellCosts.Add(cost);
        }

        public void ClearCosts()
        {
            spellCosts.Clear();
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            foreach (SpellCost cost in spellCosts)
                if (cost.Consume(player, playerLevel, spellData))
                    return true;

            var errors = new StringBuilder();
            foreach (SpellCost cost in spellCosts)
            {
                var lastError = cost.LastError;
                if (lastError?.Length > 0)
                    errors.AppendLine(lastError);
            }
            LastError = errors.ToString();
            return false;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            var separatorWord = Spellwright.GetTranslation("General", "Or").Value.ToLower();
            var separator = $" {separatorWord} ";

            var stringBuilder = new StringBuilder();
            foreach (SpellCost cost in spellCosts)
            {
                var description = cost.GetDescription(player, playerLevel, spellData);
                if (description != null)
                    stringBuilder.AppendDelimited(separator, description);
            }
            return stringBuilder.ToString();
        }

        public OptionalSpellCost WithCost(SpellCost cost)
        {
            spellCosts.Add(cost);
            return this;
        }

        public OptionalSpellCost WithCost(int itemType, int cost = 1)
        {
            spellCosts.Add(new SingleItemSpellCost(itemType, cost));
            return this;
        }
    }
}
