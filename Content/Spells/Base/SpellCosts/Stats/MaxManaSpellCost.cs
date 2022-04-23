using System;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts.Stats
{
    internal class MaxManaSpellCost : SpellCost
    {
        public int Cost { get; }

        public MaxManaSpellCost(int cost)
        {
            Cost = cost;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return true;

            if (player.statManaMax < realCost)
            {
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughMaxMana").Format(realCost);
                return false;
            }

            player.statManaMax -= realCost;
            return true;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return null;

            return Spellwright.GetTranslation("SpellCost", "MaxMana").Format(realCost);
        }
    }
}
