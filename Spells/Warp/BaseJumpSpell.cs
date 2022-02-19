using Microsoft.Xna.Framework;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using static Terraria.Player;

namespace Spellwright.Spells.WarpSpells
{
    internal class BaseJumpSpell : Spell
    {
        public BaseJumpSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int rangeX = 10;
            int rangeY = 20;
            int startX = (int)(player.position.X / 16f) - rangeX / 2;
            int startY = 10;

            var settings = new RandomTeleportationAttemptSettings
            {
                mostlySolidFloor = false,
                avoidAnyLiquid = true,
                avoidLava = true,
                avoidHurtTiles = true,
                avoidWalls = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = -1
            };

            bool canTeleport = false;
            Vector2 teleportPosition = UtilPlayer.CheckForGoodTeleportationSpot(ref canTeleport, player, startX, rangeX, startY, rangeY, settings);

            if (canTeleport)
            {
                Vector2 newPos = teleportPosition;
                player.Teleport(newPos, 5);
                player.velocity = Vector2.Zero;
                if (Main.netMode == NetmodeID.Server)
                {
                    RemoteClient.CheckSection(player.whoAmI, player.position);
                    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, newPos.X, newPos.Y, 5);
                }
            }
            else
            {
                Vector2 position = player.position;
                player.Teleport(position, 5);
                player.velocity = Vector2.Zero;
                if (Main.netMode == NetmodeID.Server)
                {
                    RemoteClient.CheckSection(player.whoAmI, player.position);
                    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position.X, position.Y, 5, 1);
                }
            }

            return true;
        }
    }
}