using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sara.Common.Extension
{
    public static class GenericExt
    {
        public static T To<T>(this object @object)
        {
            if (@object is T)
                return (T)@object;

            try
            {
                // ReSharper disable PossibleInvalidCastException
                return (T)@object;
                // ReSharper restore PossibleInvalidCastException
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch { }
            // ReSharper restore EmptyGeneralCatchClause

            // attempt to find type converter to do conversion
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(@object.GetType()))
                return (T)converter.ConvertFrom(@object);

            converter = TypeDescriptor.GetConverter(@object.GetType());
            if (converter.CanConvertTo(typeof(T)))
                return (T)converter.ConvertTo(@object, typeof(T));

            throw new NotSupportedException($"Unable to convert from type \"{@object.GetType()}\" to type \"{typeof(T)}\".");
        }
        public static T To<T>(this object @object, T defaultValue)
        {
            try
            {
                return @object.To<T>();
            }
            catch
            {
                return defaultValue;
            }
        }
        public static IEnumerable<T> Chain<T>(this T first, Func<T, T> follow) where T : class
        {
            for (var item = first; item != null; item = follow(item))
                yield return item;
        }

    }
}
