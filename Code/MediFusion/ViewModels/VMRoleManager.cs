using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMRoleManager
    {
        public class CRoleManager
        {
            public string Role { get; set; }
            public long? DesignationID { get; set; }
            public long? TeamID { get; set; }
        }
        public class GRoleManager
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string TeamName { get; set; }
        }

    }
}
