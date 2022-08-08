using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.Core.Markers
{
    internal class SpellTypeLinkMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            string typeId = markerData.GetParameter("type", markerData.Id);
            return new FormattedText(markerData.Text).WithLink("SpellType").WithParam("type", typeId).ToString();
        }
    }
}
