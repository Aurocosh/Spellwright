using Microsoft.Xna.Framework;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal interface TextPart
    {
        public float Width { get; }
        public float Height { get; }
        public string Text { get; }

        public Color GetColor(Color color);
        public TextPart Alter(string text);
    }
}
