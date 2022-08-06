using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors
{
    internal class SpellModifierLinkMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            string typeId = markerData.GetParameter("type", markerData.Id);
            return new FormattedText(markerData.Text).WithLink("SpellModifier").WithParam("type", typeId).ToString();
        }
    }
}
