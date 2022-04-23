using System;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts.Stats
{
    internal class HealthSpellCost : SpellCost
    {
        public int Cost { get; }

        public HealthSpellCost(int cost)
        {
            Cost = cost;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return true;

            if (player.statLife < realCost)
            {
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughHealth").Format(realCost);
                return false;
            }

            player.statLife -= realCost;
            return true;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return null;

            return Spellwright.GetTranslation("SpellCost", "HealthCost").Format(realCost);
        }
    }
}
