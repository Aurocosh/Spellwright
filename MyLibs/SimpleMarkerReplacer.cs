using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spellwright.Lib
{
    internal class SimpleMarkerReplacer
    {
        public bool RemoveMissingMarkers { get; set; }
        private readonly Dictionary<string, string> parameters = new();

        public void AddMarkerValue(string marker, string value)
        {
            parameters[marker] = value;
        }

        public void ClearValues()
        {
            parameters.Clear();
        }

        private string ReplaceMarker(string marker)
        {
            if (parameters.TryGetValue(marker, out var value))
                return value;
            return RemoveMissingMarkers ? "" : marker;
        }

        public string Replace(string template)
        {
            return Regex.Replace(template, @"\{([^}]+)\}", x => ReplaceMarker(x.Groups[1].Value));
        }
    }
}
