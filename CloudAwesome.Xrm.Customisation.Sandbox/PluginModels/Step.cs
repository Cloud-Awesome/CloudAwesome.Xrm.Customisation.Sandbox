using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox.PluginModels
{
    public enum ExecutionStage { PreValidation, PreOperation, PostOperation }

    public enum ExecutionMode { Synchronous, Asynchronous }

    public class Step
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public ExecutionStage Stage { get; set; }

        public ExecutionMode ExecutionMode { get; set; }

        public string Message { get; set; }

        public string PrimaryEntity { get; set; }

        public int ExecutionOrder { get; set; }

        public bool AsyncAutoDelete { get; set; }

        public string UnsecureConfiguration { get; set; }

        public string SecureConfiguration { get; set; }

        [XmlArrayItem("Attribute")]
        public string[] FilteringAttributes { get; set; }

        public EntityImage[] EntityImages { get; set; }
    }
}
