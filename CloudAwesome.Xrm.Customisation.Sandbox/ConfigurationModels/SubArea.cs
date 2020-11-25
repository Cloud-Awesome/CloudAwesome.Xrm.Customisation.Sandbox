using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public enum SubAreaType { Entity = 0}
    public class SubArea
    {
        public SubArea Type { get; set; }
        public string Entity { get; set; }
        public string Title { get; set; }
    }
}
