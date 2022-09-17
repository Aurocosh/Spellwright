using Spellwright.Config;
using Spellwright.Content.Items.SpellTomes.Base;
using Spellwright.Content.Spells.Base;
using Spellwright.Lib;
using Spellwright.UI.Components.TextBox.MarkerProcessors.Base;
using Spellwright.UI.Components.TextBox.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Spellwright.Core.Markers
{
    internal class TomeSpellListMarkerProcessor : MarkerProcessor
    {
        public override string ProcessMarker(MarkerData markerData, Player player)
        {
            if (!ModContent.TryFind(Spellwright.Instance.Name, markerData.Id, out ModItem tomeItem))
                return $"Item {markerData.Id} is not found";
            if (tomeItem is not SpellTome spellTome)
                return $"Item {markerData.Id} is not a spell tome";

            var content = spellTome.GetContent();
            MultiValueDictionary<int, ModSpell> spellsByLevel = PrepareSpellList(content.Spells);

            var stringBuilder = new StringBuilder();
            int maxLevel = spellsByLevel.Keys.DefaultIfEmpty(0).Max();
            int limit = maxLevel + 1;
            for (int i = 0; i < limit; i++)
            {
                var spells = spellsByLevel.GetValues(i);
                if (spells.Count == 0)
                    continue;

                var levelWord = Spellwright.GetTranslation("General", "Level").Value;
                var levelHeader = $"{levelWord} {i}";

                stringBuilder.AppendLine(levelHeader);
                foreach (var spell in spells)
                {
                    var displayName = spell.DisplayName.GetTranslation(Language.ActiveCulture);
                    var line = new FormattedText(displayName).WithLink("Spell", spell.Name).ToString();
                    stringBuilder.AppendLine(line);
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private static MultiValueDictionary<int, ModSpell> PrepareSpellList(List<ModSpell> spells)
        {
            var spellsByLevel = new MultiValueDictionary<int, ModSpell>();
            foreach (var spell in spells)
            {
                if (!SpellwrightServerConfig.Instance.DisabledSpellIds.Contains(spell.Type))
                {
                    int spellLevel = spell.SpellLevel;
                    spellsByLevel.Add(spellLevel, spell);
                }
            }

            foreach (var item in spellsByLevel.Values)
                item.Sort(new SpellComparer());

            return spellsByLevel;
        }
    }
}
