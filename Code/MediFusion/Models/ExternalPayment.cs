using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class ExternalPayment
    {
        public long ID { get; set; }
        public string ExternalChargeID { get; set; }
        public string ExternalPatientID { get; set; }

        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        public virtual Practice Practice { get; set; }

        [ForeignKey("ID")]
        public long LocationID { get; set; }
        public virtual Location Location { get; set; }
 
        public string POSCode { get; set; }
        public long? POSID { get; set; }
 
        public string ProviderName { get; set; }
        [ForeignKey("ID")]
        public long ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [ForeignKey("ID")]
        public long? CPTID { get; set; }
        public virtual Cpt Cpt { get; set; }

        public string CptCode { get; set; }
        public string DaysOrUnits { get; set; }
        [Column(TypeName = "Date")]
        public DateTime DateOfService{ get; set; }
        public decimal Charges { get; set; }
        public decimal Balance { get; set; }
        public decimal Adj { get; set; }
        public decimal InsurancePayment { get; set; }
        //public decimal? PrimaryPatResp { get; set; }
        public decimal PatientPayment { get; set; }
        public DateTime SubmittetdDate { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        public string PatientName { get; set; }
        public string MergeStatus { get; set; }
        public string InsuranceName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CheckDate { get; set; }
        public string CheckNumber { get; set; }
    }
}
