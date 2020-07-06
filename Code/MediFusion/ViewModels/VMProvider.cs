using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMProvider
    {
        public class CProvider
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string NPI { get; set; }
            public string SSN { get; set; }
            public string TaxonomyCode { get; set; }

        }

        public class GProvider
        {   
            public long? ID { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string NPI { get; set; }
            public string SSN { get; set; }
            public string TaxonomyCode { get; set; }
            public string Address { get; set; }
            public string OfficePhoneNum { get; set; }

        }


    }
}
