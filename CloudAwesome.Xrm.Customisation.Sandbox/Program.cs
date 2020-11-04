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
            
            
            Console.ReadKey();
        }

        public void Run()
        {
            // 1. Get and Display the XML manifest
            const string sourceFile = "../../SampleManifest_v2.xml";
            var manifest = GetPluginManifest(sourceFile);

            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                Console.WriteLine($"Assembly FriendlyName = {pluginAssembly.FriendlyName};");

                foreach (var plugin in pluginAssembly.Plugins)
                {
                    Console.WriteLine($"    PluginType FriendlyName = {plugin.FriendlyName}");

                    if (plugin.Steps == null)
                    {
                        continue;
                    }

                    foreach (var step in plugin.Steps)
                    {
                        Console.WriteLine($"        Step = {step.FriendlyName}");
                    }
                }
            }

            // 2. Register DLL


            // 3. Register Steps


            // 4. Register WF actions


            // 5. Add everything to specified solution


            // 6. Remove anything unwanted? 
            //      (Or clobber before registering?)

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
