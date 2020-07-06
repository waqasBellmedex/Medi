using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class Charge
    {
        public long ID { get; set; }


        public long? VisitID { get; set; }

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


        [ForeignKey("ID")]
        public long CPTID { get; set; }
        public virtual Cpt Cpt { get; set; }

        [ForeignKey("ID")]
        public long? Modifier1ID { get; set; }
        public virtual Modifier Modifier1 { get; set; }
        public decimal? Modifier1Amount { get; set; }

        [ForeignKey("ID")]
        public long? Modifier2ID { get; set; }
        public virtual Modifier Modifier2 { get; set; }
        public decimal? Modifier2Amount { get; set; }

        [ForeignKey("ID")]
        public long? Modifier3ID { get; set; }
        public virtual Modifier Modifier3 { get; set; }
        public decimal? Modifier3Amount { get; set; }

        [ForeignKey("ID")]
        public long? Modifier4ID { get; set; }
        public virtual Modifier Modifier4 { get; set; }
        public decimal? Modifier4Amount { get; set; }
        public string Units { get; set; }
        public string Minutes { get; set; }
        public int? NdcUnits { get; set; }
        [ForeignKey("ID")]
        public long? AppointmentCPTID { get; set; }
      // public virtual AppointmentCPT AppointmentCPT { get; set; }
        public string NdcMeasurementUnit { get; set; }
        public string Description { get; set; }
        public string NdcNumber { get; set; }
        public string UnitOfMeasurement { get; set; }
        [Column(TypeName = "Date")]
        public DateTime DateOfServiceFrom { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ?TimeUnits { get; set; }
        public int ?BaseUnits { get; set; }
        public int ?ModifierUnits { get; set; }
        public string Pointer1 { get; set; }
        public string Pointer2 { get; set; }
        public string Pointer3 { get; set; }
        public string Pointer4 { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ?Copay { get; set; }
        public decimal? Coinsurance { get; set; }
        public decimal? Deductible { get; set; }
        public decimal? OtherPatResp { get; set; }
        public decimal? PrimaryBilledAmount { get; set; }
        public decimal? PrimaryAllowed { get; set; }
        public decimal? PrimaryWriteOff { get; set; }
        public decimal? PrimaryPaid { get; set; }
        //public decimal? PrimaryPatResp { get; set; }
        public decimal? PrimaryBal { get; set; }
        public decimal? PrimaryPatientBal { get; set; }
        public decimal? PrimaryTransferred { get; set; }
        [MaxLength(15)]
        public string PrimaryStatus { get; set; }
        public decimal? SecondaryBilledAmount { get; set; }
        public decimal? SecondaryAllowed { get; set; }
        public decimal? SecondaryWriteOff { get; set; }
        public decimal? SecondaryPaid { get; set; }
        public decimal? SecondaryPatResp { get; set; }
        public decimal? SecondaryBal { get; set; }
        public decimal? SecondaryPatientBal { get; set; }
        public decimal? SecondaryTransferred { get; set; }
        public string SecondaryStatus { get; set; }

        public decimal? TertiaryBilledAmount { get; set; }
        public decimal? TertiaryAllowed { get; set; }
        public decimal? TertiaryWriteOff { get; set; }
        public decimal? TertiaryPaid { get; set; }
        public decimal? TertiaryPatResp { get; set; }
        public decimal? TertiaryBal { get; set; }
        public decimal? TertiaryPatientBal { get; set; }
        public decimal? TertiaryTransferred { get; set; }
        public string TertiaryStatus { get; set; }
        public decimal? PatientAmount { get; set; }
        public decimal? PatientPaid { get; set; }
        public bool IsSubmitted { get; set; }
        public bool? IsDontPrint { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? SubmittetdDate { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        public string RejectionReason { get; set; }

        [ForeignKey("ID")]
        public long? RevenueCodeID { get; set; }
        public virtual RevenueCode RevenueCode { get; set; }

        public decimal? Discount { get; set; }
        public DateTime? PrimaryPaymentDate { get; set; }
        public DateTime? SecondaryPaymentDate { get; set; }
        public string AuthorizationNum { get; set; }
        public bool? IsReversalApplied { get; set; }
        public string WriteOffReason { get; set; }
        [ForeignKey("ID")]
        public long? AttendingProviderID { get; set; }
        public virtual Provider AttendingProvider { get; set; }
        public bool IsResubmitted { get; set; }
        public string ExtraColumn { get; set; }

    }

}
