using Spellwright.Common.Players;
using Spellwright.Network.Base;
using System.Collections.Generic;
using Terraria;
using static Spellwright.Common.Players.SpellwrightBuffPlayer;

namespace Spellwright.Network.Sync
{
    internal class EffectLevelHandler : SyncPacketHandler<List<BuffLevelData>>
    {
        protected override void HandleData(SyncData<List<BuffLevelData>> data, byte fromWho, bool fromServer)
        {
            var modPlayer = Main.player[data.PlayerId].GetModPlayer<SpellwrightBuffPlayer>();
            modPlayer.SetBuffLevels(data.Value);
        }
    }
}
