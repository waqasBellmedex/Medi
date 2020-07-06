using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class Jobs
    {
        public class CTable
        {
            public long? VisitID { get; set; }
            public long? PracticeID { get; set; }
            public long? LocationID { get; set; }
            public long? ProviderID { get; set; }
            public long? PrimaryPatientPlanID { get; set; }
            public long? PatientID { get; set; }

            public long? SubmittedDateID { get; set; }
            public long? PrimaryPaidID { get; set; }
            public long? PlanID { get; set; }
            public long? InsuranceplanID { get; set; }




        }
    }
}
