using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class PicklistAttributeMetadataType: IAttributeMetadataType
    {
        public PicklistAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            var schemaName = string.IsNullOrEmpty(attribute.SchemaName)
                ? CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix)
                : attribute.SchemaName;

            AttributeMetadata = new PicklistAttributeMetadata()
            {
                LogicalName = schemaName,
                SchemaName = schemaName,
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled),
                OptionSet = new OptionSetMetadata()
                {
                    Name = attribute.GlobalOptionSet,
                    IsGlobal = true,
                    OptionSetType = OptionSetType.Picklist
                }
            };
        }

    public string Name => nameof(PicklistAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
