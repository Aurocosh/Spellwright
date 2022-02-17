namespace Spellwright.Spells
{
    internal sealed class SpellStringData : SpellData
    {
        public string Value { get; }

        public SpellStringData(string value)
        {
            Value = value;
        }
    }
}
