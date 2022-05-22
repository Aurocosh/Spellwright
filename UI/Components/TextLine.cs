using Spellwright.UI.Components.TextBox.TextParts;
using System.Collections.Generic;

namespace Spellwright.UI.Components
{
    internal class TextLine
    {
        public float Width { get; }
        public float Height { get; }
        public List<ITextPart> Parts { get; }

        public TextLine(float width, float height, List<ITextPart> parts)
        {
            Width = width;
            Height = height;
            Parts = parts;
        }
    }
}
