using Microsoft.Xrm.Sdk.Metadata;
using CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class MemoAttributeMetadataType: IAttributeMetadataType
    {
        public MemoAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            var schemaName = string.IsNullOrEmpty(attribute.SchemaName)
                ? CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix)
                : attribute.SchemaName;

            AttributeMetadata = new MemoAttributeMetadata()
            {
                LogicalName = schemaName,
                SchemaName = schemaName,
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled),
                MaxLength = attribute.MaxLength == 0 ? 1000 : attribute.MaxLength,
                Format = StringFormat.TextArea
            };
        }

        public string Name => nameof(MemoAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
