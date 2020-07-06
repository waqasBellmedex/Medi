using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._277CA
{
    public class ClaimAckHeader
    {
        public string FilePath { get; set; }
        public string FileContents { get; set; }

        public string ISA06SenderID { get; set; }
        public string ISA08ReceiverID { get; set; }
        public DateTime? ISADateTime { get; set; }
        public string ISAControlNumber { get; set; }
        public string GS02SenderID { get; set; }
        public string GS03ReceiverID { get; set; }
        public string GSControlNumber { get; set; }
        public string STControlNumber { get; set; }
        public string VersionNumber { get; set; }

        // Information Source
        public string PayerEntity { get; set; }
        public string PayerOrgName { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerID { get; set; }
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string PayerContactPerson { get; set; }
        public string PayerContactNum { get; set; }
        public string PayerTRN { get; set; }

        public DateTime ?PayerRecepitDate_050 { get; set; }
        public DateTime ?PayerProcessDate_009 { get; set; }

        // Information Receiver
        public string SubmitterOrgName { get; set; }
        public string SubmitterFirstName { get; set; }
        public string SubmitterEdiID { get; set; }

        public string SubmitterTRN { get; set; }
        public string SubmitterCategoryCode { get; set; }
        public string SubmitterStatusCode { get; set; }
        public string SubmitterEntityCode { get; set; }
        public DateTime ?SubmitterStatusDate { get; set; }
        public string SubmitterActionCode { get; set; }
        public decimal SubmitterClaimsAmt { get; set; }

        public long SubmitterAcceptedClaims { get; set; }
        public long SubmitterRejectedClaims { get; set; }
        public decimal SubmitterAcceptedAmt { get; set; }
        public decimal SubmitterRejectedAmt { get; set; }

        public List<ClaimAckVisit> ClaimAckVisits { get; set; }
    }
}
