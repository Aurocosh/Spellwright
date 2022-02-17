using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace Spellwright.Config
{
    class SpellwrightServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        [Label("Test")]
        [Tooltip("Test")]
        public bool Test { get; set; }

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            if (!Spellwright.IsPlayerServerOwner(Main.LocalPlayer))
            {
                message = "You are not the server owner";
                return false;
            }
            return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
        }
    }
}
