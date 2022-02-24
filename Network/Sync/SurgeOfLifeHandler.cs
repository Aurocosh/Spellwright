using Spellwright.Content.Buffs.Spells;
using Spellwright.Network.Base;
using Terraria;

namespace Spellwright.Network.Handlers
{
    internal class SurgeOfLifeHandler : SyncPacketHandler<int>
    {
        protected override void HandleData(SyncData<int> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<SurgeOfLifePlayer>();
            modPlayer.LifeRegenValue = data.Value;
        }
    }
}
