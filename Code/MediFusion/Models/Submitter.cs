using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MediFusionPM.Models
{
    public class Submitter
    {
        public long ID { get; set; }
        public string Name { get; set; }
        [MaxLength(55)]
        public string Address { get; set; }
        [MaxLength(55)]
        public string Address2 { get; set; }

        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(9)]
        public string ZipCode { get; set; }

        public string SubmissionUserName { get; set; }
        public string SubmissionPassword { get; set; }
        public bool ManualSubmission { get; set; }
        public string FileName { get; set; }


        public string X12_837_NM1_41_SubmitterName { get; set; }
        public string X12_837_NM1_41_SubmitterID { get; set; }

        public string X12_837_ISA_02 { get; set; }
        public string X12_837_ISA_04 { get; set; }

        public string X12_837_ISA_06 { get; set; }

        public string X12_837_GS_02 { get; set; }


        public string SubmitterContactPerson { get; set; }
        public string SubmitterContactNumber { get; set; }
        [MaxLength(60)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string SubmitterEmail { get; set; }
        public string SubmitterFaxNumber { get; set; }


        public string X12_270_NM1_41_SubmitterName { get; set; }
        public string X12_270_NM1_41_SubmitterID { get; set; }

        public string X12_270_ISA_02 { get; set; }
        public string X12_270_ISA_04 { get; set; }

        public string X12_270_ISA_06 { get; set; }

        public string X12_270_GS_02 { get; set; }


        public string X12_276_NM1_41_SubmitterName { get; set; }
        public string X12_276_NM1_41_SubmitterID { get; set; }

        public string X12_276_ISA_02 { get; set; }
        public string X12_276_ISA_04 { get; set; }

        public string X12_276_ISA_06 { get; set; }

        public string X12_276_GS_02 { get; set; }

        public long ReceiverID { get; set; }
        public virtual Receiver Receiver { get; set; }
        public long ClientID { get; set; }
        public virtual Client Client { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? AutoDownloading { get; set; }
        public bool? AutoSubmission { get; set; }
    }
}
