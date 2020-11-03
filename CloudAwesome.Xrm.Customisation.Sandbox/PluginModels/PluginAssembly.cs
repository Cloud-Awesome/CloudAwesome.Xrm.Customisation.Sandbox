namespace CloudAwesome.Xrm.Customisation.Sandbox.PluginModels
{
    public class PluginAssembly
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Assembly { get; set; }

        public string SolutionName { get; set; }

        public Plugin[] Plugins { get; set; }

    }
}
