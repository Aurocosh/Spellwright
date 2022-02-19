using Microsoft.Xna.Framework;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.GameContent;

namespace Spellwright.Spells.WarpSpells
{
    internal class OceanStepSpell : TeleportationSpell
    {
        public OceanStepSpell(string name, string incantation) : base(name, incantation)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            bool isOnLeftSide = player.position.X / 16f < Main.maxTilesX / 2;

            int teleportDestination = 0;
            if (spellData is OceanStepData oceanStepData)
                teleportDestination = oceanStepData.TeleportDestination;

            bool teleportToRightSide;
            if (teleportDestination == 0)
                teleportToRightSide = isOnLeftSide;
            else
                teleportToRightSide = teleportDestination == 2;

            Vector2 vector = Vector2.Zero;
            int crawlOffsetX = teleportToRightSide.ToDirectionInt();
            int startX = teleportToRightSide ? (Main.maxTilesX - 50) : 50;
            bool canTeleport = true;
            if (!TeleportHelpers.RequestMagicConchTeleportPosition(player, -crawlOffsetX, startX, out Point landingPoint))
            {
                canTeleport = false;
                startX = ((!teleportToRightSide) ? (Main.maxTilesX - 50) : 50);
                if (TeleportHelpers.RequestMagicConchTeleportPosition(player, crawlOffsetX, startX, out landingPoint))
                    canTeleport = true;
            }

            if (canTeleport)
                vector = landingPoint.ToWorldCoordinates(8f, 16f) - new Vector2(player.width / 2, player.height);

            Teleport(player, vector, canTeleport, 5);
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