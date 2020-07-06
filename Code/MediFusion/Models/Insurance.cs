using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MediFusionPM.Models
{
   
        public class Insurance
        {
            [Required]
            public long ID { get; set;}
            [Required]
            [MaxLength (30)]
            public string Name { get; set; }
            [MaxLength(50)]
            public string OrganizationName { get; set; }
            [MaxLength(55)]
            public string Address1 { get; set; }
           [MaxLength(55)]
            public string Address2{ get; set; }
            [MaxLength(20)]
            public string City { get; set; }
            [MaxLength (2)]
            public string State { get; set; }
            [MaxLength(9)]
            public string ZipCode { get; set; }
            [StringLength(10)]
            public String OfficePhoneNum { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string Email { get; set; }
            //[Url]
            public string Website { get; set; }
            [StringLength(10)]
            public string FaxNumber { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            [MaxLength(500)]
            public string Notes { get; set; }
            public string AddedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? UpdatedDate { get; set; }

        }

    }

