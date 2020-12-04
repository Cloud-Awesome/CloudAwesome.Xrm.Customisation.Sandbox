using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public interface IAttributeMetadataType
    {
        string Name { get; }
        
        AttributeMetadata AttributeMetadata { get; }
    }
}
