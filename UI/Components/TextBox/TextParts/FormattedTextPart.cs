using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Spellwright.UI.Components.TextBox.Text;
using System.Collections.Generic;
using DColor = System.Drawing.Color;
using DColorTranslator = System.Drawing.ColorTranslator;

namespace Spellwright.UI.Components.TextBox.TextParts
{
    internal class FormattedTextPart : PlainTextPart
    {
        private readonly Dictionary<string, Color> linkColorMap = new()
        {
            { "Spell", Color.DarkGoldenrod },
            { "ModItem", Color.DarkSlateBlue},
        };

        public int TextId { get; }
        public bool HasLink { get; }
        public string ParameterText { get; }
        public Color CustomColor { get; }
        public bool HasCustomColor { get; }

        public FormattedTextPart(int textId, string text, string linkText, DynamicSpriteFont font) :
            base(text, font)
        {
            TextId = textId;
            ParameterText = "";
            HasCustomColor = false;

            var linkData = LinkData.Parse(linkText);
            HasLink = linkData.IsLinkValid;

            if (linkData.HasParameter("color"))
            {
                var colorName = linkData.GetParameter("color");
                var clrColor = DColor.FromName(colorName);
                if (!clrColor.IsKnownColor)
                    clrColor = DColorTranslator.FromHtml(colorName);
                CustomColor = new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);
                HasCustomColor = true;

            }

            // TODO tostring
            //linkData.RemoveParameter("color");
            //LinkText = linkData.ToString();
            ParameterText = linkText;

            if (!HasCustomColor && HasLink)
            {
                if (!linkColorMap.TryGetValue(linkData.Type, out var color))
                    color = Color.CornflowerBlue;

                CustomColor = color;
                HasCustomColor = true;
            }
        }

        public FormattedTextPart(int textId, string text, bool hasLink, string parameterText, Color customColor, bool hasCustomColor, DynamicSpriteFont font)
            : base(text, font)
        {
            TextId = textId;
            HasLink = hasLink;
            ParameterText = parameterText;
            CustomColor = customColor;
            HasCustomColor = hasCustomColor;
        }

        public override Color GetColor(Color color)
        {
            return HasCustomColor ? CustomColor : color;
        }

        public override ITextPart Alter(string text)
        {
            return new FormattedTextPart(TextId, text, HasLink, ParameterText, CustomColor, HasCustomColor, Font);
        }
    }
}
