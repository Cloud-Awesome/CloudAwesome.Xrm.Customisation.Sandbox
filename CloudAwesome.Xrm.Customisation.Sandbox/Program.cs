using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.Sandbox.EntityModel;
using CloudAwesome.Xrm.Customisation.Sandbox.PluginModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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

                // Check if assembly already exists
                var assemblyQuery = new QueryExpression()
                {
                    EntityName = PluginAssembly.EntityLogicalName,
                    ColumnSet = new ColumnSet(PluginAssembly.PrimaryIdAttribute, PluginAssembly.PrimaryNameAttribute),
                    Criteria = new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(PluginAssembly.PrimaryNameAttribute, ConditionOperator.Equal, pluginAssembly.Name),
                            new ConditionExpression("version", ConditionOperator.Equal, version)
                        }
                    }
                };

                var assemblyResults = client.RetrieveMultiple(assemblyQuery);

                var createdAssembly = new EntityReference(PluginAssembly.EntityLogicalName);
                var assemblyEntity = new PluginAssembly()
                {
                    Name = pluginAssembly.Name,
                    Culture = culture,
                    Version = version,
                    PublicKeyToken = publicKeyToken,
                    SourceType = PluginAssembly_SourceType.Database,// Only database supported now
                    IsolationMode = PluginAssembly_IsolationMode.Sandbox, // Only Sandbox supported now
                    Content = Convert.ToBase64String(File.ReadAllBytes(pluginAssembly.Assembly))
                };

                if (assemblyResults.Entities.Count == 0)
                {
                    // Create
                    createdAssembly.Id = client.Create(assemblyEntity);

                    // TODO - Reference Core for Entity Extensions
                    // var assemblyId = assemblyEntity.Create();
                }
                else
                {
                    // Update
                    createdAssembly.Id = assemblyResults.Entities.FirstOrDefault().Id;
                    assemblyEntity.Id = createdAssembly.Id;
                    client.Update(assemblyEntity);

                    // TODO - Reference Core for Entity Extensions
                    //assemblyEntity.Update();
                }

                foreach (var plugin in pluginAssembly.Plugins)
                {
                    // 3. Register plugins
                    Console.WriteLine($"    PluginType FriendlyName = {plugin.FriendlyName}");

                    var pluginQuery = new QueryExpression()
                    {
                        EntityName = PluginType.EntityLogicalName,
                        ColumnSet = new ColumnSet(PluginType.PrimaryIdAttribute, PluginType.PrimaryNameAttribute),
                        Criteria = new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(PluginType.PrimaryNameAttribute, ConditionOperator.Equal, plugin.Name),
                                new ConditionExpression("pluginassemblyid", ConditionOperator.Equal, createdAssembly.Id)
                            }
                        }
                    };
                    var pluginResults = client.RetrieveMultiple(pluginQuery);

                    PluginType pluginType = new PluginType()
                    {
                        PluginAssemblyId = createdAssembly,
                        TypeName = plugin.Name,
                        FriendlyName = plugin.FriendlyName,
                        Name = plugin.Name,
                        Description = plugin.Description
                    };

                    var createdPluginType = new EntityReference(PluginType.EntityLogicalName);
                    if (pluginResults.Entities.Count == 0)
                    {
                        // Create
                        createdPluginType.Id = client.Create(pluginType);

                        // TODO - Reference Core for Entity Extensions
                        //createdPluginType.Create(client);
                    }
                    else
                    {
                        // Update
                        createdPluginType.Id = pluginResults.Entities.FirstOrDefault().Id;
                        pluginType.Id = createdPluginType.Id;
                        client.Update(pluginType);

                        // TODO - Reference Core for Entity Extensions
                        //createdPluginType.Update(client);
                    }

                    if (plugin.Steps == null)
                    {
                        continue;
                    }

                    foreach (var step in plugin.Steps)
                    {
                        // 4. Register plugin steps
                        Console.WriteLine($"        Step = {step.FriendlyName}");

                        // TODO - Get SDK Message ref
                        var sdkMessageQuery = new QueryExpression(SdkMessage.EntityLogicalName)
                        {
                            ColumnSet = new ColumnSet(SdkMessage.PrimaryIdAttribute, SdkMessage.PrimaryNameAttribute),
                            Criteria = new FilterExpression()
                            {
                                Conditions =
                                {
                                    new ConditionExpression(SdkMessage.PrimaryNameAttribute, ConditionOperator.Equal,
                                        step.Message)
                                }
                            }
                        };
                        var sdkMessage = client.RetrieveMultiple(sdkMessageQuery).Entities
                            .FirstOrDefault().ToEntityReference();

                        var stepsQuery = new QueryExpression(SdkMessageProcessingStep.EntityLogicalName)
                        {
                            ColumnSet = new ColumnSet(SdkMessageProcessingStep.PrimaryIdAttribute, 
                                SdkMessageProcessingStep.PrimaryNameAttribute),
                            Criteria = new FilterExpression()
                            {
                                Conditions =
                                {
                                    new ConditionExpression("eventhandler", ConditionOperator.Equal, createdPluginType.Id),
                                    new ConditionExpression("sdkmessageid", ConditionOperator.Equal, sdkMessage.Id),
                                    new ConditionExpression("stage", ConditionOperator.Equal, 
                                        (int)SdkMessageProcessingStep_Stage.Postoperation),
                                }
                            }
                        };
                        var stepsResults = client.RetrieveMultiple(stepsQuery);

                        var sdkStep = new SdkMessageProcessingStep()
                        {
                            Name = step.Name,
                            Configuration = step.UnsecureConfiguration,
                            Mode = SdkMessageProcessingStep_Mode.Asynchronous, // Hard-coded for now... =/
                            Rank = step.ExecutionOrder,
                            Stage = SdkMessageProcessingStep_Stage.Postoperation, // Hard-coded for now... =/
                            SupportedDeployment = SdkMessageProcessingStep_SupportedDeployment.ServerOnly, // Hard-coded for now... =/
                            EventHandler = createdPluginType,
                            SdkMessageId = sdkMessage,
                            Description = step.Description,
                            AsyncAutoDelete = step.AsyncAutoDelete
                            // TODO loop through attributes to create a single string?
                            //FilteringAttributes = step.FilteringAttributes.

                        };

                        EntityReference createdStep = new EntityReference(SdkMessageProcessingStep.EntityLogicalName);
                        if (stepsResults.Entities.Count == 0)
                        {
                            // Create
                            createdStep.Id = client.Create(sdkStep);
                        }
                        else
                        {
                            // Update
                            createdStep.Id = stepsResults.Entities.FirstOrDefault().Id;
                            sdkStep.Id = createdStep.Id;
                            client.Update(sdkStep);
                        }

                        foreach (var image in step.EntityImages)
                        {
                            // 5. Register entity images
                            Console.WriteLine($"        Image = {image.Name}");

                            var imageQuery = new QueryExpression(SdkMessageProcessingStepImage.EntityLogicalName)
                            {
                                ColumnSet = new ColumnSet(SdkMessageProcessingStepImage.PrimaryIdAttribute,
                                    SdkMessageProcessingStep.PrimaryNameAttribute),
                                Criteria = new FilterExpression()
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(SdkMessageProcessingStepImage.PrimaryNameAttribute,
                                            ConditionOperator.Equal, image.Name),
                                        new ConditionExpression("sdkmessageprocessingstepid",
                                            ConditionOperator.Equal, createdStep.Id)
                                    }
                                }
                            };
                            var imageResults = client.RetrieveMultiple(imageQuery);

                            var stepImage = new SdkMessageProcessingStepImage()
                            {
                                Name = image.Name,
                                EntityAlias = image.Name,
                                //Attributes = new AttributeCollection(), // Currently empty... //image.Attributes,
                                ImageType = SdkMessageProcessingStepImage_ImageType.PreImage,
                                MessagePropertyName = "Target",
                                SdkMessageProcessingStepId = createdStep
                            };

                            EntityReference createdImage = new EntityReference(SdkMessageProcessingStepImage.EntityLogicalName);
                            if (imageResults.Entities.Count == 0)
                            {
                                // Create
                                createdImage.Id = client.Create(stepImage);
                            }
                            else
                            {
                                // Update
                                createdImage.Id = imageResults.Entities.FirstOrDefault().Id;
                                stepImage.Id = createdImage.Id;
                                client.Update(stepImage);
                            }

                        }
                    }
                }
            }

            Console.WriteLine("All done... So far...");

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
