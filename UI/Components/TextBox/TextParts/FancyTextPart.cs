using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using CColor = System.Drawing.Color;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class FancyTextPart : PlainTextPart
    {
        public string Link { get; }
        public Color CustomColor { get; }

        public bool HasCustomColor { get; }

        public bool HasLink => Link?.Length > 0;

        public FancyTextPart(string text, string options, DynamicSpriteFont font) :
            base(text, font)
        {
            Link = "";
            HasCustomColor = false;
            foreach (var option in options.Split(','))
            {
                var parts = option.Split('=', 2);
                if (parts.Length == 2)
                {
                    var name = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (name == "link")
                    {
                        Link = value;
                    }
                    if (name == "color")
                    {
                        var clrColor = CColor.FromName(value);
                        CustomColor = new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);
                        HasCustomColor = true;
                    }
                }
            }

            if (!HasCustomColor && Link.Length > 0)
            {
                CustomColor = Color.CornflowerBlue;
                HasCustomColor = true;
            }
        }

        public FancyTextPart(string text, string link, Color customColor, bool hasCustomColor, DynamicSpriteFont font)
            : base(text, font)
        {
            Link = link;
            CustomColor = customColor;
            HasCustomColor = hasCustomColor;
        }

        public override Color GetColor(Color color)
        {
            return HasCustomColor ? CustomColor : color;
        }

        public override ITextPart Alter(string text)
        {
            return new FancyTextPart(text, Link, CustomColor, HasCustomColor, Font);
        }
    }
}
