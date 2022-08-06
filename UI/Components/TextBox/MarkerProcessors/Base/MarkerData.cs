using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spellwright.UI.Components.TextBox.MarkerProcessors.Base
{
    internal class MarkerData
    {
        public string Type { get; }
        public string Text { get; }
        public string Id { get; }

        private readonly Dictionary<string, string> parameters = new();

        public MarkerData(string type, string text, string id)
        {
            Type = type;
            Text = text;
            Id = id;
        }

        public IReadOnlyDictionary<string, string> Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters.Clear();
                foreach (var pair in value)
                    parameters.Add(pair.Key, pair.Value);
            }
        }

        public bool HasParameter(string name) => parameters.ContainsKey(name);
        public bool RemoveParameter(string name) => parameters.Remove(name);

        public string GetParameter(string name, string fallbackValue = null)
        {
            if (parameters.TryGetValue(name, out var value))
                return value;
            return fallbackValue;
        }

        public T GetParameter<T>(string name, T fallbackValue)
             where T : Enum
        {
            if (parameters.TryGetValue(name, out var value) && int.TryParse(value, out int intValue))
                return (T)(object)intValue;
            return fallbackValue;
        }

        public bool GetParameter(string name, bool fallbackValue)
        {
            if (parameters.TryGetValue(name, out var value))
                return value == "True";
            return fallbackValue;
        }

        public int GetParameter(string name, int fallbackValue)
        {
            if (parameters.TryGetValue(name, out var value) && int.TryParse(value, out int intValue))
                return intValue;
            return fallbackValue;
        }

        public void SetParameter(string name, string value = "")
        {
            parameters.Add(name, value);
        }

        public void SetParameter<T>(string name, T value)
             where T : Enum
        {
            SetParameter(name, (int)(object)value);
        }

        public void SetParameter(string name, bool value)
        {
            string stringValue = value.ToString();
            SetParameter(name, stringValue);
        }

        public void SetParameter(string name, int value)
        {
            string stringValue = value.ToString();
            SetParameter(name, stringValue);
        }

        public string this[string name]
        {
            get => GetParameter(name);
            set => parameters.Add(name, value);
        }

        public override string ToString()
        {
            var marker = $"*{{{Type}}}";
            if (parameters.Count > 0)
            {
                var parameterList = new List<string>();
                foreach (var (name, value) in parameters)
                    if (value.Length > 0)
                        parameterList.Add($"{name}:{value}");
                    else
                        parameterList.Add(name);
                var allParameters = string.Join(',', parameterList);
                marker = $"{marker}({allParameters})";
            }
            return marker;
        }

        public static MarkerData Parse(string markerText)
        {
            Regex markerRegex = new(@"\*\{([^\}]*)\}(?:\{([^\}]*)\})?(?:\(([^)]*)\))?");

            var match = markerRegex.Match(markerText);
            if (match.Success)
            {
                var typeTextValue = match.Groups[1].Value;
                var markerTextValue = match.Groups[2].Value;

                var typeParts = typeTextValue.Split(':', 2);

                var markerType = typeParts[0];
                var markerId = "";
                if (typeParts.Length > 1)
                    markerId = typeParts[1];

                var markerData = new MarkerData(markerType, markerTextValue, markerId);

                if (match.Groups[3].Success)
                {
                    var paramString = match.Groups[3].Value;
                    var paramList = paramString.Split(',');
                    foreach (var param in paramList)
                    {
                        var paramParts = param.Split(':', 2);
                        var paramName = paramParts[0];
                        var paramValue = "";
                        if (paramParts.Length > 1)
                            paramValue = paramParts[1];
                        markerData.SetParameter(paramName, paramValue);
                    }
                }
                return markerData;
            }
            else
            {
                Spellwright.Instance.Logger.Error($"Invalid marker format: {markerText}");
                return null;
            }
        }
    }
}
