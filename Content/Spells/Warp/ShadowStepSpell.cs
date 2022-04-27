using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Extensions;
using Spellwright.Lib.Primitives;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Warp
{
    internal class ShadowStepSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 5;
            UseType = SpellType.Cantrip;
            teleportStyle = 1;
            resetVelocity = false;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.EoCShield, 1)
                .WithCost(ItemID.TeleportationPotion, 4);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 5);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
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

            Teleport(player, teleportPosition, canTeleport);
            return canTeleport;
        }
    }
}