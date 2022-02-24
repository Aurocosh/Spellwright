using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.Handlers
{
    internal class DashPlayerTimerHandler : SyncPacketHandler<int>
    {
        protected override void HandleData(SyncData<int> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<SpellwrightDashPlayer>();
            modPlayer.DashTimer = data.Value;
        }
    }
}
