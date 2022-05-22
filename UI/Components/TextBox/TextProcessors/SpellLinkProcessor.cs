using Spellwright.Core.Links.Base;
using Spellwright.UI.Components.TextBox.StateData;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.UI.Components.TextBox.TextProcessors
{
    internal class SpellLinkProcessor : ILinkProcessor
    {
        private readonly PageContentHandler linkHandler = new();

        public LinkTextResult Process(string linkText)
        {
            var linkData = LinkData.Parse(linkText);
            if (linkData != null)
            {
                var result = linkHandler.HandleLink(ref linkData, Main.LocalPlayer);
                if (result != null)
                {
                    string id = linkData.GetParameter("id") ?? linkData.Type;
                    return new LinkTextResult(id, result, linkText, linkData.ToString());
                }
            }

            return new LinkTextResult("Error", "Error occured while generating the page", linkText, linkText);
        }
    }
}
