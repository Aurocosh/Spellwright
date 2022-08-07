using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors.Base
{
    internal class PageMarkerProcessor
    {
        private static readonly Dictionary<string, MarkerProcessor> markerHandlers = new();

        private static void registerProcessor(MarkerProcessor markerProcessor) => markerHandlers[markerProcessor.Type] = markerProcessor;

        static PageMarkerProcessor()
        {
            registerProcessor(new Header1MarkerProcessor());
            registerProcessor(new Header2MarkerProcessor());
            registerProcessor(new LevelListMarkerProcessor());
            registerProcessor(new SpellLinkMarkerProcessor());
            registerProcessor(new StaticLinkMarkerProcessor());
            registerProcessor(new SpellTypeLinkMarkerProcessor());
            registerProcessor(new SpellModifierLinkMarkerProcessor());
        }

        public string ReplaceMarkers(string input, Player player)
        {
            var result = Regex.Replace(
                input,
                @"(#(?:\[[^\]]*\])?\{[^\}]*\}(?:\([^)]*\))?)",
                m => ReplaceMarker(m.Captures[0].Value, player)
            );

            return result;
        }

        private string ReplaceMarker(string markerText, Player player)
        {
            var markerData = MarkerData.Parse(markerText);
            if (markerData == null)
                return markerText;
            return ProcessMarker(markerData, player);

        }

        private string ProcessMarker(MarkerData markerData, Player player)
        {
            if (markerData == null)
                return null;

            if (!markerHandlers.TryGetValue(markerData.Type, out var handler))
            {
                Spellwright.Instance.Logger.Error($"Marker processor error. Unknown marker type: {markerData.Type}");
                return null;
            }

            return handler.ProcessMarker(markerData, player);
        }
    }
}
