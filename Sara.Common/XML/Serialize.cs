using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Sara.Common.XML
{
    public static class Serialize
    {
        public static void Save(object model, string Path)
        {
            if (string.IsNullOrEmpty(Path))
                throw new NullReferenceException("Path cannot be null or blank!");

            if (!Directory.Exists(Directory.GetParent(Path).FullName))
                Directory.CreateDirectory(Directory.GetParent(Path).FullName);

            var writer = new XmlSerializer(model.GetType());

            using (var file = new StreamWriter(Path))
            {
                writer.Serialize(file, model);
                file.Close();
            }
        }
        public static T Load<T>(string path) where T : new()
        {
            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException("Path cannot be null or blank!");

            if (!System.IO.File.Exists(path))
                return new T();
            var reader = new XmlSerializer(typeof(T));

            /////
            // This was a pain.  I had a 0x00 invalid character for XML that would cause a failure during Deserialize.
            // The following site provided the solution - Sara
            // http://baleinoid.com/whaly/2011/08/xml-deserialization-invalid-character/
            /////
            var xml = System.IO.File.ReadAllText(path);
            var sr3 = new StringReader(xml);

            // The following commented out code was used to handle an invalid character issue.
            // I ran into another issue with the normalization of /r/n to /n
            // Using XmlTextReader allowed me to remove the normalization by setting WhitespaceHandling to Significant. 
            // I don't know if i will run into the invalide character again, will see - Sara

            //var settings = new XmlReaderSettings { CheckCharacters = false };
            //var xr3 = XmlReader.Create(sr3, settings);

            var xr3 = new XmlTextReader(sr3);
            xr3.WhitespaceHandling = WhitespaceHandling.Significant;

            return (T)reader.Deserialize(xr3);
        }

    }
}
