using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class SubmissionLog
    {
        [Required]
        public long ID { get; set; }

        public long ClientID { get; set; }
        public long ReceiverID { get; set; }
        public long SubmitterID { get; set; }
        public string SubmitType { get; set; }
        public long ?DownloadedFileID { get; set; }

        public string PdfPath { get; set; }
        public string FormType { get; set; }

        public string ISAControlNumber { get; set; }
        public long ClaimCount { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Notes { get; set; }

        public string Transaction837Path { get; set; }

        public string IK5_Status { get; set; }
        public string AK9_Status { get; set; }

        public int? NoOfTotalST { get; set; }
        public int? NoOfReceivedST { get; set; }
        public int? NoOfAcceptedST { get; set; }

        public string IK5_ErrorCode { get; set; }
        public string AK9_ErrorCode { get; set; }

        public string Transaction999Path { get; set; }
        public string Trasaction277CAPath { get; set; }

        

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Resolve { get; set; }
    }

    public class ListSubmission
    {
        public long Id { get; set; }
        public string notes { get; set; }
    }

}

