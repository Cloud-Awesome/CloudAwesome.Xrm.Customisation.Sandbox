using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class DateTimeAttributeMetadataType: IAttributeMetadataType
    {
        public DateTimeAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            var schemaName = string.IsNullOrEmpty(attribute.SchemaName)
                ? CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix)
                : attribute.SchemaName;

            AttributeMetadata = new DateTimeAttributeMetadata()
            {
                LogicalName = schemaName,
                SchemaName = schemaName,
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled),
                Format = attribute.DateTimeFormat
                // TODO - behaviour (User local vs. TZ-independent)
            };
        }

        public string Name => nameof(DateTimeAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
