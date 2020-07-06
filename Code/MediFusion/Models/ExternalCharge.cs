using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class ExternalCharge
    {
        public long ID { get; set; }
        public string ExternalPatientID { get; set; }
        public long? VisitID { get; set; }
        public string DOB { get; set; }
        public long? Insurance { get; set; }
        public bool IsRegularRecord { get; set; }
        public long GroupID { get; set; }

        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        public virtual Practice Practice { get; set; }

        [ForeignKey("ID")]
        public long LocationID { get; set; }
        public virtual Location Location { get; set; }
 
        public string VisitPOSCode { get; set; }
        public string POSCode { get; set; }
        public long? POSID { get; set; }
 
        public string ProviderName { get; set; }
        [ForeignKey("ID")]
        public long ProviderID { get; set; }
        public long? RefProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        public string OfficeName { get; set; }

        public string Gender { get; set; }

        [ForeignKey("ID")]
        public long? CPTID { get; set; }
        public virtual Cpt Cpt { get; set; }

        public string CptCode { get; set; }

        [ForeignKey("ID")]
        public long? Modifier1ID { get; set; }
        public virtual Modifier Modifier1 { get; set; }
        public string Modifier1Code { get; set; }

        [ForeignKey("ID")]
        public long? Modifier2ID { get; set; }
        public virtual Modifier Modifier2 { get; set; }
        public string Modifier2Code { get; set; }

        [ForeignKey("ID")]
        public long? Modifier3ID { get; set; }
        public virtual Modifier Modifier3 { get; set; }
        public string Modifier3Code { get; set; }

        [ForeignKey("ID")]
        public long? Modifier4ID { get; set; }
        public virtual Modifier Modifier4 { get; set; }
        public string Modifier4Code { get; set; }
        public string DaysOrUnits { get; set; }
        [Column(TypeName = "Date")]
        public DateTime DateOfService{ get; set; }
        public decimal Charges { get; set; }
        public decimal Balance { get; set; }
        public decimal PrimaryBal { get; set; }
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [MaxLength(10)]
        public string MergeStatus { get; set; }
        public string InsuranceName { get; set; }
        public string SecondaryInsuranceName { get; set; }
        public long? PrimaryInsuredID { get; set; }
        public long? SecondaryInsuredID { get; set; }
        public long? PatientID { get; set; }
        public string ErrorMessage { get; set; }
        public long? ChargeID { get; set; }
        [MaxLength(250)]
        public string FileName { get; set; }
        [DefaultValue("NP")]
        public string PaymentProcessed { get; set; }
        public long? PaymentCheckID { get; set; }
        //// Saad Fields are Added on Dated 03/27/2020  04.40 PM
        public string SheetName { get; set; }
        public string PrescribingMD { get; set; }
        public string ReportType { get; set; }
        public string DiagnosisCode { get; set; }
        public string NotBilled { get; set; }
        public string NeedDemos { get; set; }
        public string Remarks { get; set; }
        public bool resolve { get; set; }
        public string ResolvedErrorMessage { get; set; }
        public string MiddleInitial { get; set; }

        public string DXCode1 { get; set; }
        public string DXCode2 { get; set; }
        public string DXCode3 { get; set; }
        public string DXCode4 { get; set; }
        public string DXCode5 { get; set; }
        public string DXCode6 { get; set; }
    }
}
