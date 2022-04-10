namespace Spellwright.Content.Spells.Base.CostModifiers
{
    public interface ICostModifier
    {
        float ModifyCost(float currentCost);
    }
}
