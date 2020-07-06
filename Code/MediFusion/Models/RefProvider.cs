using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    
    public class RefProvider
    {
     [Required]
     public long ID { get; set; }
     [Required]
     [MaxLength (60)]
     public string Name { get; set; }
     [Required]
     [MaxLength (35)]
     public string LastName { get; set; }
    [Required]
    [MaxLength (35)]
     public string FirstName { get; set; }
     [MaxLength(3)]
     public string MiddleInitial { get; set; }
     public string Title { get; set; }
    [StringLength(10)]
     public string NPI { get; set; }
     [StringLength(9)]
     public string SSN { get; set; }
    [MaxLength(10)]
     public string TaxonomyCode { get; set; }

    [StringLength(9)]
     public string TaxID { get; set; }
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
     [MaxLength(10)]
     public string FaxNumber { get; set; }
     [ForeignKey("ID")]
     public long? PracticeID { get; set; }
     public virtual Practice Practice { get; set; }
      [MaxLength(60)]
      [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
      public string Email { get; set; }
      public bool IsActive { get; set; }
      public bool  IsDeleted { get; set; } 
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

