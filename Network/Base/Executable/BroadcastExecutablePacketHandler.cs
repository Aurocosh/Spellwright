namespace Spellwright.Network.Base.Executable
{
    internal class BroadcastExecutablePacketHandler<T> : BroadcastPacketHandler<T>
        where T : IExecutablePacket
    {
        protected override void HandleData(T data, byte fromWho, bool fromServer)
        {
            data.Execute();
        }
    }
}
