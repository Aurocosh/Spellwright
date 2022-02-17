namespace Spellwright.Spells.SpellExtraData
{
    internal sealed class OceanStepData : SpellData
    {
        public int TeleportDestination { get; } // 0 - not set, 1 - left, 2 - right

        public OceanStepData(int teleportDestination)
        {
            TeleportDestination = teleportDestination;
        }
    }
}
