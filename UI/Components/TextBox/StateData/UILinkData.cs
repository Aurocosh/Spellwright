using Microsoft.Xna.Framework;
using Spellwright.UI.Components.TextBox.TextParts;

namespace Spellwright.UI.Components.TextBox.StateData
{
    internal class UILinkData
    {
        public FormattedTextPart TextPart { get; }
        public int LineIndex { get; }
        public string Text { get; }
        public string Link { get; }

        public Vector2 TopLeft { get; }
        public Vector2 BottomRight { get; }

        public UILinkData(FormattedTextPart textPart, int lineIndex, string text, string link, Vector2 topLeft, Vector2 bottomRight)
        {
            TextPart = textPart;
            LineIndex = lineIndex;
            Text = text;
            Link = link;
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public bool IsInBounds(float x, float y)
        {
            return TopLeft.X <= x && x <= BottomRight.X && TopLeft.Y <= y && y <= BottomRight.Y;
        }

        public bool IsInBounds(Vector2 vector)
        {
            return TopLeft.X <= vector.X && vector.X <= BottomRight.X && TopLeft.Y <= vector.Y && vector.Y <= BottomRight.Y;
        }
    }
}
