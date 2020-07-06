using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MediFusionPM.Models
{

    public class Visit
    {
        [Required]
        public long ID { get; set; }

        public string PageNumber { get; set; }

        [ForeignKey("ID")]
        public long? BatchDocumentID { get; set; }
        public virtual BatchDocument BatchDocument { get; set; }


        [ForeignKey("ID")]
        public long ClientID { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        [ForeignKey("ID")]
        public long LocationID { get; set; }
        public virtual Location Location { get; set; }
        [ForeignKey("ID")]
        public long POSID { get; set; }
        public virtual POS POS { get; set; }
         [ForeignKey("ID")]
        public long? PatientAppointmentID { get; set; }
		public virtual PatientAppointment PatientAppointment {get;set;}
        [ForeignKey("ID")]
        public long ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [ForeignKey("ID")]
        public long? RefProviderID { get; set; }
        public virtual RefProvider RefProvider { get; set; }
        [ForeignKey("ID")]
        public long? SupervisingProvID { get; set; }
        public virtual Provider SupervisingProv { get; set; }
        [ForeignKey("ID")]
        public long? OrderingProvID { get; set; }
        public virtual Provider OrderingProv { get; set; }
        [ForeignKey("AttendingProID")]
        public long? AttendingProviderID { get; set; }
        public virtual Provider AttendingProID { get; set; }
        [ForeignKey("OperatingProID")]
        public long? OperatingProviderID { get; set; }
        public virtual Provider OperatingProID { get; set; }

        [ForeignKey("ID")]
        public long PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("ID")]
        public long? PrimaryPatientPlanID { get; set; }
        public PatientPlan PrimaryPatientPlan { get; set; }

        [ForeignKey("ID")]
        public long? SecondaryPatientPlanID { get; set; }
        public PatientPlan SecondaryPatientPlan { get; set; }

        [ForeignKey("ID")]
        public long? TertiaryPatientPlanID { get; set; }
        public PatientPlan TertiaryPatientPlan { get; set; }
        [ForeignKey("ID")]
        public long? SubmissionLogID { get; set; }
        public virtual SubmissionLog SubmissionLog { get; set; }

        [ForeignKey("SubmissionLog2")]
        public long? SubmissionLogID2 { get; set; }
        public virtual SubmissionLog SubmissionLog2 { get; set; }
        [ForeignKey("SubmissionLog3")]
        public long? SubmissionLogID3 { get; set; }
        public virtual SubmissionLog SubmissionLog3 { get; set; }
        public string RejectionReason { get; set; }

        [ForeignKey("ID")]
        public long ICD1ID { get; set; }
        public virtual ICD ICD1 { get; set; }

        [ForeignKey("ID")]
        public long? ICD2ID { get; set; }
        public virtual ICD ICD2 { get; set; }

        [ForeignKey("ID")]
        public long? ICD3ID { get; set; }
        public virtual ICD ICD3 { get; set; }

        [ForeignKey("ID")]
        public long? ICD4ID { get; set; }
        public virtual ICD ICD4 { get; set; }

        [ForeignKey("ID")]
        public long? ICD5ID { get; set; }
        public virtual ICD ICD5 { get; set; }

        [ForeignKey("ID")]
        public long? ICD6ID { get; set; }
        public virtual ICD ICD6 { get; set; }

        [ForeignKey("ID")]
        public long? ICD7ID { get; set; }
        public virtual ICD ICD7 { get; set; }

        [ForeignKey("ID")]
        public long? ICD8ID { get; set; }
        public virtual ICD ICD8 { get; set; }

        [ForeignKey("ID")]
        public long? ICD9ID { get; set; }
        public virtual ICD ICD9 { get; set; }

        [ForeignKey("ID")]
        public long? ICD10ID { get; set; }
        public virtual ICD ICD10 { get; set; }

        [ForeignKey("ID")]
        public long? ICD11ID { get; set; }
        public virtual ICD ICD11 { get; set; }
        [ForeignKey("ID")]
        public long? ICD12ID { get; set; }
        public virtual ICD ICD12 { get; set; }
        public string AuthorizationNum { get; set; }
        public bool OutsideReferral { get; set; }
        public bool? IsForcePaper { get; set; }
        public bool? IsDontPrint { get; set; }
        public string ReferralNum { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? OnsetDateOfIllness { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? FirstDateOfSimiliarIllness { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? IllnessTreatmentDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfPregnancy { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? AdmissionDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DischargeDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? LastXrayDate { get; set; }
        public string LastXrayType { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? UnableToWorkFromDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? UnableToWorkToDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? AccidentDate { get; set; }
        [MaxLength(2)]
        public string AccidentState { get; set; }
        public string AccidentType { get; set; }
        public string CliaNumber { get; set; }
        public bool? OutsideLab { get; set; }
        public decimal? LabCharges { get; set; }
        public string ClaimNotes { get; set; }
        public string PayerClaimControlNum { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string ServiceAuthExcpCode { get; set; }
        public string ValidationMessage { get; set; }
        public bool? Emergency { get; set; }
        public bool? EPSDT { get; set; }
        public bool? FamilyPlan { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Copay { get; set; }
        public decimal? CopayPaid { get; set; }
        public decimal? Coinsurance { get; set; }
        public decimal? Deductible { get; set; }
        public decimal? OtherPatResp { get; set; }
        //Need To check ec column didnt found in visit Table
        [MaxLength(15)]
        public string PrimaryStatus { get; set; }

        public decimal? PrimaryBilledAmount { get; set; }
        public decimal? PrimaryAllowed { get; set; }
        public decimal? PrimaryWriteOff { get; set; }
        public decimal? PrimaryPaid { get; set; }
        //public decimal? PrimaryPatResp { get; set; }
        public decimal? PrimaryBal { get; set; }
        public decimal? PrimaryPatientBal { get; set; }
        public decimal? PrimaryTransferred { get; set; }

        public decimal? MovedToAdvancePayment { get; set; }
        public string SecondaryStatus { get; set; }

        public decimal? SecondaryBilledAmount { get; set; }
        public decimal? SecondaryAllowed { get; set; }
        public decimal? SecondaryWriteOff { get; set; }
        public decimal? SecondaryPaid { get; set; }
        public decimal? SecondaryPatResp { get; set; }
        public decimal? SecondaryBal { get; set; }
        public decimal? SecondaryPatientBal { get; set; }
        public decimal? SecondaryTransferred { get; set; }


        public string TertiaryStatus { get; set; }
        public decimal? TertiaryBilledAmount { get; set; }
        public decimal? TertiaryAllowed { get; set; }
        public decimal? TertiaryWriteOff { get; set; }
        public decimal? TertiaryPaid { get; set; }
        public decimal? TertiaryPatResp { get; set; }
        public decimal? TertiaryBal { get; set; }
        public decimal? TertiaryPatientBal { get; set; }
        public decimal? TertiaryTransferred { get; set; }






        //public decimal ?PrimaryBilledAmount { get; set; }
        //    public decimal ?PrimaryPlanAmount { get; set; }
        //    public decimal ?PrimaryPlanAllowed { get; set; }
        //    public decimal ?PrimaryPlanPaid { get; set; }
        //    public decimal ?PrimaryWriteOff { get; set; }
        //    public decimal? SecondaryBilledAmount { get; set; }
        //    public decimal ?SecondaryPlanAmount { get; set; }
        //    public decimal ?SecondaryPlanAllowed { get; set; }
        //    public decimal ?SecondaryPlanPaid { get; set; }
        //    public decimal ?SecondaryWriteOff { get; set; }
        //    public decimal? TertiaryBilledAmount { get; set; }
        //    public decimal ?TertiaryPlanAmount { get; set; }
        //    public decimal ?TertiaryPlanAllowed { get; set; }
        //    public decimal ?TertiaryPlanPaid { get; set; }
        //    public decimal ?TertiaryWriteOff { get; set; }


        public decimal? PatientAmount { get; set; }
        public decimal? PatientPaid { get; set; }
        public bool IsSubmitted { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? SubmittedDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfServiceFrom { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfServiceTo { get; set; }

         [ForeignKey("VisitID")]
        public ICollection<Charge> Charges { get; set; }

        [ForeignKey("VisitID")]
        public ICollection<PatientPayment> PatientPayments { get; set; }

        public string AddedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }

        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        public ICollection<Notes> Note { get; set; }
         public int? StatementStatus { get; set; }
        public DateTime? LastStatementDate { get; set; }
        public DateTime? LastSeenDate { get; set; }
        public string ExternalInvoiceNumber { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? PrimaryPaymentDate { get; set; }
        public DateTime? SecondaryPaymentDate { get; set; }
        public string PrescribingMD { get; set; }
        public bool? IsReversalApplied { get; set; }
        public bool? DocumentBatchApplied { get; set; }
        public string WriteOffReason { get; set; }
        public bool? HoldStatement { get; set; }
        [NotMapped]
        public Object InstitutionalData { get; set; }
        [MaxLength(1)]
        public string VisitType { get; set; }
        public bool IsResubmitted { get; set; }
        public string ExtraColumn { get; set; }
    }
}


