using Spellwright.UI.Components.TextBox.Text;
using Terraria;
using Terraria.Localization;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors.Base
{
    internal abstract class MarkerProcessor
    {
        public string Name => GetType().Name;
        public string Type { get; }

        public abstract string ProcessMarker(MarkerData markerData, Player player);

        protected MarkerProcessor()
        {
            var type = Name;
            if (type.EndsWith("MarkerProcessor"))
                type = type.Substring(0, type.LastIndexOf("MarkerProcessor"));
            Type = type;
        }
        protected LocalizedText GetTranslation(string key) => Spellwright.GetTranslation("MarkerProcessor", Type, key);
        protected FormattedText GetFormText(string key) => new FormattedText(GetTranslation(key).Value);
    }
}
