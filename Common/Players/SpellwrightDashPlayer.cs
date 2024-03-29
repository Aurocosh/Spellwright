using Microsoft.Xna.Framework;
using Spellwright.Network.Sync;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Common.Players
{
    public class SpellwrightDashPlayer : ModPlayer
    {
        public int DashTimer = 0;
        public int NoGravityTimer = 0;

        public void Dash(Vector2 velocity, int dashDuration)
        {
            if (!CanUseDash())
                return;

            Player.velocity = velocity;
            //if (Main.netMode == NetmodeID.MultiplayerClient)
            //    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Player.whoAmI);

            DashTimer = dashDuration;
        }

        public void DisableGraviry(int duration)
        {
            NoGravityTimer = duration;
        }

        public override void PreUpdateMovement()
        {
            if (DashTimer > 0)
            {
                //Player.eocDash = DashTimer;
                Player.dashType = 10;
                Player.armorEffectDrawShadowEOCShield = true;
                DashTimer--;
            }

            if (NoGravityTimer > 0)
            {
                Player.vortexDebuff = true;
                NoGravityTimer--;
            }
        }

        public bool CanUseDash()
        {
            return DashTimer == 0
                //&& Player.dashType == 0 // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                //&& !Player.setSolar // player isn't wearing solar armor
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }

        public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
        {
            var clone = clientClone as SpellwrightDashPlayer;
            clone.DashTimer = DashTimer;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            new DashPlayerTimerSyncAction(Player.whoAmI, toWho, DashTimer).Execute();
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var clone = clientPlayer as SpellwrightDashPlayer;
            if (clone.DashTimer < DashTimer)
                new DashPlayerTimerSyncAction(Player.whoAmI, DashTimer).Execute();
        }
    }
}
