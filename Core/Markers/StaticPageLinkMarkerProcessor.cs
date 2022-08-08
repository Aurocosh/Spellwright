using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class StaticLinkMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            string linkId = markerData.GetParameter("id", markerData.Id);
            return new FormattedText(markerData.Text).WithLink("Static").WithParam("id", linkId).ToString();
        }
    }
}
