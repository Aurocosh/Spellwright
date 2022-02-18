using Microsoft.Xna.Framework;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

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

            int num = 50;
            int num2 = 50;
            int num3 = WorldGen.beachDistance - num - num2;
            if (teleportToRight)
                num3 = Main.maxTilesX - num3 - 1 - num2;
            else
                num3 -= num2 / 2;

            //new RandomTeleportationAttemptSettings
            //{
            //    avoidAnyLiquid = true,
            //    avoidHurtTiles = true,
            //    attemptsBeforeGivingUp = 1000,
            //    maximumFallDistanceFromOrignalPoint = 300
            //};

            Vector2 vector = Vector2.Zero;
            int crawlOffsetX = teleportToRight.ToDirectionInt();
            int startX = teleportToRight ? (Main.maxTilesX - 50) : 50;
            bool foundPlaceToTeleport = true;
            if (!TeleportHelpers.RequestMagicConchTeleportPosition(player, -crawlOffsetX, startX, out Point landingPoint))
            {
                foundPlaceToTeleport = false;
                startX = ((!teleportToRight) ? (Main.maxTilesX - 50) : 50);
                if (TeleportHelpers.RequestMagicConchTeleportPosition(player, crawlOffsetX, startX, out landingPoint))
                    foundPlaceToTeleport = true;
            }

            if (foundPlaceToTeleport)
                vector = landingPoint.ToWorldCoordinates(8f, 16f) - new Vector2(player.width / 2, player.height);

            if (foundPlaceToTeleport)
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

            return true;
        }

        public override bool ProcessExtraData(SpellStructure structure, out SpellData spellData)
        {
            int teleportDestination = 0;
            if (structure.Argument.Length > 0)
            {
                var destination = structure.Argument.ToLower();
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

            spellData = new OceanStepData(structure, teleportDestination);
            return true;
        }
    }

    internal sealed class OceanStepData : SpellData
    {
        public int TeleportDestination { get; } // 0 - not set, 1 - left, 2 - right

        public OceanStepData(SpellStructure structure, int teleportDestination) : base(structure)
        {
            TeleportDestination = teleportDestination;
        }
    }
}