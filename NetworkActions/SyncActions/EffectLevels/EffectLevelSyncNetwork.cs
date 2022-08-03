using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Spellwright.Common.Players.SpellwrightBuffPlayer;

namespace Spellwright.Network.Sync.EffectLevels
{
    [Serializable]
    internal class EffectLevelSyncNetwork : SyncNetworkAction
    {
        public List<BuffLevelData> BuffLevels { get; set; }

        public EffectLevelSyncNetwork(int senderId, IEnumerable<BuffLevelData> buffLevels) : base(senderId)
        {
            BuffLevels = buffLevels.ToList();
        }

        public EffectLevelSyncNetwork(int senderId, int receiverId, IEnumerable<BuffLevelData> buffLevels) : base(senderId, receiverId)
        {
            BuffLevels = buffLevels.ToList();
        }

        public override void DoAction()
        {
            var modPlayer = SenderPlayer.GetModPlayer<SpellwrightBuffPlayer>();
            modPlayer.SetBuffLevels(BuffLevels);
        }
    }
}
