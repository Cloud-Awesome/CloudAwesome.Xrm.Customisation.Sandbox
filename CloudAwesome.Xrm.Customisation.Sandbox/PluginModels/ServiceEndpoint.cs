namespace CloudAwesome.Xrm.Customisation.Sandbox.PluginModels
{
    public enum ServiceEndpointContract { Queue = 0, Topic = 1, OneWay = 2, TwoWay = 3, Rest = 4, EventHub = 5 }

    public enum ServiceEndpointMessageFormat { NETBinary = 0, Json = 1, XML = 2  }

    public enum ServiceEndpointAuthType { SASKey = 0, SASToken = 1 }

    public enum ServiceEndpointUserClaim { None = 0, UserId = 1  }

    public class ServiceEndpoint
    {
        public string Name { get; set; }

        public string NamespaceAddress { get; set; }

        // aka DesignationType
        public ServiceEndpointContract Contract { get; set; }

        public string Path { get; set; }

        public ServiceEndpointMessageFormat MessageFormat { get; set; }

        public ServiceEndpointAuthType AuthType { get; set; }

        public string SASKeyName { get; set; }

        public string SASKey { get; set; }

        public ServiceEndpointUserClaim UserClaim { get; set; }

        public string Description { get; set; }

        public Plugin[] Plugins { get; set; }

    }
}