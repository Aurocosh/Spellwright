using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Spellwright.UI.Components.Args;
using Spellwright.UI.Components.TextBox;
using Spellwright.UI.Components.TextBox.TextParts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UIMessageBox : UIPanel
    {
        protected UIScrollbar Scrollbar;

        private string text;
        private float height;
        private bool heightNeedsRecalculating;
        private readonly List<LineData> _drawTexts = new();
        private readonly List<TextLine> _textLines = new();
        private readonly List<LinkData> linkDatas = new();

        public event EventHandler<LineClickEventArgs> OnLineClicked;

        public UIMessageBox(string text)
        {
            SetText(text);
        }

        public override void OnActivate()
        {
            base.OnActivate();
            heightNeedsRecalculating = true;
        }

        public bool HasText()
        {
            return text.Length > 0;
        }

        public string GetText()
        {
            return text;
        }

        public void SetText(string text)
        {
            this.text = text;
            ResetScrollbar();
        }

        private void ResetScrollbar()
        {
            if (Scrollbar != null)
            {
                Scrollbar.ViewPosition = 0;
                heightNeedsRecalculating = true;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle space = GetInnerDimensions();
            float position = 0f;
            if (Scrollbar != null)
            {
                position = -Scrollbar.GetValue();
            }
            //foreach (var drawText in _drawTexts)
            //{
            //    if (position + drawText.height > space.Height)
            //        break;
            //    if (position >= 0)
            //        Utils.DrawBorderString(spriteBatch, drawText.text, new Vector2(space.X, space.Y + position), Color.White, 1f);
            //    position += drawText.height;
            //}
            foreach (var textLine in _textLines)
            {
                if (position + textLine.Height > space.Height)
                    break;

                if (position >= 0)
                {
                    float partsWidth = 0;
                    foreach (var part in textLine.Parts)
                    {
                        Utils.DrawBorderString(spriteBatch, part.Text, new Vector2(space.X + partsWidth, space.Y + position), part.GetColor(Color.White), 1f);
                        partsWidth += part.Width;
                    }
                }
                position += textLine.Height;
            }

            Recalculate();
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            if (!heightNeedsRecalculating)
            {
                return;
            }
            CalculatedStyle space = GetInnerDimensions();
            if (space.Width <= 0 || space.Height <= 0)
            {
                return;
            }
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            _drawTexts.Clear();


            var input = "My favorite search engine is [Duck Duck Go](link=Spell:Bird Of Midas,color=blue). Got stuck between two objects, waited 5 minutes for the narrator to [speak](link=spell:Return), realised it was a bug; I've personally never had the need for a RectangleF structure, as I always cast floats to int when using Rectangles or simply avoid float altogether when positioning sprites. Not sure why SpriteBatch works with both float and int depending on the overload, but that's how Microsoft implemented it.";

            input = text;

            var builder = new StringBuilder();
            var textParts = new List<TextPart>();

            var rg = new Regex(@"\[([^\]]*)\]\(([^)]*)\)");

            var linkPositions = new List<(int index, int length)>();

            int outputIndex = 0;

            int startIndex = 0;
            int endIndex = input.Length - 1;
            while (startIndex <= endIndex)
            {
                var match = rg.Match(input, startIndex);
                if (match.Success)
                {
                    if (startIndex != match.Index)
                    {
                        var part = input.Substring(startIndex, match.Index - startIndex);
                        builder.Append(part);
                        textParts.Add(new PlainTextPart(part, font));
                        outputIndex += part.Length;
                    }

                    string linkText = match.Groups[1].Value;
                    string linkValue = match.Groups[2].Value;

                    var extraData = linkValue.Split(",");



                    builder.Append(linkText);
                    textParts.Add(new FancyTextPart(linkText, linkValue, font));
                    startIndex = match.Index + match.Length;

                    linkPositions.Add((outputIndex, linkText.Length));
                    outputIndex += linkText.Length;
                }
                else
                {
                    var part = input.Substring(startIndex, endIndex - startIndex + 1);
                    builder.Append(part);
                    textParts.Add(new PlainTextPart(part, font));
                    //startIndex = endIndex + 1;
                    break;
                }
            }



            var result = builder.ToString();

            int startingIndex = 0;

            float position = 0f;
            float textHeight = font.MeasureString("A").Y;
            //foreach (string line in text.Split('\n'))
            foreach (string line in result.Split('\n'))
            {
                string drawString = line;
                do
                {
                    string remainder = "";
                    while (font.MeasureString(drawString).X > space.Width)
                    {
                        remainder = drawString[drawString.Length - 1] + remainder;
                        drawString = drawString.Substring(0, drawString.Length - 1);
                    }
                    if (remainder.Length > 0)
                    {
                        int index = drawString.LastIndexOf(' ');
                        if (index >= 0)
                        {
                            remainder = drawString.Substring(index + 1) + remainder;
                            drawString = drawString.Substring(0, index);
                        }
                    }

                    _drawTexts.Add(new LineData(drawString, textHeight, startingIndex, drawString.Length));

                    int lastIndexDraw = startingIndex + drawString.Length;
                    foreach (var pos in linkPositions)
                    {
                        int lastIndexPos = pos.index + pos.length;
                        if (lastIndexDraw >= pos.index && lastIndexDraw <= lastIndexPos || lastIndexPos >= startingIndex && lastIndexPos <= lastIndexDraw)
                        {
                            int sliceStart = Math.Max(startingIndex, pos.index);
                            int sliceEnd = Math.Min(pos.index + pos.length, lastIndexDraw);
                            int prevChars = sliceStart - startingIndex;
                            int sliceChars = sliceEnd - sliceStart;

                            var prevText = drawString.Substring(0, prevChars);
                            var linkText = drawString.Substring(prevChars, sliceChars);

                            float minX = font.MeasureString(prevText).X;
                            float maxX = minX + font.MeasureString(linkText).X;
                            float minY = position;
                            float maxY = minY + textHeight;
                        }
                    }

                    startingIndex += drawString.Length;

                    position += textHeight;
                    drawString = remainder;
                }
                while (drawString.Length > 0);
            }

            var parts = new LinkedList<TextPart>();
            foreach (var part in textParts)
            {
                var subparts = part.Text.Split('\n');
                if (subparts.Length > 0)
                {
                    for (int i = 0; i < subparts.Length; i++)
                    {
                        if (i != 0)
                            parts.AddLast(new NewLineTextPart());
                        var text = subparts[i];
                        parts.AddLast(part.Alter(text));
                    }
                }
                else
                {
                    parts.AddLast(part);
                }
            }



            float currentWidth = 0;
            var lineParts = new List<TextPart>();

            _textLines.Clear();
            while (parts.Count > 0)
            {
                var nextPart = parts.First.Value;
                parts.RemoveFirst();

                if (nextPart is NewLineTextPart)
                {
                    var textLine = new TextLine(currentWidth, textHeight, lineParts);
                    _textLines.Add(textLine);
                    lineParts = new List<TextPart>();
                    currentWidth = 0;
                }

                float partWidth = font.MeasureString(nextPart.Text).X;
                if (currentWidth + partWidth < space.Width)
                {
                    lineParts.Add(nextPart);
                    currentWidth += partWidth;
                }
                else
                {
                    string acceptedText = nextPart.Text;
                    string remainder = "";
                    while (currentWidth + font.MeasureString(acceptedText).X > space.Width)
                    {
                        remainder = acceptedText[^1] + remainder;
                        acceptedText = acceptedText[0..^1];
                    }
                    if (remainder.Length > 0)
                    {
                        int index = acceptedText.LastIndexOf(' ');
                        if (index >= 0)
                        {
                            remainder = acceptedText.Substring(index + 1) + remainder;
                            acceptedText = acceptedText.Substring(0, index);
                        }
                    }

                    if (acceptedText.Length > 0)
                    {
                        var acceptedPart = nextPart.Alter(acceptedText);
                        var remainingPart = nextPart.Alter(remainder);
                        lineParts.Add(acceptedPart);
                        parts.AddFirst(remainingPart);
                        currentWidth += font.MeasureString(acceptedText).X;
                    }
                    else
                    {
                        parts.AddFirst(nextPart);
                    }

                    var textLine = new TextLine(currentWidth, textHeight, lineParts);
                    _textLines.Add(textLine);
                    lineParts = new List<TextPart>();
                    currentWidth = 0;
                }
            }


            if (lineParts.Count > 0)
            {
                var textLine = new TextLine(currentWidth, textHeight, lineParts);
                _textLines.Add(textLine);
                lineParts = new List<TextPart>();
                currentWidth = 0;
            }

            int lineIndex = 0;
            float baseY = 0;
            linkDatas.Clear();
            foreach (var textLine in _textLines)
            {
                float baseX = 0;
                foreach (var part in textLine.Parts)
                {
                    if (part is FancyTextPart linkPart)
                    {
                        var minX = baseX;
                        var maxX = minX + part.Width;
                        var minY = baseY;
                        var maxY = minY + part.Height;

                        var topLeft = new Vector2(minX, minY);
                        var bottomRight = new Vector2(maxX, maxY);
                        linkDatas.Add(new LinkData(lineIndex, linkPart.Text, linkPart.Link, topLeft, bottomRight));

                    }
                    baseX += part.Width;
                }
                lineIndex++;
                baseY += textLine.Height;
            }


            height = position;
            heightNeedsRecalculating = false;
        }

        public override void Recalculate()
        {
            base.Recalculate();
            UpdateScrollbar();
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);

            //CalculatedStyle space = GetInnerDimensions();
            //float y = evt.MousePosition.Y - space.Y + Scrollbar.GetValue();
            //float position = 0f;

            //int lineIndex = 0;
            //foreach (var drawText in _drawTexts)
            //{
            //    position += drawText.height;
            //    if (y <= position)
            //    {
            //        //Console.Out.WriteLine("line" + drawText.Item1);
            //        OnLineClicked?.Invoke(this, new LineClickEventArgs(lineIndex, drawText.text));
            //        return;
            //    }
            //    lineIndex++;
            //}


            CalculatedStyle space = GetInnerDimensions();
            float x = evt.MousePosition.X - space.X;
            float y = evt.MousePosition.Y - space.Y + Scrollbar.GetValue();


            foreach (var linkData in linkDatas)
            {
                if (linkData.IsInBounds(x, y))
                {
                    OnLineClicked?.Invoke(this, new LineClickEventArgs(linkData.LineIndex, linkData.Text, linkData.Link));
                    return;
                }
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            PlayerInput.LockVanillaMouseScroll("Spellwright/UIMessageBox");
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (Scrollbar != null)
                Scrollbar.ViewPosition -= evt.ScrollWheelValue;
        }

        public void SetScrollbar(UIScrollbar scrollbar)
        {
            Scrollbar = scrollbar;
            UpdateScrollbar();
            heightNeedsRecalculating = true;
        }

        private void UpdateScrollbar()
        {
            Scrollbar?.SetView(GetInnerDimensions().Height, height);
        }
    }
}
