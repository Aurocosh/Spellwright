using Microsoft.Xna.Framework;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class SpecialSymbolTextPart : ITextPart
    {
        public float Width => 0;

        public float Height => 0;

        public string Text => Symbol.ToString();

        public char Symbol { get; }

        public SpecialSymbolTextPart(char symbol)
        {
            Symbol = symbol;
        }

        public Color GetColor(Color color)
        {
            return color;
        }
        public ITextPart Alter(string text)
        {
            return new SpecialSymbolTextPart(Symbol);
        }
    }
}
