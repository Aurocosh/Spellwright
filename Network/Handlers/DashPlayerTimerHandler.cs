using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.Handlers
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
