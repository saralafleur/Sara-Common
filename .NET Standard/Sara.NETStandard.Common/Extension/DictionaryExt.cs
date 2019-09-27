using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Sara.NETStandard.Common.Extension
{
    public static class DictionaryExt
    {
        public static int GetLongestKeyLength(this Dictionary<string, string> dictionary)
        {
            return dictionary.Select(pair => pair.Key.Length).Concat(new[] { 0 }).Max();
        }

        public static int GetLongestValueLength(this Dictionary<string, string> dictionary)
        {
            return dictionary.Select(pair => pair.Value.Length).Concat(new[] { 0 }).Max();
        }
        public static ReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TKey, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new ReadOnlyDictionary<TKey, TSource>(source.ToDictionary(keySelector));
        }
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(keySelector, valueSelector));
        }
        public static T GetValue<T>(this Dictionary<string, string> dictionary, string keyName, T defaultValue)
        {
            var returnValue = defaultValue;
            if (dictionary.ContainsKey(keyName))
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                if (defaultValue is int && string.IsNullOrEmpty(dictionary[keyName]))
                    return defaultValue;

                var value = (T)converter.ConvertFromString(dictionary[keyName]);

                if (value is string)
                    if (string.IsNullOrEmpty(value as string))
                        return defaultValue;

                return value;
            }
            return returnValue;
        }

    }
}
