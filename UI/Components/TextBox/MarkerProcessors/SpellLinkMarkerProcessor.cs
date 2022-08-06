using Microsoft.Xna.Framework;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using Terraria;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors
{
    internal class SpellLinkMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            string spellId = markerData.GetParameter("name", markerData.Id) + "Spell";
            return new FormattedText(markerData.Text, Color.DarkGoldenrod).WithLink("Spell").WithParam("name", spellId).ToString();
        }
    }
}
