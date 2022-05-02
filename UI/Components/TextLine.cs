using Spellwright.UI.Components.TextBox.TextParts;
using System.Collections.Generic;

namespace Spellwright.UI.Components.TextBox
{
    internal class TextLine
    {
        public float Width { get; }
        public float Height { get; }
        public List<TextPart> Parts { get; }

        public TextLine(float width, float height, List<TextPart> parts)
        {
            Width = width;
            Height = height;
            Parts = parts;
        }
    }
}
