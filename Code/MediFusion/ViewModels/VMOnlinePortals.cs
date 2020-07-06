using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMCOnlinePortals
    {

        public class COnlinePortal
        {
            public string PortalType { get; set; }
            public string Name { get; set; }

        }


        public class GOnlinePortal
        {
            public long ID { get; set; }
            public string PortalType { get; set; }

            public string Name { get; set; }
            public string URL { get; set; }

            public string InsurancePlan { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

    }
}