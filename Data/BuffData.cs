namespace Spellwright.Data
{
    internal class BuffData
    {
        public int Type { get; }
        public int Duration { get; }

        public BuffData(int type, int duration)
        {
            Type = type;
            Duration = duration;
        }
    }
}
