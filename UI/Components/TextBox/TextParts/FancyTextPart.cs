using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using CColor = System.Drawing.Color;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class FancyTextPart : PlainTextPart
    {
        public string Link { get; }
        public Color Color { get; }

        public FancyTextPart(string text, string options, DynamicSpriteFont font) :
            base(text, font)
        {
            Color = Color.CornflowerBlue;
            Link = options;
            foreach (var option in options.Split(','))
            {
                var parts = option.Split('=', 2);
                if (parts.Length < 2)
                    continue;

                var name = parts[0].Trim();
                var value = parts[1].Trim();

                if (name == "link")
                {
                    Link = value;
                }
                if (name == "color")
                {
                    var clrColor = CColor.FromName(value);
                    Color = new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);
                }
            }
        }

        public override Color GetColor(Color color)
        {
            return Color;
        }

        public override TextPart Alter(string text)
        {
            return new FancyTextPart(text, Link, Font);
        }
    }
}
