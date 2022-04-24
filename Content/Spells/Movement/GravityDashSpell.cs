using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Spellwright.Util;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Movement
{
    internal class GravityDashSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 8;
            UseType = SpellType.Cantrip;

            UnlockCost = new SingleItemSpellCost(ItemID.Feather, 30);
            UnlockCost = new MultipleItemSpellCost()
                .WithCost(ItemID.Feather, 30)
                .WithCost(ItemID.GravitationPotion, 5);

            CastCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 5);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            Vector2 velocity = Main.MouseWorld - player.Center;
            velocity.Normalize();
            velocity *= 14;

            var dashTime = UtilTime.SecondsToTicks(1);
            dashPlayer.Dash(velocity, dashTime);
            dashPlayer.DisableGraviry(dashTime);

            return true;
        }
    }
}