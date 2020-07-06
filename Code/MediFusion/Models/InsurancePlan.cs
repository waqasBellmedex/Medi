using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;
namespace MediFusionPM.Models
{

    public class InsurancePlan
    {
        [Required]
        public long ID { get; set; }
        [Required]
        [MaxLength(200)]
        public string PlanName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [ForeignKey("ID")]
        public long? InsuranceID { get; set; }
        public virtual Insurance Insurance { get; set; }
        [ForeignKey("ID")]
        public long? PlanTypeID { get; set; }
        public virtual PlanType PlanType { get; set; }
        public bool IsCapitated { get; set; }
        public string SubmissionType { get; set; }
        public string PayerID { get; set; }

        [ForeignKey("ID")]
        public long? Edi837PayerID { get; set; }
        public virtual Edi837Payer Edi837Payer { get; set; }


        [ForeignKey("ID")]
        public long? Edi270PayerID { get; set; }
        public virtual Edi270Payer Edi270Payer { get; set; }

        [ForeignKey("ID")]
        public long? Edi276PayerID { get; set; }
        public virtual Edi276Payer Edi276Payer { get; set; }

        public string FormType { get; set; }

        public int? OutstandingDays { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<InsuranceBillingoption> InsuranceBillingoption { get; set; }


    }



    }

