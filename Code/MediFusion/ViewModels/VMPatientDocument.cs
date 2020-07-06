using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMPatientDocument
    {
    }
    public class PEmailSend
    {
        public string toEmail { get; set; }
        public string body { get; set; }

        public string subject { get; set; }
        public string CC { get; set; }
        public string attachment { get; set; }
    }
}
