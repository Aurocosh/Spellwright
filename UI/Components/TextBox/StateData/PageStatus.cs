namespace Spellwright.UI.Components.TextBox.TextData
{
    internal class PageStatus
    {
        public string Text { get; }
        public float ScrollPosition { get; set; }

        public PageStatus(string text, float scrollPosition = 0)
        {
            Text = text;
            ScrollPosition = scrollPosition;
        }
    }
}
