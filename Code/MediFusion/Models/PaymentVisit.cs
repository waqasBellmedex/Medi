using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    namespace TodoApi.Models
    {
        public class PaymentVisit
        {
            public long ID { get; set; }
            
           // [ForeignKey("ID")]
            public long? PaymentCheckID { get; set; }
            //public virtual PaymentCheck PaymentCheck { get; set; }
            [ForeignKey("ID")]
            public long? VisitID { get; set; }
            public virtual Visit Visit { get; set; }
            [ForeignKey("ID")]
            public long? PatientID { get; set; }
            public virtual Patient Patient { get; set; }
            public string ClaimNumber { get; set; }
           [ForeignKey("PaymentVisitID")]
            public ICollection<PaymentCharge> PaymentCharge { get; set; }


            [ForeignKey("ID")]
            public long? BatchDocumentID { get; set; }
            public virtual BatchDocument BatchDocument { get; set; }

            public string PageNumber { get; set; }


            public int MyProperty { get; set; }

            // CLP02
            public string ProcessedAs { get; set; }
            public decimal? BilledAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? WriteOffAmount { get; set; }
            public decimal? PatientAmount { get; set; }
            public string PayerICN { get; set; }

            [MaxLength(30)]
            public string PatientLastName { get; set; }
            [MaxLength(30)]
            public string PatientFIrstName { get; set; }
            [MaxLength(30)]
            public string InsuredLastName { get; set; }
            [MaxLength(30)]
            public string InsuredFirstName { get; set; }
            public string InsuredID { get; set; }

            [MaxLength(30)]
            public string ProvLastName { get; set; }
            [MaxLength(30)]
            public string ProvFirstName { get; set; }
            public string ProvNPI { get; set; }
            [MaxLength(10)]
            public string PayerContactNumber { get; set; }
            public string ForwardedPayerName { get; set; }
            public string ForwardedPayerID { get; set; }

            public DateTime? ClaimStatementFromDate { get; set; }
            public DateTime? ClaimStatementToDate { get; set; }
            public DateTime? PayerReceivedDate { get; set; }


            public string Comments { get; set; }

            public string Status { get; set; }
            public DateTime? PostedDate { get; set; }
            public string PostedBy { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            [NotMapped]
            public string PatientName { get; set; }


        }


    }
}
