using Spellwright.Core.Links.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.Core.Links
{
    internal class StaticPageHandler : PageHandler
    {
        public StaticPageHandler()
        {
        }

        public override string ProcessLink(ref LinkData linkData, Player player)
        {
            var pageName = linkData.GetParameter("id");
            if (pageName == null)
            {
                Spellwright.Instance.Logger.Error($"Page id is not specified");
                return null;
            }

            var pageContent = GetTranslation(pageName).Value;
            if (pageContent.StartsWith("Mods.Spellwright"))
            {
                Spellwright.Instance.Logger.Error($"Static page is not found: {pageName}");
                return null;
            }
            return pageContent;
        }
    }
}
