using Microsoft.Xna.Framework;
using Spellwright.Players;
using Spellwright.Spells.Base;
using Terraria;
using Terraria.DataStructures;

namespace Spellwright.Spells.WarpSpells
{
    internal class FlashStepSpell : Spell
    {
        public FlashStepSpell(string name, string incantation) : base(name, incantation, SpellType.Cantrip)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            float playerX = player.position.X;
            float mouseX = Main.MouseWorld.X;

            int dashDirection = mouseX > playerX ? 1 : -1;
            var newVelocity = new Vector2(dashDirection * 20, 0);

            dashPlayer.Dash(newVelocity, 60);

            return true;
        }
    }
}