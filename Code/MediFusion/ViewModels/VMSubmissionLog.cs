using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMSubmissionLog
    {

        public class CSubmissionLog
        {
            public string SubmitBatchNumber { get; set; }
            public string FormType { get; set; }
            public string Receiver { get; set; }
            public DateTime? SubmitDate { get; set; }
            public string SubmitType { get; set; }
            public string ISAControllNumber { get; set; }
            public string ResolvedErrorMessage { get; set; }

        }


        public class GSubmissionLog
        {
            public long? PracticeID { get; set; }
            public long SubmitBatchNumber { get; set; }
            public string Receiver { get; set; }
            public string FormType { get; set; }
            public string SubmitDate { get; set; }
            public string Status { get; set; }
            public string ISAControllNumber { get; set; }
            public string  SubmitType { get; set; }
            public string Notes { get; set; }
            public long? NumOfVisits { get; set; }
            public long VisitCount { get; set; }
            public decimal? VisitAmount { get; set; }
            public string Resolve { get; set; }

        }




    }
}
