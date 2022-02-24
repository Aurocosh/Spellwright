using Spellwright.DustSpawners;
using Spellwright.Network.Base;

namespace Spellwright.Network.Dusts
{
    internal class AoeCastDustHandler : BroadcastPacketHandler<AoeCastDustSpawner>
    {
        protected override void HandleData(AoeCastDustSpawner data, byte fromWho, bool fromServer)
        {
            data.Spawn();
        }
    }
}
