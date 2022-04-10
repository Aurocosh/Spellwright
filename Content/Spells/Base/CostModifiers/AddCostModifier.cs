namespace Spellwright.Content.Spells.Base.CostModifiers
{
    internal class AddCostModifier : ICostModifier
    {
        public float Value { get; }

        public AddCostModifier(float value)
        {
            Value = value;
        }

        public float ModifyCost(float currentCost)
        {
            return currentCost + Value;
        }
    }
}
