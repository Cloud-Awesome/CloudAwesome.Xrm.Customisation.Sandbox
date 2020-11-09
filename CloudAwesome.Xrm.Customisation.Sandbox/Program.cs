using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.Sandbox.EntityModel;
using CloudAwesome.Xrm.Customisation.Sandbox.PluginModels;
using Microsoft.Xrm.Sdk;
using PluginAssembly = CloudAwesome.Xrm.Customisation.Sandbox.EntityModel.PluginAssembly;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            Console.ReadKey();
        }

        public static void Run()
        {
            // 1. Get and Display the XML manifest
            const string sourceFile = "../../SampleManifest_v2.xml";
            var manifest = GetPluginManifest(sourceFile);

            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                // 2. Register DLL
                Console.WriteLine($"Assembly FriendlyName = {pluginAssembly.FriendlyName};");

                var assemblyFileInfo = new FileInfo(pluginAssembly.Assembly);
                var assembly = Assembly.LoadFile(assemblyFileInfo.FullName);
                var version = assembly.GetName().Version.ToString();
                var culture = assembly.GetName().CultureName;
                var publicKeyToken = assembly.GetName().GetPublicKeyToken().ToString();
                
                PluginAssembly assemblyEntity = new PluginAssembly()
                {
                    Name = pluginAssembly.Name,
                    Culture = culture,
                    Version = version,
                    PublicKeyToken = publicKeyToken,
                    SourceType = PluginAssembly_SourceType.Database,// Only database supported now
                    IsolationMode = PluginAssembly_IsolationMode.Sandbox, // Only Sandbox supported now
                    Content = Convert.ToBase64String(File.ReadAllBytes(pluginAssembly.Assembly))
                };

                // TODO - Reference Core for Entity Extensions
                // var assemblyId = assemblyEntity.Create();
                
                foreach (var plugin in pluginAssembly.Plugins)
                {
                    // 3. Register plugins
                    Console.WriteLine($"    PluginType FriendlyName = {plugin.FriendlyName}");

                    if (plugin.Steps == null)
                    {
                        continue;
                    }

                    foreach (var step in plugin.Steps)
                    {
                        // 4. Register plugin steps
                        Console.WriteLine($"        Step = {step.FriendlyName}");
                    }
                }
            }

            Console.ReadKey();

            // 5. Register Service Endpoints



            // 6. Register WebHooks



            // (Post-v1)
            // (7. Register CWAs)


            // (Maybe do this during each section above?)
            // 8. Add everything to specified solution
            // 9. Remove anything unwanted? 
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
