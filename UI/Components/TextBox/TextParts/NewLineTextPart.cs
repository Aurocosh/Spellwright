using Microsoft.Xna.Framework;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class NewLineTextPart : TextPart
    {
        public float Width => 0;

        public float Height => 0;

        public string Text => "";

        public Color Color => Color.White;

        public Color GetColor(Color color)
        {
            return color;
        }
        public TextPart Alter(string text)
        {
            return new NewLineTextPart();
        }

    }
}
