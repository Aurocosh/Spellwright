using Newtonsoft.Json;
using Spellwright.Core.Spells;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace Spellwright.Config
{
    class SpellwrightServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static SpellwrightServerConfig Instance;

        [Range(1, 10000000)]
        [DefaultValue(10)]
        [Header("$Mods.Spellwright.Config.SpellReagentCosts.Header")]
        public int CommonReagentCost { get; set; }

        [Range(1, 10000000)]
        [DefaultValue(1000)]
        public int RareReagentCost { get; set; }

        [Range(1, 10000000)]
        [DefaultValue(100000)]
        public int MythicalReagentCost { get; set; }

        [DefaultValue(true)]
        [Header("$Mods.Spellwright.Config.SpellCosts.Header")]
        public bool CastCostEnabled { get; set; }

        [DefaultValue(true)]
        public bool UnlockCostEnabled { get; set; }

        [Header("$Mods.Spellwright.Config.Spells.Header")]
        public readonly HashSet<string> DisabledSpells = new();
        [JsonIgnore]
        public readonly HashSet<int> DisabledSpellIds = new();

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
        {
            if (!Spellwright.IsPlayerServerOwner(Main.LocalPlayer))
            {
                message = NetworkText.FromLiteral(Spellwright.GetTranslation("Config", "Errors", "NotServerOwner").ToString());
                return false;
            }

            return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
        }

        public override void OnChanged()
        {
            ReloadDisabledSpells();
        }

        private void ReloadDisabledSpells()
        {
            DisabledSpellIds.Clear();
            foreach (string spellIncantation in DisabledSpells)
            {
                var incantation = Regex.Replace(spellIncantation, @"\s+", " ").Trim();
                var spell = SpellLibrary.GetSpellByIncantation(incantation);
                if (spell != null)
                    DisabledSpellIds.Add(spell.Type);
            }
        }
    }
}
