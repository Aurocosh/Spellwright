using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Items.Reagents;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellwright.Content.Spells.Movement
{
    internal class FlashStepSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 2;
            UseType = SpellType.Cantrip;

            SpellCost = new ReagentSpellCost(ModContent.ItemType<RareSpellReagent>(), 1);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            float playerX = player.Center.X;
            float mouseX = Main.MouseWorld.X;

            int dashDirection = mouseX > playerX ? 1 : -1;
            var newVelocity = new Vector2(dashDirection * 20, 0);

            dashPlayer.Dash(newVelocity, 60);

            return true;
        }
    }
}