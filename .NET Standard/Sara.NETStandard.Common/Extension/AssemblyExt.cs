using System;
using System.Linq;
using System.Reflection;

namespace Sara.NETStandard.Common.Extension
{
    public static class AssemblyExt
    {
        /// <summary>
        /// Returns the Copyright value from the Assembly.
        /// </summary>
        public static string GetCopyright(this System.Reflection.Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
        public static string GetAssemblyVersion(this System.Reflection.Assembly assembly)
        {
            var attribute = GetAssemblyAttribute<AssemblyVersionAttribute>(assembly);
            return attribute == null ? string.Empty : attribute.Version;
        }
        /// <summary>
        /// Returns the Title from the Assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyTitle(this System.Reflection.Assembly assembly)
        {
            var attribute = GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
            return attribute == null ? string.Empty : attribute.Title;
        }
        /// <summary>
        /// Returns an Assembly attribute.
        /// </summary>
        public static T GetAssemblyAttribute<T>(this System.Reflection.Assembly assembly) where T : Attribute
        {
            return assembly.GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
        }
        /// <summary>
        /// Returns the FileVersion from the Assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyFileVersion(this System.Reflection.Assembly assembly)
        {
            var attribute = GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly);
            return attribute == null ? string.Empty : attribute.Version;
        }
    }
}
