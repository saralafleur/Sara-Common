using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sara.NETStandard.Common.Extension
{
    public static class ObjectExt
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
