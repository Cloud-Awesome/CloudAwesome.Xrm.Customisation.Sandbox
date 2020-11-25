using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class Group
    {
        public string Name { get; set; }
        public SubArea[] SubAreas { get; set; }
    }
}
