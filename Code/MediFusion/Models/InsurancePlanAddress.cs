using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MediFusionPM.Models
{
   
        public class InsurancePlanAddress
        {

            [Required]
            public long  ID { get; set; }
            [ForeignKey("ID")]
            public long InsurancePlanId { get; set; }
            public virtual InsurancePlan InsurancePlan { get; set; }
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
            [MaxLength(10)]
            public string PhoneNumber { get; set; }
            [MaxLength(10)]
            public string FaxNumber { get; set; }
            public bool IsDeleted { get; set; }
            [MaxLength(500)]
            public string Notes { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }





        }
    }



