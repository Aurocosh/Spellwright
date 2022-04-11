namespace Spellwright.Content.Spells.Base
{
    public class SpellParameter
    {
        public string Name { get; }
        public string Value { get; }
        public SpellParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
