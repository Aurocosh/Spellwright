using System;

namespace Spellwright.UI.Components.Args
{
    internal class LinkClickedEventArgs : EventArgs
    {
        public int LineIndex { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }

        public LinkClickedEventArgs(int lineIndex, string text, string link)
        {
            LineIndex = lineIndex;
            Text = text;
            Link = link;
        }
    }
}
