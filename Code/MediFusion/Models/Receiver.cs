using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MediFusionPM.Models
{
    public class Receiver
    {
        public long ID { get; set; }
        public string Name { get; set; }
        [MaxLength(55)]
        public string Address1 { get; set; }
        [MaxLength(55)]
        public string Address2 { get; set; }
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [StringLength(10)]
        public string FaxNumber { get; set; }
        [MaxLength(60)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string Email { get; set; }
        public string Website { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(9)]
        public string ZipCode { get; set; }

        public string SubmissionMethod { get; set; }
        public string SubmissionURL { get; set; }
        public string SubmissionPort { get; set; }

        public string SubmissionDirectory { get; set; }
        public string ReportsDirectory { get; set; }
        public string ErasDirectory { get; set; }




        public string X12_837_NM1_40_ReceiverName { get; set; }
        public string X12_837_NM1_40_ReceiverID { get; set; }

        public string X12_837_ISA_01 { get; set; }
        public string X12_837_ISA_03 { get; set; }
        public string X12_837_ISA_05 { get; set; }
        public string X12_837_ISA_07 { get; set; }
        public string X12_837_ISA_08 { get; set; }

        public string X12_837_GS_03 { get; set; }

        public string X12_270_NM1_40_ReceiverName { get; set; }
        public string X12_270_NM1_40_ReceiverID { get; set; }
        public string X12_270_ISA_01 { get; set; }
        public string X12_270_ISA_03 { get; set; }
        public string X12_270_ISA_05 { get; set; }
        public string X12_270_ISA_07 { get; set; }
        public string X12_270_ISA_08 { get; set; }
        public string X12_270_GS_03 { get; set; }

        public string X12_276_NM1_40_ReceiverName { get; set; }
        public string X12_276_NM1_40_ReceiverID { get; set; }
        public string X12_276_ISA_01 { get; set; }
        public string X12_276_ISA_03 { get; set; }
        public string X12_276_ISA_05 { get; set; }

        //internal bool Address(string address)
        //{
        //    throw new NotImplementedException();
        //}

        public string X12_276_ISA_07 { get; set; }
        public string X12_276_ISA_08 { get; set; }
        public string X12_276_GS_03 { get; set; }

        public string ElementSeperator { get; set; } = "*";
        public string SegmentSeperator { get; set; } = "~";
        public string SubElementSeperator { get; set; } = ":";
        public string RepetitionSepeator { get; set; } = "[";

        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

    }
}
