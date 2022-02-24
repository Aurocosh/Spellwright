using Spellwright.DustSpawners;

namespace Spellwright.Network.Base
{
    internal class DustSpawnPacketHandler<T> : BroadcastPacketHandler<T>
        where T : DustSpawner
    {
        protected override void HandleData(T data, byte fromWho, bool fromServer)
        {
            data.Spawn();
        }
    }
}
