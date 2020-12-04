using System.Collections.Generic;
using CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes;
using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public class AttributeMetadataContext
    {
        public readonly Dictionary<string, IAttributeMetadataType> Strategies = new Dictionary<string, IAttributeMetadataType>();

        public AttributeMetadataContext(Attribute attribute, string publisherPrefix)
        {
            Strategies.Add(nameof(StringAttributeMetadataType), new StringAttributeMetadataType(attribute, publisherPrefix));
        }

        public IAttributeMetadataType GetAttributeMetadata(string name)
        {
            var strategyName = $"{name}AttributeMetadataType";
            return Strategies[strategyName];
        }
    }
}
