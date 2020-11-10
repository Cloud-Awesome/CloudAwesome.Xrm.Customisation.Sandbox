using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.Sandbox.EntityModel;
using CloudAwesome.Xrm.Customisation.Sandbox.PluginModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using PluginAssembly = CloudAwesome.Xrm.Customisation.Sandbox.EntityModel.PluginAssembly;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CrmServiceClient(
                "AuthType=Office365;" +
                "Username=arthur@cloudawesome.uk;" +
                "Password='RRbkX{d}0npe<]y&bB1;su@fSJz}v&fHx>85';" +
                "Url=https://awesome-sandbox.crm11.dynamics.com");

            Run("../../SampleManifest_v2.xml", client);
            Console.ReadKey();
        }

        public static void Run(string manifestSourceFile, CrmServiceClient client)
        {
            // 1. Get and Display the XML manifest
            var manifest = GetPluginManifest(manifestSourceFile);

            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                // 2. Register DLL
                Console.WriteLine($"Assembly FriendlyName = {pluginAssembly.FriendlyName};");

                var assemblyFileInfo = new FileInfo(pluginAssembly.Assembly);
                var assembly = Assembly.LoadFile(assemblyFileInfo.FullName);
                var assemblyParts = assembly.FullName.Split(',');

                var version = assemblyParts[1].Split('=')[1].Trim();
                var culture = assemblyParts[2].Split('=')[1].Trim();
                var publicKeyToken = assemblyParts[3].Split('=')[1].Trim();

                //Entity assemblyEntity = new Entity("pluginassembly")
                //{
                //    ["name"] = pluginAssembly.Name,
                //    ["culture"] = culture,
                //    ["version"] = version,
                //    ["publickeytoken"] = publicKeyToken,
                //    ["sourcetype"] = new OptionSetValue(0),
                //    ["isolationmode"] = new OptionSetValue(2),
                //    ["content"] = Convert.ToBase64String(File.ReadAllBytes(pluginAssembly.Assembly))
                //};

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

                // TODO - Query target environment to see if it already exists. Update if it does
                // Or have an upsert function to save duplication..?

                // TODO - Reference Core for Entity Extensions
                // var assemblyId = assemblyEntity.Create();
                var createdAssembly = client.Create(assemblyEntity);
                
                foreach (var plugin in pluginAssembly.Plugins)
                {
                    // 3. Register plugins
                    Console.WriteLine($"    PluginType FriendlyName = {plugin.FriendlyName}");

                    PluginType pluginType = new PluginType()
                    {
                        PluginAssemblyId = new EntityReference("pluginassembly", createdAssembly),
                        TypeName = plugin.Name,
                        FriendlyName = plugin.FriendlyName,
                        Name = plugin.Name,
                        Description = plugin.Description
                    };

                    var createdPluginType = client.Create(pluginType);

                    if (plugin.Steps == null)
                    {
                        continue;
                    }

                    foreach (var step in plugin.Steps)
                    {
                        // 4. Register plugin steps
                        Console.WriteLine($"        Step = {step.FriendlyName}");

                        var sdkStep = new SdkMessageProcessingStep(){
                            Name = step.Name,
                            Configuration = step.UnsecureConfiguration,
                            Mode = SdkMessageProcessingStep_Mode.Asynchronous,
                            Rank = step.ExecutionOrder,
                            Stage = SdkMessageProcessingStep_Stage.Postoperation,
                            SupportedDeployment = SdkMessageProcessingStep_SupportedDeployment.ServerOnly,
                            // QUESTION - why is this deprecated?! What it's been replaced by?!
                            PluginTypeId = new EntityReference("plugintype", createdPluginType),
                            // TODO - need something to query SdkMessages and properly map them to the manifest options...
                            //  Probably means that needs an unvalidated string in the manifest...
                            SdkMessageId = new EntityReference("sdkmessage", Guid.Parse("20bebb1b-ea3e-db11-86a7-000a3a5473e8")),
                            Description = step.Description,
                            AsyncAutoDelete = step.AsyncAutoDelete
                            // TODO loop through attributes to create a single string?
                            //FilteringAttributes = step.FilteringAttributes.
                            
                        };

                        var createdStep = client.Create(sdkStep);

                        foreach (var image in step.EntityImages)
                        {
                            // 5. Register entity images
                            Console.WriteLine($"        Image = {image.Name}");

                            //var stepImage = new SdkMessageProcessingStepImage(){
                            //    Name = image.Name,
                            //    Attributes = image.Attributes,
                            //    ImageType = SdkMessageProcessingStepImage_ImageType.PreImage
                            //};

                            //var createdImage = client.Create(stepImage);
                        }
                    }
                }
            }

            Console.WriteLine("All done... So far...");
            Console.ReadKey();

            // 6. Register Service Endpoints



            // 7. Register WebHooks



            // (Post-v1)
            // (8. Register CWAs)


            // (Maybe do this during each section above?)
            // 9. Add everything to specified solution
            // 10. Remove anything unwanted? 
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
