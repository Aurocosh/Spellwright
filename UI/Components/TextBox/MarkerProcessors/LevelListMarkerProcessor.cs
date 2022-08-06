using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.SpellRelated;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors
{
    internal class LevelListMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            var stringBuilder = new StringBuilder();

            AscendSpell ascendSpell = ModContent.GetInstance<AscendSpell>();
            int maxLevel = 10;
            int limit = maxLevel + 1;
            for (int i = 1; i < limit; i++)
            {
                var levelWord = Spellwright.GetTranslation("General", "Level").Value;
                var levelHeader = $"{levelWord} {i}";

                var cost = ascendSpell.GetLevelUpCost(i - 1);
                if (cost != null)
                {
                    var costDescritpion = cost.GetDescription(player, 0, SpellData.EmptyData);
                    levelHeader += $" - {costDescritpion}";
                }

                stringBuilder.Append(levelHeader);
                if (i != maxLevel)
                    stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
