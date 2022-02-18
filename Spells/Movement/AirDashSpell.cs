using Microsoft.Xna.Framework;
using Spellwright.Players;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.DataStructures;

namespace Spellwright.Spells.WarpSpells
{
    internal class AirDashSpell : Spell
    {
        public AirDashSpell(string name, string incantation) : base(name, incantation, SpellType.Cantrip)
        {
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