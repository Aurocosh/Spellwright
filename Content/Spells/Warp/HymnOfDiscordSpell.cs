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
    internal class HymnOfDiscordSpell : TeleportationSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 10;
            UseType = SpellType.Cantrip;
            teleportStyle = 1;
            resetVelocity = false;
            useDelay = 0;
            canAutoReuse = false;

            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.ChaosElementalBanner, 1)
                .WithCost(ItemID.TeleportationPotion, 10);

            SpellCost = new SingleItemSpellCost(ModContent.ItemType<MythicalSpellReagent>(), 2);
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
                var worldPosition = point.ToWorldVector2() + playerAlignVector;
                if (Collision.SolidCollision(worldPosition, width, height))
                    continue;

                teleportPosition = worldPosition;
                canTeleport = true;
            }

            Teleport(player, teleportPosition, canTeleport);

            if (player.chaosState)
            {
                player.statLife -= player.statLifeMax2 / 7;
                var damageSource = PlayerDeathReason.ByOther(13);
                if (Main.rand.Next(2) == 0)
                    damageSource = PlayerDeathReason.ByOther(player.Male ? 14 : 15);

                if (player.statLife <= 0)
                    player.KillMe(damageSource, 1.0, 0);

                player.lifeRegenCount = 0;
                player.lifeRegenTime = 0;
            }

            player.AddBuff(BuffID.ChaosState, 360);

            return canTeleport;
        }
    }
}