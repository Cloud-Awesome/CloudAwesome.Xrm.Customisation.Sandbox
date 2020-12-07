using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class IntegerAttributeMetadataType: IAttributeMetadataType
    {
        public IntegerAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            var schemaName = string.IsNullOrEmpty(attribute.SchemaName)
                ? CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix)
                : attribute.SchemaName;

            AttributeMetadata = new IntegerAttributeMetadata()
            {
                LogicalName = schemaName,
                SchemaName = schemaName,
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled),
                MinValue = attribute.MinValue,
                MaxValue = attribute.MaxValue
            };
        }

        public string Name => nameof(IntegerAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
