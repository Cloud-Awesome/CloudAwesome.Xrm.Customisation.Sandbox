using System;
using System.IO;
using System.Xml.Serialization;

using CloudAwesome.Xrm.Customisation.Sandbox.PluginModels;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Get and Display the XML manifest
            const string sourceFile = "../../SampleManifest_v2.xml";
            var plugins = GetPluginManifest(sourceFile);

            foreach (var plugin in plugins.PluginAssemblies)
            {
                Console.WriteLine($"Assembly FriendlyName = {plugin.FriendlyName};");

                foreach (var pluginType in plugin.Plugins)
                {
                    Console.WriteLine($"    PluginType FriendlyName = {pluginType.FriendlyName}");

                    if (pluginType.Steps == null)
                    {
                        continue;
                    }

                    foreach (var step in pluginType.Steps)
                    {
                        Console.WriteLine($"        Step = {step.Name}");
                    }
                }
            }

            // 2. Register DLL


            // 3. Register Steps


            // 4. Register WF actions


            // 5. Add everything to specified solution


            // 6. Remove anything unwanted? 
            //      (Or clobber before registering?)



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
