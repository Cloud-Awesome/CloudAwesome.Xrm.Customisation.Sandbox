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
using ServiceEndpoint = CloudAwesome.Xrm.Customisation.Sandbox.EntityModel.ServiceEndpoint;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CrmServiceClient(
                "AuthType=Office365;" +
                "Username=arthur@cloudawesome.uk;" +
                "Password='<T(},C>7D]#oNu,bgNuz7O5*EVv%n+d$S=?^';" +
                "Url=https://awesome-sandbox.crm11.dynamics.com");

            // 1. Get and Display the XML manifest
            var manifest = GetPluginManifest("../../SampleManifest_v2.xml");

            //RegisterPlugins(manifest, client);

            RegisterServiceEndPoints(manifest, client);

            Console.ReadKey();
        }

        public static void RegisterServiceEndPoints(PluginManifest manifest, CrmServiceClient client)
        {
            // TODO - >> Only currently supports Queue - build the entity for different contracts
            // TODO - > Test manifest nodes for each contract * 6 (Queue, Topic, OneWay, TwoWay, Rest, EventHub)
            // TODO - Register steps against the endpoint
            // TODO - Register and test executing plugins on registered endpoints

            foreach (var endpoint in manifest.ServiceEndpoints)
            {
                var query = new QueryExpression()
                {
                    EntityName = ServiceEndpoint.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(ServiceEndpoint.PrimaryNameAttribute, ConditionOperator.Equal,
                                endpoint.Name)
                        }
                    }
                };
                var results = client.RetrieveMultiple(query);

                if (results.Entities.Count > 1)
                {
                    Console.WriteLine($"Service Endpoint '{endpoint.Name}' has multiple duplicates, based on the 'Name' attribute. This endpoint is being ignored until registered endpoints are de-duplicated by name");
                    continue;
                }

                Enum.TryParse<ServiceEndpoint_Contract>(endpoint.Contract.ToString(), out var contractType);
                Enum.TryParse<ServiceEndpoint_MessageFormat>(endpoint.MessageFormat.ToString(), out var messageFormat);
                Enum.TryParse<ServiceEndpoint_AuthType>(endpoint.AuthType.ToString(), out var authType);
                Enum.TryParse<ServiceEndpoint_UserClaim>(endpoint.UserClaim.ToString(), out var userClaim);

                var s = new ServiceEndpoint()
                {
                    Name = endpoint.Name,
                    NamespaceAddress = endpoint.NamespaceAddress,
                    Contract = contractType,
                    Path = endpoint.Path,
                    MessageFormat = messageFormat,
                    AuthType = authType,
                    SASKeyName = endpoint.SASKeyName,
                    SASKey = endpoint.SASKey,
                    UserClaim = userClaim,
                    Description = endpoint.Description
                };

                Guid createdServiceEndPoint;
                if (results.Entities.Count == 1)
                {
                    createdServiceEndPoint = results.Entities.FirstOrDefault().Id;
                    s.Id = createdServiceEndPoint;
                    client.Update(s);
                }
                else
                {
                    createdServiceEndPoint = client.Create(s);
                }
                
                if (!string.IsNullOrEmpty(manifest.SolutionName))
                {
                    SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName,
                        createdServiceEndPoint, ComponentType.ServiceEndpoint);
                }

                Console.WriteLine($"Service Endpoint '{s.Name}' registered");
            }

        }

        public static void RegisterPlugins(PluginManifest manifest, CrmServiceClient client)
        {
            
            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                // Get Solution Name
                string targetSolutionName = string.Empty;
                if (!string.IsNullOrEmpty(pluginAssembly.SolutionName))
                {
                    targetSolutionName = pluginAssembly.SolutionName;

                } else if (!string.IsNullOrEmpty(manifest.SolutionName))
                {
                    targetSolutionName = manifest.SolutionName;
                }

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

                var createdAssembly = new EntityReference(PluginAssembly.EntityLogicalName);
                if (assemblyResults.Entities.Count == 0)
                {
                    createdAssembly.Id = client.Create(assemblyEntity);

                    // TODO - Reference Core for Entity Extensions
                    // var assemblyId = assemblyEntity.Create();
                }
                else
                {
                    createdAssembly.Id = assemblyResults.Entities.FirstOrDefault().Id;
                    assemblyEntity.Id = createdAssembly.Id;
                    client.Update(assemblyEntity);

                    // TODO - Reference Core for Entity Extensions
                    //assemblyEntity.Update();
                }

                if (!string.IsNullOrEmpty(targetSolutionName))
                {
                    SolutionWrapper.AddSolutionComponent(client, targetSolutionName, 
                        createdAssembly.Id, ComponentType.PluginAssembly);
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

                    var pluginType = new PluginType()
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

                        if (!string.IsNullOrEmpty(targetSolutionName))
                        {
                            SolutionWrapper.AddSolutionComponent(client, targetSolutionName,
                                createdStep.Id, ComponentType.SDKMessageProcessingStep);
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
                                Attributes1 = string.Join(",", image.Attributes),
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
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream fs = File.OpenRead(path))
            {
                return (T)xmlSerializer.Deserialize(fs);
            }
        }
    }
}
