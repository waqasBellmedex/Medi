using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMClaimStatus
    {
        public class GClaimStatus
        {
            public long ?VisitID {get; set; }
            public string ResponseEntity { get; set; }
            public string TRNNumber { get; set; }
            public string ActionCode { get; set; }
            public string PayerName { get; set; }
            public string CategoryCode1 { get; set; }
            public string StatusCode1 { get; set; }
            public string FreeText1 { get; set; }
            public string FreeText2 { get; set; }
            public DateTime? StatusDate { get; set; }
            public string PayerControlNumber { get; set; }
            public DateTime AddedDate { get; set; }
            public long? DownloadedFile { get; set; }
        }
    }
}
