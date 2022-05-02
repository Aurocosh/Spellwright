using System.Collections.Generic;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class LinkHandler
    {
        private static readonly Dictionary<string, ILinkHandler> linkHandlers = new();

        private static void registerHandler(ILinkHandler linkHandler) => linkHandlers[linkHandler.Type] = linkHandler;

        static LinkHandler()
        {
            registerHandler(new SpellLinkHandler());
            registerHandler(new PlayerStatusLinkHandler());
            registerHandler(new KnownSpellsLinkHandler());
            registerHandler(new HomeLinkHandler());
            registerHandler(new UsableSpellsLinkHandler());
        }

        public string HandleLink(string linkText, Player player)
        {
            var linkParts = linkText.Split(':', 2);
            if (linkParts.Length < 1)
            {
                Spellwright.Instance.Logger.Error($"Link handler error. Invalid link format: {linkText}");
                return null;
            }

            var linkType = linkParts[0].ToLower();
            if (!linkHandlers.TryGetValue(linkType, out var handler))
            {
                Spellwright.Instance.Logger.Error($"Link handler error. Unknown link type: {linkType}");
                return null;
            }

            var linkValue = linkParts.Length > 1 ? linkParts[1] : "";
            return handler.ProcessLink(linkValue, player);
        }
    }
}
