using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public class PluginManifest
    {
        public string Random { get; set; }

        public Plugin[] Plugins { get; set; }
    }
}
