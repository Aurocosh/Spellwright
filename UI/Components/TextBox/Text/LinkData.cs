using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spellwright.UI.Components.TextBox.Text
{
    internal class LinkData
    {
        private static readonly Regex linkRegex = new(@"link:([^=\)]+)=?([^\)]+)?\)");

        public string Type { get; }

        private readonly Dictionary<string, string> parameters = new();

        public LinkData(string type)
        {
            Type = type;
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
            var link = $"link:{Type}";
            if (parameters.Count > 0)
            {
                var parameterList = new List<string>();
                foreach (var (name, value) in parameters)
                {
                    if (value.Length > 0)
                        parameterList.Add($"{name}:{value}");
                    else
                        parameterList.Add(name);
                }
                var allParameters = string.Join('&', parameterList);
                link = $"{link}={allParameters}";
            }
            return link;
        }

        public static LinkData Parse(string linkText)
        {


            Regex linkRegex = new(@"link:([^=\)]+)\s*=?\s*(.*)?");


            var match = linkRegex.Match(linkText);
            if (match.Success)
            {
                var linkType = match.Groups[1].Value;
                var linkData = new LinkData(linkType);

                if (match.Groups[2].Success)
                {
                    var paramString = match.Groups[2].Value;
                    var paramList = paramString.Split('&');
                    foreach (var param in paramList)
                    {
                        var paramParts = param.Split(':', 2);
                        var paramName = paramParts[0];
                        var paramValue = "";
                        if (paramParts.Length > 1)
                            paramValue = paramParts[1];
                        linkData.SetParameter(paramName, paramValue);
                    }
                }
                return linkData;
            }
            else
            {
                Spellwright.Instance.Logger.Error($"Invalid link format: {linkText}");
                return null;
            }
        }
    }
}
