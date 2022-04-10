using Spellwright.Extensions;
using System;
using Terraria;

namespace Spellwright.Content.Spells.Base.Reagents
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

        public bool CanConsume(Player player, int playerLevel, float costModifier, SpellData spellData)
        {
            if (ItemType <= 0)
                return true;

            int realCost = (int)Math.Floor(Cost * costModifier);
            if (realCost <= 0)
                return true;

            return player.HasItems(ItemType, Cost);
        }

        public override bool Consume(Player player, int playerLevel, float costModifier, SpellData spellData)
        {
            if (ItemType <= 0)
                return true;

            int realCost = (int)Math.Floor(Cost * costModifier);
            if (realCost <= 0)
                return true;

            if (!player.ConsumeItems(ItemType, Cost))
            {
                LastError = Spellwright.GetTranslation("Messages", "NotEnoughReagents");
                return false;
            }
            return true;
        }
    }
}
