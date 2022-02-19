﻿using Microsoft.Xna.Framework;
using Spellwright.Spells.Base;
using Terraria;
using Terraria.ID;

namespace Spellwright.Spells.WarpSpells
{
    internal abstract class TeleportationSpell : Spell
    {
        public TeleportationSpell(string name, string incantation, SpellType type = SpellType.Invocation) : base(name, incantation, type)
        {
        }

        protected static void Teleport(Player player, Vector2 position, bool canTeleport, int teleportStyle)
        {
            int noTeleportSign = 0;
            if (!canTeleport)
            {
                noTeleportSign = 1;
                position = player.position;
            }

            player.Teleport(position, teleportStyle);
            player.velocity = Vector2.Zero;
            if (Main.netMode == NetmodeID.Server)
            {
                RemoteClient.CheckSection(player.whoAmI, player.position);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position.X, position.Y, teleportStyle, noTeleportSign);
            }
        }
    }
}