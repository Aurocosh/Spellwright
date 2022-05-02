using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using DColor = System.Drawing.Color;
using DColorTranslator = System.Drawing.ColorTranslator;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class FormattedTextPart : PlainTextPart
    {
        public string Link { get; }
        public Color CustomColor { get; }

        public bool HasCustomColor { get; }

        public bool HasLink => Link?.Length > 0;

        public FormattedTextPart(string text, string options, DynamicSpriteFont font) :
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
                    else if (name == "color")
                    {
                        var clrColor = DColor.FromName(value);
                        if (!clrColor.IsKnownColor)
                            clrColor = DColorTranslator.FromHtml(value);
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

        public FormattedTextPart(string text, string link, Color customColor, bool hasCustomColor, DynamicSpriteFont font)
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
            return new FormattedTextPart(text, Link, CustomColor, HasCustomColor, Font);
        }
    }
}
