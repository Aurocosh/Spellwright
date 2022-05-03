namespace Spellwright.Content.Spells.Storage.Base
{
    internal enum StorageAction : byte
    {
        Invalid,
        Push,
        Pop,
        Lock,
        Unlock
    }
}
