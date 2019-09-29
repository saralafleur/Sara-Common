using System;
using System.Linq;

namespace Sara.Common.Localization
{
    public static class Localize
    {
        public static bool EnumTryParse(System.Type enumType, string value, out object result, bool ignoreCase)
        {
            var name = (from n in Enum.GetNames(enumType)
                           where n.ToLower() == (value == null ? null : value.ToLower())
                           select n).SingleOrDefault();

            result = name == null ? null : Enum.Parse(enumType, value, ignoreCase);

            return name != null;
        }
    }
}
