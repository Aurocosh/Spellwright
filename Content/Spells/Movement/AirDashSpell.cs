using Microsoft.Xna.Framework;
using Spellwright.Common.Players;
using Spellwright.Content.Spells.Base;
using Spellwright.Content.Spells.Base.SpellCosts.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Spellwright.Content.Spells.Movement
{
    internal class AirDashSpell : ModSpell
    {
        public override void SetStaticDefaults()
        {
            SpellLevel = 3;
            UseType = SpellType.Cantrip;
            useDelay = 80;
            useSound = SoundID.DoubleJump;
            UnlockCost = new SingleItemSpellCost(ItemID.Feather, 30);
        }

        public override bool Cast(Player player, int playerLevel, SpellData spellData, IEntitySource source, Vector2 position, Vector2 direction)
        {
            SpellwrightDashPlayer dashPlayer = player.GetModPlayer<SpellwrightDashPlayer>();
            if (!dashPlayer.CanUseDash())
                return false;

            Vector2 velocity = Main.MouseWorld - player.Center;
            velocity.Normalize();
            velocity *= 14;

            dashPlayer.Dash(velocity, 40);

            return true;
        }
    }
}