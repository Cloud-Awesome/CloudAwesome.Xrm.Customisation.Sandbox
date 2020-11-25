using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class OptionSet
    {
        public string DisplayName { get; set; }
        public string SchemaName { get; set; }

        [XmlArrayItem("Item")]
        public string[] Items { get; set; }

    }
}
