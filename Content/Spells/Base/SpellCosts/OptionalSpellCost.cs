using System.Collections.Generic;
using Terraria;

namespace Spellwright.Content.Spells.Base.Reagents
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

            var errors = new List<string>();
            foreach (SpellCost cost in spellCosts)
            {
                errors.Add(cost.LastError);
                LastError = string.Join("\n", errors);
            }

            return false;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            var separatorWord = Spellwright.GetTranslation("General", "Or").Value.ToLower();
            var separator = $" {separatorWord} ";

            var descriptions = new List<string>();
            foreach (SpellCost cost in spellCosts)
            {
                var description = cost.GetDescription(player, playerLevel, spellData);
                if (description != null)
                    descriptions.Add(description);
            }

            if (descriptions.Count == 0)
                return null;
            else
                return string.Join(separator, descriptions);
        }
    }
}
