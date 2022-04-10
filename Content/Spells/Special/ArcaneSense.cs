using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Terraria;

namespace Spellwright.Content.Spells.Special
{
    internal class ArcaneSense : ModSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            if (spellData == null)
                return false;

            var modPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var message = Spellwright.GetTranslation("Generic", "LevelDescription") + $": {modPlayer.PlayerLevel}";

            Main.NewText(message);

            return true;
        }
    }
}
