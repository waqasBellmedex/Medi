using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class Provider
    {
        [Required]
        public long ID { get; set; }
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        [Required]
        [MaxLength(35)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(35)]
        public string FirstName { get; set; }
        [MaxLength(3)]
        public string MiddleInitial { get; set; }
        [MaxLength(15)]
        public string Title { get; set; }
        [Required]
        [StringLength(10)]
        public string NPI { get; set; }
        //[Required] // removed Required From SSN
        [StringLength(9)]
        public string SSN { get; set; }
        [MaxLength(10)]
        public string TaxonomyCode { get; set; }
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
        [StringLength(10)]
        public string OfficePhoneNum { get; set; }
        [MaxLength(4)]
        public string PhoneNumExt { get; set; }
        [MaxLength(10)]
        public string FaxNumber { get; set; }
        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        public bool? BillUnderProvider { get; set; }
        public bool? ReportTaxID { get; set; }

        [MaxLength(60)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string Email { get; set; }
        public string DEANumber { get; set; }
        public string UPINNumber { get; set; }
        public string LicenceNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string AddedBy { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }

        [MaxLength(55)]
        public string PayToAddress1 { get; set; }
        [MaxLength(55)]
        public string PayToAddress2 { get; set; }
        [MaxLength(20)]
        public string PayToCity { get; set; }
        [MaxLength(2)]
        public string PayToState { get; set; }
        [MaxLength(9)]
        public string PayToZipCode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<InsuranceBillingoption> InsuranceBillingoption { get; set; }
        [NotMapped]
        public virtual ICollection<ProviderSchedule> ProviderSchedule { get; set; }
        public string ReferralDocumentFileName { get; set; }



    }

}

