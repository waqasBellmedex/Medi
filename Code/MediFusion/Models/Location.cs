using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
   
        public class Location
        {
            [Required]
            public long ID { get; set; }
            [Required]
            [MaxLength(60)]
            public string Name { get; set; }
            [Required]
            [MaxLength(60)]
            public string OrganizationName { get; set; }
            
         
            [ForeignKey("ID")]
            public long? PracticeID { get; set; }
            public virtual Practice Practice { get; set; }
            [StringLength(10)]
            public string NPI { get; set; }
            
            [Required]
            [ForeignKey("ID")]
            public long POSID { get; set; }
            public virtual POS POS { get; set; }

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
            [MaxLength(10)]
            public string CLIANumber { get; set; }
            [StringLength(10)]
            public string Fax { get; set; }
            public String website { get; set; }
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
            public String Email { get; set; }
            [StringLength(10)]
            public String PhoneNumber { get; set; }
            [MaxLength(4)]
            public string PhoneNumExt { get; set; }
            [MaxLength(500)]
            public string Notes { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public string AddedBy { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime AddedDate { get; set; }

            public string UpdatedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? UpdatedDate { get; set; }

        }
    }

