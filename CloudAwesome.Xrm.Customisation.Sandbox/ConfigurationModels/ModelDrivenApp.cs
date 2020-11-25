using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class ModelDrivenApp
    {
        public string Name { get; set; }

        public string UniqueName { get; set; }

        public string Description { get; set; }

        public SiteMap SiteMap { get; set; }
    }
}
