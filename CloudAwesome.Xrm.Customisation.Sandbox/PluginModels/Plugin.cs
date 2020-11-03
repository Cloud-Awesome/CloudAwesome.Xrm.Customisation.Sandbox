namespace CloudAwesome.Xrm.Customisation.Sandbox.PluginModels
{
    public class Plugin
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public Step[] Steps { get; set; }
    }
}
