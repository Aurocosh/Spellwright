using Spellwright.Common.Players;
using Terraria;

namespace Spellwright.Network
{
    internal class DashPlayerTimerHandler : IntSyncPacketHandler
    {
        protected override void HandleData(int data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[fromWho].GetModPlayer<SpellwrightDashPlayer>();
            modPlayer.DashTimer = data;
        }
    }
}
