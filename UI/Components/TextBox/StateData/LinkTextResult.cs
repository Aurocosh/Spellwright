namespace Spellwright.UI.Components.TextBox.StateData
{
    internal class LinkTextResult
    {
        public string LinkId { get; }
        public string Content { get; }
        public string OriginalLink { get; }
        public string CorrectedLink { get; }

        public LinkTextResult(string linkId, string content, string originalLink, string correctedLink)
        {
            LinkId = linkId;
            Content = content;
            OriginalLink = originalLink;
            CorrectedLink = correctedLink;
        }
    }
}
