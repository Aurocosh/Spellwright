using System;

namespace Spellwright.UI.Components.Args
{
    internal class PageChangedEventArgs : EventArgs
    {
        public string Text { get; set; }

        public PageChangedEventArgs(string text)
        {
            Text = text;
        }
    }
}
