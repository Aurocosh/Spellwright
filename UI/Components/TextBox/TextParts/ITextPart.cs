using Microsoft.Xna.Framework;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal interface ITextPart
    {
        public float Width { get; }
        public float Height { get; }
        public string Text { get; }

        public Color GetColor(Color color);
        public ITextPart Alter(string text);
    }
}
