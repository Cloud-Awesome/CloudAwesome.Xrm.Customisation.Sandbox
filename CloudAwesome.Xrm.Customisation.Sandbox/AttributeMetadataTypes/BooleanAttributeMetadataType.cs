using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class BooleanAttributeMetadataType: IAttributeMetadataType
    {
        public BooleanAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            var schemaName = string.IsNullOrEmpty(attribute.SchemaName)
                ? CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix)
                : attribute.SchemaName;

            AttributeMetadata = new BooleanAttributeMetadata()
            {
                LogicalName = schemaName,
                SchemaName = schemaName,
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled),
                DefaultValue = false,
                OptionSet = new BooleanOptionSetMetadata()
                {
                    TrueOption = new OptionMetadata()
                    {
                        Label = CustomisationHelpers.CreateLabelFromString("Yes")
                    },
                    FalseOption = new OptionMetadata()
                    {
                        Label = CustomisationHelpers.CreateLabelFromString("No")
                    }
                }
            };
        }

        public string Name => nameof(BooleanAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
