using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMInsurance
    {
        public class CInsurance
        {
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }

        }

        public class GInsurance
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string Address { get; set; }
            public string OfficePhoneNum { get; set; }
            public string Email { get; set; }
            public string Website { get; set; }


        }

    }
}
