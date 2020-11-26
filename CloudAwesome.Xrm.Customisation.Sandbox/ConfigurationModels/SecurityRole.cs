using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class SecurityRole
    {
        public string Name { get; set; }

        [XmlArrayItem("Privilege")]
        public string[] Privileges { get; set; }

    }
}
