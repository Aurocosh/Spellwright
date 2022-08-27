using Microsoft.Xna.Framework;
using System;
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
            Link = new LinkData();
            CustomColor = null;
        }

        public FormattedText(string text, Color? color)
        {
            Text = text;
            Link = new LinkData();
            CustomColor = color;
        }

        public FormattedText WithColor(Color color)
        {
            CustomColor = color;
            return this;
        }

        public FormattedText WithLink(string linkType)
        {
            Link.Type = linkType;
            return this;
        }

        public FormattedText WithLink(string linkType, string linkId)
        {
            Link.Type = linkType;
            Link.Id = linkId;
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

        public FormattedText WithParam(string parameter, int value)
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
            if (CustomColor != null)
            {
                var color = CustomColor.Value;
                var dcolor = DColor.FromArgb(color.A, color.R, color.G, color.B);
                string colorHex = DColorTranslator.ToHtml(dcolor);
                Link.SetParameter("color", colorHex);
            }

            var optionString = Link.ToString();
            return $"*[{Text}]{optionString}";
        }
    }
}
