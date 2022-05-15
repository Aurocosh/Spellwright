using System;
using System.Collections.Generic;

namespace Spellwright.Lib
{
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            if (!TryGetValue(key, out List<TValue> values))
            {
                values = new List<TValue>();
                Add(key, values);
            }
            values.Add(value);
        }

        public IReadOnlyList<TValue> GetValues(TKey key)
        {
            if (!TryGetValue(key, out List<TValue> values))
                return values;
            return Array.Empty<TValue>();
        }
    }
}
