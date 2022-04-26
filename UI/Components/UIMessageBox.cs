using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
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
        private readonly List<Tuple<string, float>> _drawTexts = new();

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
            foreach (var drawText in _drawTexts)
            {
                if (position + drawText.Item2 > space.Height)
                    break;
                if (position >= 0)
                    Utils.DrawBorderString(spriteBatch, drawText.Item1, new Vector2(space.X, space.Y + position), Color.White, 1f);
                position += drawText.Item2;
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
            float position = 0f;
            float textHeight = font.MeasureString("A").Y;
            foreach (string line in text.Split('\n'))
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
                    _drawTexts.Add(new Tuple<string, float>(drawString, textHeight));
                    position += textHeight;
                    drawString = remainder;
                }
                while (drawString.Length > 0);
            }
            height = position;
            heightNeedsRecalculating = false;
        }

        public override void Recalculate()
        {
            base.Recalculate();
            UpdateScrollbar();
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
