using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class SubmittedCharge
    {
        public long? VisitID { get; set; }
        public string  ProcessedAs { get; set; }
        public string PaymentMethod { get; set; }

    }
}
