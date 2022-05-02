using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Spellwright.UI.Components.Args;
using Spellwright.UI.Components.TextBox;
using Spellwright.UI.Components.TextBox.TextParts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Spellwright.UI.Components
{
    internal class UIFormattedTextBox : UIPanel
    {
        protected UIScrollbar Scrollbar;

        private string text;
        private float height;
        private bool heightNeedsRecalculating;
        private List<TextLine> textLines = new();
        private List<LinkInfo> links = new();
        private LinkInfo hoveredLink = null;
        private float? newViewPosition = null;
        private bool IsSmartCursorWanted = false;

        public event EventHandler<LinkClickedEventArgs> OnLinkClicked;

        public UIFormattedTextBox()
        {
            text = "";
            ResetScrollbar();
        }

        public override void OnActivate()
        {
            base.OnActivate();
            heightNeedsRecalculating = true;
            IsSmartCursorWanted = Main.SmartCursorWanted;
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            Main.SmartCursorWanted = IsSmartCursorWanted;
        }

        public bool HasText()
        {
            return text.Length > 0;
        }

        public virtual string GetText()
        {
            return text;
        }

        public virtual void SetText(string text)
        {
            this.text = text;
            ResetScrollbar();
        }

        public float ViewPosition
        {
            get => Scrollbar?.ViewPosition ?? 0;
            set => newViewPosition = value;
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
                position = -Scrollbar.GetValue();

            foreach (var textLine in textLines)
            {
                if (position + textLine.Height > space.Height)
                    break;

                if (position >= 0)
                {
                    float previousPartsWidth = 0;
                    foreach (var part in textLine.Parts)
                    {
                        var color = part.GetColor(ForegroundColor);
                        if (hoveredLink != null && part is FormattedTextPart formattedPart && formattedPart.Link == hoveredLink.TextPart.Link)
                            //color = Color.DarkBlue;
                            color *= 1.5f;

                        Utils.DrawBorderString(spriteBatch, part.Text, new Vector2(space.X + previousPartsWidth, space.Y + position), color, 1f);
                        previousPartsWidth += part.Width;
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
                return;

            CalculatedStyle space = GetInnerDimensions();
            if (space.Width <= 0 || space.Height <= 0)
                return;

            DynamicSpriteFont font = FontAssets.MouseText.Value;

            var parser = new TextBoxParser();
            var textParts = parser.ParseText(text, font);
            textLines = parser.SplitTextByLines(textParts, space, font);
            links = parser.GenerateLinkInformation(textLines);

            float position = 0f;
            foreach (var textLine in textLines)
                position += textLine.Height;

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

            LinkInfo linkInfo = GetLinkUnderCursor(evt.MousePosition);
            if (linkInfo != null)
                OnLinkClicked?.Invoke(this, new LinkClickedEventArgs(linkInfo.LineIndex, linkInfo.Text, linkInfo.Link));
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            PlayerInput.LockVanillaMouseScroll("Spellwright/UIMessageBox");
        }

        public override void Update()
        {
            var mouseVector = Main.MouseScreen;
            hoveredLink = GetLinkUnderCursor(mouseVector);
            Main.SmartCursorWanted = hoveredLink == null;
        }

        private LinkInfo GetLinkUnderCursor(Vector2 mousePosition)
        {
            CalculatedStyle space = GetInnerDimensions();
            float x = mousePosition.X - space.X;
            float y = mousePosition.Y - space.Y + Scrollbar.GetValue();

            foreach (var linkInfo in links)
            {
                if (linkInfo.IsInBounds(x, y))
                    return linkInfo;
            }

            return null;
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
            if (newViewPosition != null)
            {
                Scrollbar.ViewPosition = newViewPosition.Value;
                newViewPosition = null;
            }
        }
    }
}
