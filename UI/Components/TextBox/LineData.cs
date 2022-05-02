namespace Spellwright.UI.Components.TextBox
{
    internal class LineData
    {
        public string text;
        public float height;
        public int startingIndex;
        public int length;

        public LineData(string text, float height, int startingIndex, int length)
        {
            this.text = text;
            this.height = height;
            this.startingIndex = startingIndex;
            this.length = length;
        }
    }
}
