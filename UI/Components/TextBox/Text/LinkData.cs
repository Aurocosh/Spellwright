using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spellwright.UI.Components.TextBox.Text
{
    internal class LinkData
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public bool IsLinkValid => Type.Length > 0;

        private readonly Dictionary<string, string> parameters = new();

        public LinkData()
        {
            Type = "";
            Id = "";
        }

        public LinkData(string type, string id)
        {
            Type = type;
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

        public string GetId(string fallbackValue)
        {
            if (Id.Length > 0)
                return Id;
            return fallbackValue;
        }

        public int GetId(int fallbackValue)
        {
            if (int.TryParse(Id, out int intValue))
                return intValue;
            return fallbackValue;
        }

        public T GetId<T>(T fallbackValue)
             where T : Enum
        {
            if (int.TryParse(Id, out int intValue))
                return (T)(object)intValue;
            return fallbackValue;
        }

        public void SetId(int id)
        {
            Id = id.ToString();
        }

        public void SetId<T>(T id)
             where T : Enum
        {
            Id = ((int)(object)id).ToString();
        }

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
            var linkPart = "";
            if (IsLinkValid)
            {
                if (Id.Length == 0)
                    linkPart = $"<{Type}>";
                else
                    linkPart = $"<{Type}:{Id}>";
            }

            var parametersPart = "";
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
                var allParameters = string.Join(',', parameterList);
                parametersPart = $"({allParameters})";
            }
            return linkPart + parametersPart;
        }

        public static LinkData Parse(string linkText)
        {
            Regex linkRegex = new(@"(?:\<([^\>]*)\>)?(?:\(([^)]*)\))?");

            var match = linkRegex.Match(linkText);
            if (match.Success)
            {
                var typeTextValue = match.Groups[1].Value;
                var typeParts = typeTextValue.Split(':', 2);

                var linkType = typeParts[0];
                var linkId = "";
                if (typeParts.Length > 1)
                    linkId = typeParts[1];

                var linkData = new LinkData(linkType, linkId);

                if (match.Groups[2].Success)
                {
                    var paramString = match.Groups[2].Value;
                    var paramList = paramString.Split(',');
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
