using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

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
            int noTeleportSign = 0;
            if (!canTeleport)
            {
                noTeleportSign = 1;
                position = player.position;
            }

            player.Teleport(position, teleportStyle);
            if (resetVelocity)
                player.velocity = Vector2.Zero;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position.X, position.Y, teleportStyle, noTeleportSign);
        }
    }
}