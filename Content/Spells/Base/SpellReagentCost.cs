namespace Spellwright.Content.Spells.Base
{
    internal class SpellReagentCost
    {
        public int ItemType { get; }
        public int Cost { get; }

        public SpellReagentCost(int itemType, int cost)
        {
            ItemType = itemType;
            Cost = cost;
        }
    }
}
