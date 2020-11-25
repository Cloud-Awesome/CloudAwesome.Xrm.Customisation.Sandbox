using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public enum PrivilegeDepth { None = 0, User = 1, BusinessUnit = 2, ParentChild = 3, Organization = 4 }

    public class EntityPermission
    {
        public string Name { get; set; }

        public PrivilegeDepth Create { get; set; }

        public PrivilegeDepth Read { get; set; }

        public PrivilegeDepth Write { get; set; }

        public PrivilegeDepth Delete { get; set; }

        public PrivilegeDepth Append { get; set; }

        public PrivilegeDepth AppendTo { get; set; }

        public PrivilegeDepth Share { get; set; }
    }
}
