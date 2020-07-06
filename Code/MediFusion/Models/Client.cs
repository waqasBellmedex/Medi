using MediFusionPM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(60)]
        public string OrganizationName { get; set; }
        public string TaxID { get; set; }
        public string ServiceLocation { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string OfficeHour { get; set; }
        public string FaxNo { get; set; }
        public string OfficePhoneNo { get; set; }
        [MaxLength(60)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string OfficeEmail { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string ContextName { get; set; }


        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }

        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LastNewTrigerModifiedDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LastNewInsertsModifiedDate { get; set; }

        public virtual ICollection<Practice> Practice { get; set; }
        public bool IsClientCreatedSuccessfully { get; set; }

        //  public virtual List <FileUploadViewModel> Files { get; set; }
        public string DeactivationReason { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public string DeactivateionAdditionalInfo { get; set; }
        public bool? IsDeactivated { get; set; }
    }
}
