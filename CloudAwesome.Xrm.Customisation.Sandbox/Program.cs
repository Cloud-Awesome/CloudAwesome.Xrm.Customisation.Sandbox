using System;
using System.IO;
using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            const string sourceFile = "SampleManifest.xml";
            var plugins = GetPluginManifest(sourceFile);

            Console.ReadKey();
        }

        public static PluginManifest GetPluginManifest(string filePath)
        {
            return DeserialiseFromFile<PluginManifest>(filePath);
        }

        public static T DeserialiseFromFile<T>(string path)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            using (FileStream fs = File.OpenRead(path))
            {
                return (T)xmlSer.Deserialize(fs);
            }
        }
    }
}
