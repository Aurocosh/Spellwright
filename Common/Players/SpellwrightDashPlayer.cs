using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Spellwright.Common.Players
{
    public class SpellwrightDashPlayer : ModPlayer
    {
        public bool isDashing = false;
        public int DashTimer = 0;

        public void Dash(Vector2 velocity, int dashDuration)
        {
            if (!CanUseDash())
                return;

            Player.velocity = velocity;

            isDashing = true;
            DashTimer = dashDuration;
        }

        public override void PreUpdateMovement()
        {
            if (DashTimer > 0)
            {
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;

                DashTimer--;
            }
        }

        public bool CanUseDash()
        {
            return DashTimer == 0
                //&& Player.dashType == 0 // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                //&& !Player.setSolar // player isn't wearing solar armor
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }
    }
}
