using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels.Main
{
    public class VMExInsuranceMapping
    {

      public class CExInsuranceMapping
        {
            public string ExternalInsuranceName { get; set; }
            public long? InsurancePlanID { get; set; }
            public string Status { get; set; }
            public DateTime? ToDate { get; set; }
            public DateTime? FromDate { get; set; }

        }


        public class GExInsuranceMapping
        {
            public long ID { get; set; }
            public string ExternalInsuranceName { get; set; }
            public string InsurancePlanID { get; set; }
            public string PlanName { get; set; }
            public string AddedBy { get; set; }
            public string AddedDate { get; set; }
            public string Status { get; set; }

        }
    }
}
