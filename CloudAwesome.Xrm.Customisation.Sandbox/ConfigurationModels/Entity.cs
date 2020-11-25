using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class Entity
    {
        public string DisplayName { get; set; }
        public string PluralName { get; set; }
        public string SchemaName { get; set; }
        public string Description { get; set; }
        public string OwnershipType { get; set; }
        public string PrimaryAttributeName { get; set; }
        public string PrimaryAttributeMaxLength { get; set; }
        public string PrimaryAttributeDescription { get; set; }
        public string IsActivity { get; set; }
        public string HasActivities { get; set; }
        public string HasNotes { get; set; }
        public string IsQuickCreateEnabled { get; set; }
        public string IsAuditEnabled { get; set; }
        public string IsDuplicateDetectionEnabled { get; set; }
        public string IsBusinessProcessEnabled { get; set; }
        public string IsDocumentManagementEnabled { get; set; }
        public string NavigationColour { get; set; }

        public Attribute[] Attributes { get; set; }

        [XmlArrayItem("Permissions")]
        public EntityPermission[] EntityPermissions { get; set; }
    }
}
