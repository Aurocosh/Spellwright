using System;
using Terraria;

namespace Spellwright.Content.Spells.Base.SpellCosts.Stats
{
    internal class TotalManaPercentSpellCost : SpellCost
    {
        public float CostPercent { get; }

        public TotalManaPercentSpellCost(float cost)
        {
            CostPercent = Math.Clamp(cost, 0, 1);
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            float costPercent = CostPercent * spellData.CostModifier;
            int realCost = (int)Math.Floor(player.statManaMax * costPercent);
            if (realCost <= 0)
                return true;

            if (player.statMana < realCost)
            {
                LastError = Spellwright.GetTranslation("SpellCost", "NotEnoughMana").Format(realCost);
                return false;
            }

            player.statMana -= realCost;
            return true;
        }

        public override string GetDescription(Player player, int playerLevel, SpellData spellData)
        {
            float costPercent = CostPercent * spellData.CostModifier;
            int realCost = (int)Math.Floor(player.statManaMax * costPercent);
            if (realCost <= 0)
                return null;

            return Spellwright.GetTranslation("SpellCost", "ManaCost").Format(realCost);
        }
    }
}
