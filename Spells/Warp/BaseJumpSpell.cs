using Microsoft.Xna.Framework;
using Spellwright.Spells.SpellExtraData;
using Spellwright.Util;
using Terraria;
using static Terraria.Player;

namespace Spellwright.Spells.WarpSpells
{
    internal class BaseJumpSpell : TeleportationSpell
    {
        public BaseJumpSpell(string name, string incantation) : base(name, incantation)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            int rangeX = 10;
            int rangeY = 20;
            int startX = (int)(player.position.X / 16f) - rangeX / 2;
            int startY = 60;

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

            Teleport(player, teleportPosition, canTeleport, 5);
            return true;
        }
    }
}