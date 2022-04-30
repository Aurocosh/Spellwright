using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
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
    internal class GraveGateSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 4;
            UseType = SpellType.Invocation;
            teleportStyle = 7;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Gravestone, 10)
                .WithCost(ItemID.TeleportationPotion, 5);

            CastCost = new ReagentSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData)
        {
            var spellPlayer = player.GetModPlayer<SpellwrightPlayer>();
            var position = spellPlayer.LastDeathPoint;
            if (position == Point.Zero)
            {
                var message = Spellwright.GetTranslation("Spells", Name, "NoDeathPoint");
                Main.NewText(message, Color.Orange);
                return false;
            }

            var startX = position.X - 40;
            var startY = position.Y - 40;
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
            Vector2 teleportPosition = player.CheckForGoodTeleportationSpot(ref canTeleport, startX, 80, startY, 80, settings);

            Teleport(player, teleportPosition, canTeleport);

            if (canTeleport)
                spellPlayer.LastDeathPoint = Point.Zero;
            return canTeleport;
        }
    }
}