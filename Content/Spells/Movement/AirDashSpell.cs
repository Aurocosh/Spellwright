using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Terraria;
using Terraria.DataStructures;

namespace Spellwright.Content.Spells.Movement
{
    internal class AirDashSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            UseType = SpellType.Cantrip;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            Vector2 velocity = Main.MouseWorld - player.Center;
            velocity.Normalize();
            velocity *= 14;

            dashPlayer.Dash(velocity, 40);

            return true;
        }
    }
}