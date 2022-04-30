using Microsoft.Xna.Framework;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Content.Spells.Base.SpellCosts.Reagent;
using Spellwright.Content.Spells.Base.Types;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace Spellwright.Content.Spells.Warp
{
    internal class HellGateSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
            teleportStyle = 7;
            castSound = SoundID.Item6;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Hellstone, 60)
                .WithCost(ItemID.TeleportationPotion, 1);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
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

            Teleport(player, teleportPosition, canTeleport);
            return canTeleport;
        }
    }

}