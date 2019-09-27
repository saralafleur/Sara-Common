using System;
using System.IO;
using System.Xml.Serialization;

namespace Sara.NETStandard.Common.Extension
{
    public static class SerializeExt
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            var writer = new XmlSerializer(toSerialize.GetType());
            var textWriter = new StringWriter();

            writer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }
        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }

}
