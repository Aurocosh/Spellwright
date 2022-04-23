using System;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts.Stats
{
    internal class MaxHealthSpellCost : SpellCost
    {
        public int Cost { get; }

        public MaxHealthSpellCost(int cost)
        {
            Cost = cost;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return true;

            if (player.statLifeMax2 < realCost)
            {
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughMaxHealth").Format(realCost);
                return false;
            }

            player.statLifeMax2 -= realCost;
            return true;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return null;

            return Spellwright.GetTranslation("SpellCost", "MaxHealth").Format(realCost);
        }
    }
}
