using System;
using Terraria;

namespace Spellwright.Content.Spells.Base.Reagents
{
    internal class ManaSpellCost : SpellCost
    {
        public int Cost { get; }

        public ManaSpellCost(int cost)
        {
            Cost = cost;
        }

        public override bool Consume(Player player, int playerLevel, SpellData spellData)
        {
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
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
            int realCost = (int)Math.Floor(Cost * spellData.CostModifier);
            if (realCost <= 0)
                return null;

            return Spellwright.GetTranslation("SpellCost", "ManaCost").Format(realCost);
        }
    }
}
