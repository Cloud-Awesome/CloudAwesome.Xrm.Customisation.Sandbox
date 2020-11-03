using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox.PluginModels
{
    public enum EntityImageType { PreImage, PostImage }

    public class EntityImage
    {
        public string Name { get; set; }

        public EntityImageType Type { get; set; }

        [XmlArrayItem("Attribute")]
        public string[] Attributes { get; set; }

    }
}
