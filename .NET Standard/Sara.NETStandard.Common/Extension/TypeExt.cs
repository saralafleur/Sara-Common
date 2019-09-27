using System;
using System.Linq;

namespace Sara.NETStandard.Common.Extension
{
    public static class TypeExt
    {
        /// <summary>
        /// Returns a list of Class Types that implement the specified Type.
        /// </summary>
        public static System.Type[] GetClassTypes(this System.Reflection.Assembly assembly, System.Type type)
        {
            var allTypes = assembly.GetTypes();

            return allTypes.Where(t => t.IsClass).Where(t => t == type || t.IsSubclassOf(type) || (type.IsInterface && t.ImplementsInterface(type))).ToArray();
        }

        /// <summary>
        /// Returns an array of System.Type objects representing a filtered list of interfaces
        //  implemented or inherited by the specified type.
        public static bool ImplementsInterface(this System.Type type, System.Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new Exception("Expected an interface type.");
            }

            return type.FindInterfaces((i, j) => Equals(i, j) || i.IsSubclassOf((System.Type)j), interfaceType).Length != 0;
        }
    }
}
