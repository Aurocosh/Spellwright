using Spellwright.UI.Components.TextBox.Text;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class PageContentHandler
    {
        private static readonly Dictionary<string, PageHandler> linkHandlers = new();

        private static void registerHandler(PageHandler linkHandler) => linkHandlers[linkHandler.Type] = linkHandler;

        static PageContentHandler()
        {
            registerHandler(new SpellPageHandler());
            registerHandler(new PlayerStatusPageHandler());
            registerHandler(new KnownSpellsPageHandler());
            //registerHandler(new HomePageHandler());
            registerHandler(new SpellListPageHandler());
            registerHandler(new StaticPageHandler());
        }

        public string HandleLink(ref LinkData linkData, Player player)
        {
            if (linkData == null)
                return null;

            if (!linkHandlers.TryGetValue(linkData.Type, out var handler))
            {
                Spellwright.Instance.Logger.Error($"Link handler error. Unknown link type: {linkData.Type}");
                return null;
            }

            return handler.ProcessLink(ref linkData, player);
        }
    }
}
