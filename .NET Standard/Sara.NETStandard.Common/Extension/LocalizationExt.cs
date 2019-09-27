using System;
using System.Resources;

namespace Sara.NETStandard.Common.Extension
{
    public static class LocalizationExt
    {
        public static string ToLocalizedString(this Enum e)
        {
            var type = e.GetType();

            foreach (var name in type.Assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(".resources"))
                {
                    var manager = new ResourceManager(name.Replace(".resources", System.String.Empty), type.Assembly);
                    var localizedString = manager.GetString(type.Name + "_" + e);
                    if (localizedString != null)
                        return localizedString;
                }
            }
            return "UNKNOWN STRING";
        }
    }
}
