namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public enum CdsConnectionType { AppRegistration, ConnectionString, UserNameAndPassword }

    public class ConfigurationManifest
    {
        public string SolutionName { get; set; }
        public string PubilsherUniqueName { get; set; }

        public bool Clobber { get; set; }

        public CdsConnectionType ConnectionType { get; set; }

        public string CdsConnectionString { get; set; }

        public string CdsUrl { get; set; }

        public string CdsUserName { get; set; }

        public string CdsPassword { get; set; }

        public string CdsAppId { get; set; }

        public string CdsAppSecret { get; set; }

        public Entity[] Entities { get; set; }

        public OptionSet[] OptionSets { get; set; }

        public SecurityRole[] SecurityRoles { get; set; }

        public ModelDrivenApp[] ModelDrivenApps { get; set; }
    }
}
