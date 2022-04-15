using Spellwright.Common.Players;
using Spellwright.Network.Base;
using Terraria;
using static Spellwright.Common.Players.SpellwrightBuffPlayer;

namespace Spellwright.Network.Sync.EffectLevels
{
    internal class SingleEffectLevelHandler : SyncPacketHandler<BuffLevelData>
    {
        protected override void HandleData(SyncData<BuffLevelData> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<SpellwrightBuffPlayer>();
            modPlayer.SetBuffLevel(data.Value);
        }
    }
}
