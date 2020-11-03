namespace CloudAwesome.Xrm.Customisation.Sandbox.PluginModels
{
    public enum CdsConnectionType { AppRegistration, ConnectionString, UserNameAndPassword }

    public class PluginManifest
    {
        public PluginAssembly[] PluginAssemblies { get; set; }

        public ServiceEndpoint[] ServiceEndpoints { get; set; }

        public Webhook[] Webhooks { get; set; }

        public WorkflowAssembly[] WorkflowAssemblies { get; set; }

        public string SolutionName { get; set; }

        public CdsConnectionType ConnectionType { get; set; }

        public string CdsConnectionString { get; set; }

        public string CdsUrl { get; set; }

        public string CdsUserName { get; set; }

        public string CdsPassword { get; set; }

        public string CdsAppId { get; set; }

        public string CdsAppSecret { get; set; }
    }
}
