using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Attribute = CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels.Attribute;

namespace CloudAwesome.Xrm.Customisation.Sandbox.AttributeMetadataTypes
{
    public class LookupAttributeMetadataType: IAttributeMetadataType
    {
        public LookupAttributeMetadataType(Attribute attribute, string publisherPrefix)
        {
            throw new NotImplementedException("TODO!!");

            var schemaName = string.IsNullOrEmpty(attribute.SchemaName)
                ? CustomisationHelpers.CreateLogicalNameFromDisplayName(attribute.DisplayName, publisherPrefix)
                : attribute.SchemaName;

            AttributeMetadata = new LookupAttributeMetadata()
            {
                LogicalName = schemaName,
                SchemaName = schemaName,
                DisplayName = CustomisationHelpers.CreateLabelFromString(attribute.DisplayName),
                Description = CustomisationHelpers.CreateLabelFromString(attribute.Description),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(attribute.RequiredLevel),
                IsAuditEnabled = new BooleanManagedProperty(attribute.IsAuditEnabled)
            };
        }

        public string Name => nameof(LookupAttributeMetadataType);
        public AttributeMetadata AttributeMetadata { get; }
    }
}
