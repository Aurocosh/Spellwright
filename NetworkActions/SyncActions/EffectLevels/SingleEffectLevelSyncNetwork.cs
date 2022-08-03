using Spellwright.Common.Players;
using Spellwright.Network.NetworkActions;
using System;
using static Spellwright.Common.Players.SpellwrightBuffPlayer;

namespace Spellwright.Network.Sync.EffectLevels
{
    [Serializable]
    internal class SingleEffectLevelSyncNetwork : SyncNetworkAction
    {
        public BuffLevelData BuffLevel { get; set; }

        public SingleEffectLevelSyncNetwork(int senderId, BuffLevelData buffLevel) : base(senderId)
        {
            BuffLevel = buffLevel;
        }

        public SingleEffectLevelSyncNetwork(int senderId, int receiverId, BuffLevelData buffLevel) : base(senderId, receiverId)
        {
            BuffLevel = buffLevel;
        }

        public override void DoAction()
        {
            var modPlayer = SenderPlayer.GetModPlayer<SpellwrightBuffPlayer>();
            modPlayer.SetBuffLevel(BuffLevel);
        }
    }
}
