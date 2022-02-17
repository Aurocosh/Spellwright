using Microsoft.Xna.Framework;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using static Terraria.Player;

namespace Spellwright.Spells.WarpSpells
{
    internal class OceanStepSpell : Spell
    {
        public OceanStepSpell(string name, string incantation) : base(name, incantation, SpellType.Invocation)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            bool isOnLeftSide = player.position.X / 16f < Main.maxTilesX / 2;

            int teleportDestination = 0;
            if (spellData is OceanStepData oceanStepData)
                teleportDestination = oceanStepData.TeleportDestination;

            bool teleportToRight;
            if (teleportDestination == 0)
                teleportToRight = isOnLeftSide;
            else
                teleportToRight = teleportDestination == 2;

            bool flag2 = false;
            int num = 50;
            int num2 = 50;
            int num3 = WorldGen.beachDistance - num - num2;
            if (teleportToRight)
                num3 = Main.maxTilesX - num3 - 1 - num2;
            else
                num3 -= num2 / 2;

            new RandomTeleportationAttemptSettings
            {
                avoidAnyLiquid = true,
                avoidHurtTiles = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = 300
            };

            Vector2 vector = Vector2.Zero;
            int crawlOffsetX = teleportToRight.ToDirectionInt();
            int startX = teleportToRight ? (Main.maxTilesX - 50) : 50;
            flag2 = true;
            if (!TeleportHelpers.RequestMagicConchTeleportPosition(player, -crawlOffsetX, startX, out Point landingPoint))
            {
                flag2 = false;
                startX = ((!teleportToRight) ? (Main.maxTilesX - 50) : 50);
                if (TeleportHelpers.RequestMagicConchTeleportPosition(player, crawlOffsetX, startX, out landingPoint))
                    flag2 = true;
            }

            if (flag2)
                vector = landingPoint.ToWorldCoordinates(8f, 16f) - new Vector2(player.width / 2, player.height);

            if (flag2)
            {
                Vector2 newPos = vector;
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


            //for (int d = 0; d < 70; d++)
            //    Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default, 1.5f);

            return true;
        }

        public override bool ProcessExtraData(string argument, out SpellData spellData)
        {
            int teleportDestination = 0;
            if (argument.Length > 0)
            {
                var destination = argument.ToLower();
                if (destination == "west" || destination == "left")
                {
                    teleportDestination = 1;
                }
                else if (destination == "east" || destination == "right")
                {
                    teleportDestination = 2;
                }
                else
                {
                    spellData = null;
                    return false;
                }
            }

            spellData = new OceanStepData(teleportDestination);
            return true;
        }
    }
}