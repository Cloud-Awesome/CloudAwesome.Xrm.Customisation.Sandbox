namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public class Plugin
    {
        public PluginType PluginType { get; set; }
        public string FriendlyName { get; set; }
        public string Reference { get; set; }
        public string CodeRef { get; set; }
        public string Stage { get; set; }
        public string ExecutionMode { get; set; }
        public string Message { get; set; }
        public string ExecutionOrder { get; set; }
        public string UnsecureConfiguration { get; set; }

    }

    public enum PluginType { Plugin, CustomWorkflow }
}
