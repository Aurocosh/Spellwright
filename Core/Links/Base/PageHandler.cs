using Spellwright.UI.Components.TextBox.Text;
using Terraria;
using Terraria.Localization;

namespace Spellwright.Core.Links
{
    internal abstract class PageHandler
    {
        public string Name => GetType().Name;
        public string Type { get; }

        public abstract string ProcessLink(ref LinkData linkData, Player player);

        protected PageHandler()
        {
            var type = Name;
            if (type.EndsWith("PageHandler"))
                type = type.Substring(0, type.LastIndexOf("PageHandler"));
            Type = type;
        }

        protected LocalizedText GetTranslation(string key) => Spellwright.GetTranslation("PageHandler", Type, key);
        protected FormattedText GetFormText(string key) => new FormattedText(GetTranslation(key).Value);
    }
}
