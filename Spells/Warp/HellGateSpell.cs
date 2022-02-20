using Microsoft.Xna.Framework;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using static Terraria.Player;

namespace Spellwright.Spells.WarpSpells
{
    internal class HellGateSpell : TeleportationSpell
    {
        public HellGateSpell(string name, string incantation) : base(name, incantation)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int centerX = Main.maxTilesX / 2;
            int idk = 100;
            int idkHalf = idk / 2;
            int teleportStartY = Main.UnderworldLayer + 20;
            int teleportRangeY = 80;
            var settings = new RandomTeleportationAttemptSettings
            {
                mostlySolidFloor = true,
                avoidAnyLiquid = true,
                avoidLava = true,
                avoidHurtTiles = true,
                avoidWalls = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = 30
            };

            bool canTeleport = false;
            Vector2 teleportPosition = player.CheckForGoodTeleportationSpot(ref canTeleport, centerX - idkHalf, idk, teleportStartY, teleportRangeY, settings);
            if (!canTeleport)
                teleportPosition = player.CheckForGoodTeleportationSpot(ref canTeleport, centerX - idk, idkHalf, teleportStartY, teleportRangeY, settings);
            if (!canTeleport)
                teleportPosition = player.CheckForGoodTeleportationSpot(ref canTeleport, centerX + idkHalf, idkHalf, teleportStartY, teleportRangeY, settings);

            Teleport(player, teleportPosition, canTeleport, 7);
            return true;
        }
    }

}