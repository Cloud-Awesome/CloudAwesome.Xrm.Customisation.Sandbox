using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public class PluginManifest
    {
        public enum CdsConnectionType { AppRegistration, ConnectionString, UserNameAndPassword }

        public PluginAssembly[] PluginAssemblies { get; set; }

        public ServiceEndpoint[] ServiceEndpoints { get; set; }

        public Webhook[] Webhooks { get; set; }

        public WorkflowAssembly[] WorkflowAssemblies { get; set; }

        public CdsConnectionType ConnectionType { get; set; }

        public string CdsConnectionString { get; set; }

        public string CdsUrl { get; set; }

        public string CdsUserName { get; set; }

        public string CdsPassword { get; set; }

        public string CdsAppId { get; set; }

        public string CdsAppSecret { get; set; }
    }
}
