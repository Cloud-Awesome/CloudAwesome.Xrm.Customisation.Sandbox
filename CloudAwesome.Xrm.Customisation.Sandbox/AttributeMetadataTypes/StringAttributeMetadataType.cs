using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class StringAttributeMetadataType: IAttributeMetadataType
    {
        public StringAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            AttributeMetadata = new StringAttributeMetadata()
            {
                LogicalName = CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix),
                SchemaName = CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix),
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled),
                MaxLength = attribute.MaxLength,
            };
        }

        public string Name => nameof(StringAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
