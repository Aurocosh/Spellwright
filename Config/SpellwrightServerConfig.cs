using Newtonsoft.Json;
using Spellwright.Core.Spells;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ModLoader.Config;

namespace Spellwright.Config
{
    [Label("$Mods.Spellwright.Config.ConfigName")]
    class SpellwrightServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static SpellwrightServerConfig Instance;

        [Range(1, 10000000)]
        [DefaultValue(10)]
        [Header("$Mods.Spellwright.Config.SpellReagentCosts.Header")]
        [Label("$Mods.Spellwright.Config.SpellReagentCosts.Common.Label")]
        [Tooltip("$Mods.Spellwright.Config.SpellReagentCosts.Common.Tooltip")]
        public int CommonReagentCost { get; set; }

        [Range(1, 10000000)]
        [DefaultValue(1000)]
        [Label("$Mods.Spellwright.Config.SpellReagentCosts.Rare.Label")]
        [Tooltip("$Mods.Spellwright.Config.SpellReagentCosts.Rare.Tooltip")]
        public int RareReagentCost { get; set; }

        [Range(1, 10000000)]
        [DefaultValue(100000)]
        [Label("$Mods.Spellwright.Config.SpellReagentCosts.Mythical.Label")]
        [Tooltip("$Mods.Spellwright.Config.SpellReagentCosts.Mythical.Tooltip")]
        public int MythicalReagentCost { get; set; }

        [DefaultValue(true)]
        [Header("$Mods.Spellwright.Config.SpellCosts.Header")]
        [Label("$Mods.Spellwright.Config.SpellCosts.CastCostEnabled.Label")]
        [Tooltip("$Mods.Spellwright.Config.SpellCosts.CastCostEnabled.Tooltip")]
        public bool CastCostEnabled { get; set; }

        [DefaultValue(true)]
        [Label("$Mods.Spellwright.Config.SpellCosts.UnlockCostEnabled.Label")]
        [Tooltip("$Mods.Spellwright.Config.SpellCosts.UnlockCostEnabled.Tooltip")]
        public bool UnlockCostEnabled { get; set; }

        [Header("$Mods.Spellwright.Config.Spells.Header")]
        [Label("$Mods.Spellwright.Config.Spells.DisabledSpells.Label")]
        [Tooltip("$Mods.Spellwright.Config.Spells.DisabledSpells.Tooltip")]
        public readonly HashSet<string> DisabledSpells = new();
        [JsonIgnore]
        public readonly HashSet<int> DisabledSpellIds = new();

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            if (!Spellwright.IsPlayerServerOwner(Main.LocalPlayer))
            {
                message = Spellwright.GetTranslation("Config", "Errors", "NotServerOwner").ToString();
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
