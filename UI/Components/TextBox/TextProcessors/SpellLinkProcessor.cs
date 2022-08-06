using Spellwright.Core.Links.Base;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.StateData;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.UI.Components.TextBox.TextProcessors
{
    internal class SpellLinkProcessor : ILinkProcessor
    {
        private readonly PageContentHandler linkHandler = new();
        private readonly PageMarkerProcessor markerHandler = new();

        public LinkTextResult Process(string linkText)
        {
            var linkData = LinkData.Parse(linkText);
            if (linkData != null)
            {
                var player = Main.LocalPlayer;
                var result = linkHandler.HandleLink(ref linkData, player);
                if (result != null)
                {
                    result = markerHandler.ReplaceMarkers(result, player);
                    string id = linkData.GetParameter("id") ?? linkData.Type;
                    return new LinkTextResult(id, result, linkText, linkData.ToString());
                }
            }

            return new LinkTextResult("Error", "Error occured while generating the page", linkText, linkText);
        }
    }
}
