using Microsoft.Xna.Framework;
using Spellwright.Extensions;
using Spellwright.Lib.Primitives;
using Spellwright.Spells.Base;
using Spellwright.Spells.SpellExtraData;
using Terraria;
using Terraria.DataStructures;

namespace Spellwright.Spells.WarpSpells
{
    internal class ShadowStepSpell : TeleportationSpell
    {
        public ShadowStepSpell(string name, string incantation) : base(name, incantation, SpellType.Cantrip)
        {
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IProjectileSource source, Vector2 position, Vector2 direction)
        {
            var center = player.Center;
            var mousePos = Main.MouseWorld;

            var centerPoint = center.ToGridPoint();
            var mousePoint = mousePos.ToGridPoint();

            bool canTeleport = false;
            var teleportPosition = center;
            var teleportLine = new ThinLine(centerPoint, mousePoint);

            int width = player.width;
            int height = player.height;
            var playerAlignVector = new Vector2(-width / 2 + 8, -height);
            foreach (var point in teleportLine)
            {
                Tile tile = Main.tile[point.X, point.Y];
                if (WorldGen.SolidTile(tile))
                    break;

                var worldPosition = point.ToWorldVector2() + playerAlignVector;
                if (Collision.SolidCollision(worldPosition, width, height))
                    continue;

                teleportPosition = worldPosition;
                canTeleport = true;
            }

            Teleport(player, teleportPosition, canTeleport, 1);
            return true;
        }
    }
}