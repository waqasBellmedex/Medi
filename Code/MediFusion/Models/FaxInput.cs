using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class FaxInput
    {
        public string PCP { get; set; }
        public string PatientName { get; set; }
        public string PatientDOB { get; set; }
        public string ServiceName { get; set; }
        public string Provider { get; set; }
        public string FaxNumber { get; set; }
        public long PCBProviderID { get; set; }
        public long ProviderID { get; set; }
        public string ReferralDocumentFileName { get; set; }

    }
}
