using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.Handlers
{
    internal class PlayerLevelHandler : SyncPacketHandler<int>
    {
        protected override void HandleData(SyncData<int> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<SpellwrightPlayer>();
            modPlayer.PlayerLevel = data.Value;
        }
    }
}
