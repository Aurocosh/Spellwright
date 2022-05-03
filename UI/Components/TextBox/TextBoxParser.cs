using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Spellwright.UI.Components.TextBox.TextParts;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.UI;

namespace Spellwright.UI.Components.TextBox
{
    internal class TextBoxParser
    {
        private static readonly Regex formatRegex = new(@"\*\[([^\]]*)\]\(([^)]*)\)");

        public List<ITextPart> ParseText(string text, DynamicSpriteFont font)
        {
            var textParts = new List<ITextPart>();

            int startIndex = 0;
            int endIndex = text.Length - 1;
            while (startIndex <= endIndex)
            {
                var match = formatRegex.Match(text, startIndex);
                if (match.Success)
                {
                    if (startIndex != match.Index)
                    {
                        var part = text[startIndex..match.Index];
                        textParts.Add(new PlainTextPart(part, font));
                    }

                    string linkText = match.Groups[1].Value;
                    string linkValue = match.Groups[2].Value;

                    textParts.Add(new FormattedTextPart(linkText, linkValue, font));
                    startIndex = match.Index + match.Length;
                }
                else
                {
                    var part = text.Substring(startIndex, endIndex - startIndex + 1);
                    textParts.Add(new PlainTextPart(part, font));
                    //startIndex = endIndex + 1;
                    break;
                }
            }

            var expandedParts = new List<ITextPart>();
            foreach (var part in textParts)
            {
                var subparts = part.Text.Split('\n');
                if (subparts.Length > 0)
                {
                    for (int i = 0; i < subparts.Length; i++)
                    {
                        if (i != 0)
                            expandedParts.Add(new SpecialSymbolTextPart('\n'));
                        var subpartText = subparts[i];
                        expandedParts.Add(part.Alter(subpartText));
                    }
                }
                else
                {
                    expandedParts.Add(part);
                }
            }

            return expandedParts;
        }

        public List<TextLine> SplitTextByLines(List<ITextPart> textParts, CalculatedStyle space, DynamicSpriteFont font)
        {
            var textLines = new List<TextLine>();
            float textHeight = font.MeasureString("A").Y;

            float linePartsWidth = 0;
            var lineParts = new List<ITextPart>();

            var unassignedParst = new LinkedList<ITextPart>(textParts);
            while (unassignedParst.Count > 0)
            {
                var nextPart = unassignedParst.First.Value;
                unassignedParst.RemoveFirst();

                if (nextPart is SpecialSymbolTextPart symbolTextPart && symbolTextPart.Symbol == '\n')
                {
                    var textLine = new TextLine(linePartsWidth, textHeight, lineParts);
                    textLines.Add(textLine);
                    lineParts = new List<ITextPart>();
                    linePartsWidth = 0;
                }

                float partWidth = font.MeasureString(nextPart.Text).X;
                if (linePartsWidth + partWidth < space.Width)
                {
                    lineParts.Add(nextPart);
                    linePartsWidth += partWidth;
                }
                else
                {
                    string acceptedText = nextPart.Text;
                    string remainder = "";
                    while (linePartsWidth + font.MeasureString(acceptedText).X > space.Width)
                    {
                        remainder = acceptedText[^1] + remainder;
                        acceptedText = acceptedText[0..^1];
                    }
                    if (remainder.Length > 0)
                    {
                        int index = acceptedText.LastIndexOf(' ');
                        if (index >= 0)
                        {
                            remainder = acceptedText[(index + 1)..] + remainder;
                            acceptedText = acceptedText[..index];
                        }
                    }

                    if (acceptedText.Length > 0)
                    {
                        var acceptedPart = nextPart.Alter(acceptedText);
                        var remainingPart = nextPart.Alter(remainder);
                        lineParts.Add(acceptedPart);
                        unassignedParst.AddFirst(remainingPart);
                        linePartsWidth += font.MeasureString(acceptedText).X;
                    }
                    else
                    {
                        unassignedParst.AddFirst(nextPart);
                    }

                    var textLine = new TextLine(linePartsWidth, textHeight, lineParts);
                    textLines.Add(textLine);
                    lineParts = new List<ITextPart>();
                    linePartsWidth = 0;
                }
            }

            if (lineParts.Count > 0)
            {
                var textLine = new TextLine(linePartsWidth, textHeight, lineParts);
                textLines.Add(textLine);
                lineParts = new List<ITextPart>();
                linePartsWidth = 0;
            }

            return textLines;
        }

        public List<UILinkData> GenerateLinkInformation(List<TextLine> textLines)
        {
            var links = new List<UILinkData>();

            int lineIndex = 0;
            float baseY = 0;
            foreach (var textLine in textLines)
            {
                float baseX = 0;
                foreach (var part in textLine.Parts)
                {
                    if (part is FormattedTextPart formattedPart && formattedPart.HasLink)
                    {
                        var minX = baseX;
                        var maxX = minX + part.Width;
                        var minY = baseY;
                        var maxY = minY + part.Height;

                        var topLeft = new Vector2(minX, minY);
                        var bottomRight = new Vector2(maxX, maxY);
                        links.Add(new UILinkData(formattedPart, lineIndex, formattedPart.Text, formattedPart.Link, topLeft, bottomRight));
                    }
                    baseX += part.Width;
                }
                lineIndex++;
                baseY += textLine.Height;
            }

            return links;
        }
    }
}
