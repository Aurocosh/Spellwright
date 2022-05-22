namespace Spellwright.UI.Components.TextBox.StateData
{
    internal class PageStatus
    {
        public string LinkId { get; set; }
        public string LinkText { get; set; }
        public string PageText { get; set; }
        public float ScrollPosition { get; set; }

        public bool IsLink => LinkText.Length > 0;

        public PageStatus(string linkId, string linkText, float scrollPosition = 0)
        {
            LinkId = linkId;
            LinkText = linkText;
            PageText = "";
            ScrollPosition = scrollPosition;
        }

        public PageStatus(string pageText, float scrollPosition = 0)
        {
            LinkId = "";
            LinkText = "";
            PageText = pageText;
            ScrollPosition = scrollPosition;
        }
    }
}
