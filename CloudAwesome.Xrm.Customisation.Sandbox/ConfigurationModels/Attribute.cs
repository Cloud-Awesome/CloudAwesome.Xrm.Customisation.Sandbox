using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class Attribute
    {
        public string DisplayName { get; set; }
        public string SchemaName { get; set; }
        public string DataType { get; set; }
        public string GlobalOptionSet { get; set; } // TODO - Optionset Default Value
        public string Description { get; set; }
        public AttributeRequiredLevel RequiredLevel { get; set; }
        public bool IsAuditEnabled { get; set; }
        public string SourceType { get; set; }
        public int MaxLength { get; set; }
        public DateTimeFormat DateTimeFormat { get; set; }
        public StringFormat StringFormat { get; set; }
        public string AutoNumberFormat { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string ReferencedEntity { get; set; }
        public string RelationshipNameSuffix { get; set; }
        public string ParentCascade { get; set; }
        public bool AddToForm { get; set; }
        public int? AddToViewOrder { get; set; }

    }
}
