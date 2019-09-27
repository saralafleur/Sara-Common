using System;
using System.Collections.Generic;
using System.Linq;

namespace Sara.NETStandard.Common.Extension
{
    public static class ListExt
    {
        public static int GetLongestLength(this List<string> list)
        {
            return list.Select(str => str.Length).Concat(new[] { 0 }).Max();
        }
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
