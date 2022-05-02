using Microsoft.Xna.Framework;
using ReLogic.Graphics;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class PlainTextPart : ITextPart
    {
        public float Width { get; }
        public float Height { get; }
        public string Text { get; }
        public DynamicSpriteFont Font { get; }

        public PlainTextPart(string text, DynamicSpriteFont font)
        {
            Text = text;
            Font = font;
            var size = font.MeasureString(Text);
            Width = size.X;
            Height = size.Y;
        }

        public virtual Color GetColor(Color color)
        {
            return color;
        }
        public virtual ITextPart Alter(string text)
        {
            return new PlainTextPart(text, Font);
        }
    }
}
