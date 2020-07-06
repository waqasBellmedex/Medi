using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class PatientPlan
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("ID")]
        public long? InsurancePlanID { get; set; }
        public virtual InsurancePlan InsurancePlan { get; set; }
        [MaxLength(3)]
        public string Coverage { get; set; }
        public string RelationShip { get; set; }
        [Required]
        [MaxLength(35)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(35)]
        public string FirstName { get; set; }
        [MaxLength(3)]
        public string MiddleInitial { get; set; }
        [MaxLength(9)]
        public string SSN { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        [MaxLength(60)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string Email { get; set; }
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
        [MaxLength(50)]
        public string SubscriberId { get; set; }
        public string GroupName { get; set; }
        public string GroupNumber { get; set; }
        public DateTime? PlanBeginDate { get; set; }

        public DateTime? PlanEndDate { get; set; }
        public decimal? Copay { get; set; }
        public decimal? Deductible { get; set; }
        public decimal? CoInsurance { get; set; }
        //Need to make EmployerID as Foreign Key later when Employer table is being created
        public long? EmlpoyerID { get; set; }
        [ForeignKey("ID")]
        public long? InsurancePlanAddressID { get; set; }
        public virtual InsurancePlanAddress InsurancePlanAddress { get; set; }
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsSelfPay { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }

        [ForeignKey("ID")]
        public long? BatchDocumentID { get; set; }
        public virtual BatchDocument BatchDocument { get; set; }

        public string PageNumber { get; set; }

        public string FrontSideCard { get; set; }
        public string backSidecard { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public string CommonKey { get; set; }
        public bool? AuthRequired { get; set; }

    }

}

