using System.Xml.Serialization;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class Entity
    {
        public string DisplayName { get; set; }
        public string PluralName { get; set; }
        public string SchemaName { get; set; }
        public string Description { get; set; }
        public OwnershipTypes OwnershipType { get; set; }
        public string PrimaryAttributeName { get; set; }
        public int? PrimaryAttributeMaxLength { get; set; }
        public string PrimaryAttributeDescription { get; set; }
        public bool IsActivity { get; set; }
        public bool HasActivities { get; set; }
        public bool HasNotes { get; set; }
        public bool IsQuickCreateEnabled { get; set; }
        public bool IsAuditEnabled { get; set; }
        public bool IsDuplicateDetectionEnabled { get; set; }
        public bool IsBusinessProcessEnabled { get; set; }
        public bool IsDocumentManagementEnabled { get; set; }
        public bool IsValidForQueue { get; set; }
        public bool ChangeTrackingEnabled { get; set; }
        public string NavigationColour { get; set; }

        public Attribute[] Attributes { get; set; }

        [XmlArrayItem("Permissions")]
        public EntityPermission[] EntityPermissions { get; set; }
    }
}
