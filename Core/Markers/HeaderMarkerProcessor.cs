using Microsoft.Xna.Framework;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using System.Collections.Generic;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class HeaderMarkerProcessor : MarkerProcessor
    {
        private static readonly Dictionary<int, Color> colorMap = new() {
            { 1, Color.Purple },
            { 2, Color.DarkGray }
        };

        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            int headerId = markerData.GetId(1);

            if (!colorMap.TryGetValue(headerId, out var color))
                color = Color.White;

            return new FormattedText(markerData.Text).WithColor(color).ToString();
        }
    }
}
