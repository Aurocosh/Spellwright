namespace Spellwright.Content.Spells.Base.CostModifiers
{
    internal class MultCostModifier : ICostModifier
    {
        public float Value { get; }

        public MultCostModifier(float value)
        {
            Value = value;
        }

        public float ModifyCost(float currentCost)
        {
            return currentCost * Value;
        }
    }
}
