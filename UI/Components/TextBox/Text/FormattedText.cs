using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DColor = System.Drawing.Color;
using DColorTranslator = System.Drawing.ColorTranslator;

namespace Spellwright.UI.Components.TextBox.Text
{
    internal class FormattedText
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public Color? CustomColor { get; set; }

        public FormattedText(string text)
        {
            Text = text;
            Link = "";
            CustomColor = null;
        }

        public FormattedText(string text, Color color)
        {
            Text = text;
            Link = "";
            CustomColor = color;
        }

        public FormattedText WithColor(Color color)
        {
            CustomColor = color;
            return this;
        }

        public FormattedText WithLink(string linkType, string link)
        {
            Link = $"{linkType}:{link}";
            return this;
        }

        public override string ToString()
        {
            var options = new List<string>(2);
            if (Link.Length > 0)
            {
                options.Add($"link={Link}");
            }
            if (CustomColor != null)
            {
                var color = CustomColor.Value;
                var dcolor = DColor.FromArgb(color.A, color.R, color.G, color.B);
                string colorHex = DColorTranslator.ToHtml(dcolor);
                options.Add($"color={colorHex}");
            }

            if (options.Count == 0)
                return Text;

            var optionString = string.Join(',', options);
            return $"[{Text}]({optionString})";
        }
    }
}
