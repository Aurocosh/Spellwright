using Microsoft.Xna.Framework;

namespace Spellwright.UI.Components.TextBox
{
    internal class LinkInfo
    {
        public int LineIndex { get; }
        public string Text { get; }
        public string Link { get; }

        public Vector2 TopLeft { get; }
        public Vector2 BottomRight { get; }

        public LinkInfo(int lineIndex, string text, string link, Vector2 topLeft, Vector2 bottomRight)
        {
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
