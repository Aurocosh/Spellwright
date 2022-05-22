using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.Sync
{
    internal class PermanentPlayerEffectsHandler : SyncPacketHandler<int[]>
    {
        protected override void HandleData(SyncData<int[]> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<SpellwrightBuffPlayer>();
            modPlayer.SetPermanentBuffs(data.Value);
        }
    }
}
