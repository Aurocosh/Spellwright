using Microsoft.Xna.Framework;
using Spellwright.Util;
using Terraria;

namespace Spellwright.Content.Spells.Base.Types
{
    internal abstract class TeleportationSpell : ModSpell
    {
        protected int teleportStyle;
        protected bool resetVelocity;

        public TeleportationSpell()
        {
            UseType = SpellType.Invocation;
            resetVelocity = true;
            teleportStyle = 5;
        }

        protected void Teleport(Player player, Vector2 position, bool canTeleport)
        {
            UtilPlayer.Teleport(player, position, canTeleport, teleportStyle, resetVelocity);
        }
    }
}