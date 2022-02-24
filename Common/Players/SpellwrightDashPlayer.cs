using Microsoft.Xna.Framework;
using Spellwright.Network;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellwright.Common.Players
{
    public class SpellwrightDashPlayer : ModPlayer
    {
        public int DashTimer = 0;

        public void Dash(Vector2 velocity, int dashDuration)
        {
            if (!CanUseDash())
                return;

            Player.velocity = velocity;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Player.whoAmI);

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

        public override void clientClone(ModPlayer clientClone)
        {
            var clone = clientClone as SpellwrightDashPlayer;
            clone.DashTimer = DashTimer;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            int playerId = Player.whoAmI;
            ModNetHandler.dashPlayerTimerSync.Sync(toWho, playerId, playerId, DashTimer);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightDashPlayer;
            if (clone.DashTimer < DashTimer)
                ModNetHandler.dashPlayerTimerSync.Sync(Player.whoAmI, DashTimer);
        }
    }
}
