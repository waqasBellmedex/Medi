using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;

namespace MediFusionPM.Models
{

    public class PaymentCheck
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long? ReceiverID { get; set; }
        public virtual Receiver Receiver { get; set; }
        [ForeignKey("ID")]
        public long? PracticeID { get; set; }
        public virtual Practice Practice { get; set; }

        //[ForeignKey("ID")]
        //public long? BatchDocumentID { get; set; }
        //public virtual BatchDocument BatchDocument { get; set; }

        public long? DownloadedFileID { get; set; }
        [MaxLength(50)]
        public string CheckNumber { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? CheckDate { get; set; }
        public decimal? CheckAmount { get; set; }

        // BPR01
        public string TransactionCode { get; set; }
        // BPR03
        public string CreditDebitFlag { get; set; }
        // BPR04
        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        public string PayerName { get; set; }

        [ForeignKey("PaymentCheckID")]
        public ICollection<PaymentVisit> PaymentVisit { get; set; }

        [MaxLength(15)]
        public string Status { get; set; }
        public decimal? AppliedAmount { get; set; }
        public decimal? UnAppliedAmount { get; set; }
        public decimal? PostedAmount { get; set; }

        public DateTime ?PostedDate { get; set; }
        public string PostedBy { get; set; }

        public string PayerID { get; set; }
        [MaxLength(55)]
        public string PayerAddress { get; set; }
        [MaxLength(20)]
        public string PayerCity { get; set; }
        [MaxLength(2)]
        public string PayerState { get; set; }
        [MaxLength(9)]
        public string PayerZipCode { get; set; }
        public string REF_2U_ID { get; set; }
        public string PayerContactPerson { get; set; }
        public string PayerContactNumber { get; set; }
        public string PayeeName { get; set; }
        public string PayeeNPI { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZipCode { get; set; }
        public string PayeeTaxID { get; set; }
        public long NumberOfVisits { get; set; }
        public long NumberOfPatients { get; set; }
  
        public string Comments { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("ID")]
        public long? BatchDocumentID { get; set; }
        public virtual BatchDocument BatchDocument { get; set; }
        public string PageNumber { get; set; }
        public bool? DocumentBatchApplied { get; set; }
        public string ExtraColumn { get; set; }
    }

}

