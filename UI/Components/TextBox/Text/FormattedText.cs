using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using DColor = System.Drawing.Color;
using DColorTranslator = System.Drawing.ColorTranslator;

namespace Spellwright.UI.Components.TextBox.Text
{
    internal class FormattedText
    {
        public string Text { get; set; }
        public LinkData Link { get; set; }
        public Color? CustomColor { get; set; }

        public FormattedText(string text)
        {
            Text = text;
            Link = null;
            CustomColor = null;
        }

        public FormattedText(string text, Color? color)
        {
            Text = text;
            Link = null;
            CustomColor = color;
        }

        public FormattedText WithColor(Color color)
        {
            CustomColor = color;
            return this;
        }

        public FormattedText WithLink(string linkType)
        {
            Link = new LinkData(linkType);
            return this;
        }

        public FormattedText WithLink(string linkType, string parameter)
        {
            Link = new LinkData(linkType);
            Link.SetParameter(parameter);
            return this;
        }

        public FormattedText WithParam(string parameter, string value = "")
        {
            Link.SetParameter(parameter, value);
            return this;
        }

        public FormattedText WithParam<T>(string parameter, T value)
            where T : Enum
        {
            Link.SetParameter(parameter, value);
            return this;
        }

        public FormattedText WithParam(string parameter, bool value)
        {
            Link.SetParameter(parameter, value);
            return this;
        }

        public FormattedText WithLink(LinkData link)
        {
            Link = link;
            return this;
        }

        public override string ToString()
        {
            var options = new List<string>(2);
            if (Link != null)
            {
                options.Add(Link.ToString());
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
            return $"*[{Text}]({optionString})";
        }
    }
}
