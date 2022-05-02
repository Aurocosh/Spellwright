using System;

namespace Spellwright.UI.Components.Args
{
    internal class LineClickEventArgs : EventArgs
    {
        public int LineIndex { get; set; }
        public string LineText { get; set; }
        public string LinkText { get; set; }

        public LineClickEventArgs(int lineIndex, string lineText, string linkText)
        {
            LineIndex = lineIndex;
            LineText = lineText;
            LinkText = linkText;
        }
    }
}
