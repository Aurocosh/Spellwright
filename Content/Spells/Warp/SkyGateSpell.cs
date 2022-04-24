using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.Types;
using Spellwright.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace Spellwright.Content.Spells.Warp
{
    internal class SkyGateSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Cloud, 10)
                .WithCost(ItemID.TeleportationPotion, 10);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
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

            Teleport(player, teleportPosition, canTeleport);
            return canTeleport;
        }
    }
}