namespace Spellwright.Data
{
    internal class BuffData
    {
        public int Type { get; set; }
        public int Duration { get; set; }

        public BuffData()
        {
            Type = 0;
            Duration = 0;
        }

        public BuffData(int type, int duration)
        {
            Type = type;
            Duration = duration;
        }
    }
}
