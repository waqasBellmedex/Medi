    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class Patient
    {
        [Required]
        public long ID { get; set; }
        
        public string ProfilePic { get; set; }
       // [Required]
        [MaxLength(15)]
        public string AccountNum { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string NewAccountNum { get; set; }
        public string MedicalRecordNumber { get; set; }
        [MaxLength(35)]
        public string LastName { get; set; }
        [MaxLength(35)]
        public string FirstName { get; set; }
        [MaxLength(3)]
        public string MiddleInitial { get; set; }
        public string Title { get; set; }
        [StringLength(9)]
        public string SSN { get; set; }
        // [DataType(DataType.DateTime)]
        [Column(TypeName = "Date")]
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        [MaxLength(55)]
        public string Address1 { get; set; }
        [MaxLength(55)]
        public string Address2 { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(9)]
        public string ZipCode { get; set; }
        public string ZipCodeExtension { get; set; }
        // [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string MobileNumber { get; set; }
        //[DataType(DataType.PhoneNumber)]
        [StringLength(10)]                         
        public string PhoneNumber { get; set; }
        [MaxLength(60)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string Email { get; set; }
        [ForeignKey("ID")]
        public long? PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        [ForeignKey("ID")]
        public long? LocationId { get; set; }
        public virtual Location Location { get; set; }
        [ForeignKey("ID")]
        public long? ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [ForeignKey("ID")]
        public long? RefProviderID { get; set; }
        public virtual RefProvider RefProvider { get; set; }

        [ForeignKey("ID")]
        public long? BatchDocumentID { get; set; }
        public virtual BatchDocument BatchDocument { get; set; }
        public string PageNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        public bool? Statement { get; set; }
        public string StatementMessage { get; set; }
        public decimal? RemainingDeductible { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string ExternalPatientID { get; set; }

        [ForeignKey("PatientID")]
        public ICollection<Notes> Note { get; set; }

        [ForeignKey("PatientID")]
        public ICollection<PatientPlan> PatientPlans { get; set; }

        [ForeignKey("PatientID")]
        public ICollection<PatientAuthorization> PatientAuthorization { get; set; }
        public string PrescribingMD { get; set; }
        [ForeignKey("PatientID")]
        public ICollection<PatientReferral> PatientReferrals { get; set; }
        public bool? DocumentBatchApplied { get; set; }
        public bool? HoldStatement { get; set; }
        public string MissingInfo { get; set; }
        public bool? sendAppointmentSMS { get; set; }
        public string ExtraColumn { get; set; }
    }

}

