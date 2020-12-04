using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class Attribute
    {
        public string DisplayName { get; set; }
        public string SchemaName { get; set; }
        public string DataType { get; set; }
        public string GlobalOptionSet { get; set; }
        public string Description { get; set; }
        public AttributeRequiredLevel RequiredLevel { get; set; }
        public bool IsAuditEnabled { get; set; }
        public string SourceType { get; set; }
        public int MaxLength { get; set; }
        public string StringFormat { get; set; }
        public string AutoNumberFormat { get; set; }
        public string DateTimeFormat { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string ReferencedEntity { get; set; }
        public string RelationshipNameSuffix { get; set; }
        public string ParentCascade { get; set; }
        public string AddToForm { get; set; }
        public string AddToViewOrder { get; set; }

    }
}
