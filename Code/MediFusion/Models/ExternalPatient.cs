using MediFusionPM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class ExternalPatient
    {
        [Required]
        public long ID { get; set; }
        public string ExternalPatientID { get; set; }
        public string ProfilePic { get; set; }
        public string AccountNum { get; set; }
        public string AccountType { get; set; }
        public string PreferredLanguage { get; set; }
        public string Status { get; set; }
        public string MergeStatus { get; set; }
        public string MedicalRecordNumber { get; set; }
        [MaxLength]
        public string LastName { get; set; }
        [MaxLength]
        public string FirstName { get; set; }
        [MaxLength]
        public string MiddleInitial { get; set; }
        public string Title { get; set; }
        [StringLength(100)]
        public string SSN { get; set; }
        // [DataType(DataType.DateTime)]
        [Column(TypeName = "Date")]
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        [MaxLength]
        public string Address1 { get; set; }
        [MaxLength]
        public string Address2 { get; set; }
        [MaxLength]
        public string City { get; set; }
        [MaxLength]
        public string State { get; set; }
        [MaxLength]
        public string ZipCode { get; set; }
        public string ZipCodeExtension { get; set; }
        // [DataType(DataType.PhoneNumber)]
        [MaxLength]
        public string MobileNumber { get; set; }
        //[DataType(DataType.PhoneNumber)]
        [MaxLength]
        public string PhoneNumber { get; set; }

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
        public string FileName { get; set; }
        public string GuarantarID { get; set; }
        public string GuarantarName { get; set; }
        public string PrimaryInsuredID { get; set; }
        public string PrimaryInsuredName { get; set; }
        public string PrimaryInsurance { get; set; }
        public string SecondaryInsuredID { get; set; }
        public string SecondaryInsuredName { get; set; }
        public string SecondaryInsurance { get; set; }
        public string PrimaryProvider { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public string EmployerCity { get; set; }
        public string EmployerState { get; set; }
        public string EmployerZip { get; set; }
        public string EmployerPhone { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyAddress { get; set; }
        public string EmergencyCity { get; set; }
        public string EmergencyState { get; set; }
        public string EmergencyZip { get; set; }
        public string EmergencyPhone { get; set; }
        public string PracticeNPI { get; set; }
        public string LocationNPI { get; set; }
        public string ProviderNPI { get; set; }
        public long? PrimaryPatientPlanID { get; set; }
        public long? SecondaryPatientPlanID { get; set; }
        public string PrimaryGroup { get; set; }
        public string PrimaryDescription { get; set; }
        public string SecondaryDescription { get; set; }
        public string SecondaryGroup { get; set; }
        public bool? NeedInsuranceCard { get; set; }
        public string PrescribingMD { get; set; }
        public string Modified { get; set; }
        public string Created { get; set; }
        public string ModifiedBy { get; set; }
        public string MissingInfo { get; set; }
        public bool IsRegularRecord { get; set; }
        public bool resolve { get; set; }
        public string ResolvedErrorMessage { get; set; }

    }
}
