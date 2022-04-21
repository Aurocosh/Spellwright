using Microsoft.Xna.Framework;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using static Terraria.Player;

namespace Spellwright.Content.Spells.Warp
{
    internal class DungeonGateSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 6;
            UseType = SpellType.Invocation;
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var dungeonPoint = new Point(Main.dungeonX, Main.dungeonY);
            var startX = dungeonPoint.X - 30;
            var startY = dungeonPoint.Y - 30;
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
            Vector2 teleportPosition = player.CheckForGoodTeleportationSpot(ref canTeleport, startX, 60, startY, 60, settings);

            Teleport(player, teleportPosition, canTeleport);
            return canTeleport;
        }
    }
}