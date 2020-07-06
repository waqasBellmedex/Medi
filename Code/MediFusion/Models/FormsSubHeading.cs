using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.ViewModel;
namespace MediFusionPM.Models
{
    public class FormsSubHeading
    {
        [Required]
        public long ID { get; set; }

        [ForeignKey("ID")]
        public long? clinicalFormsID { get; set; }
        public virtual ClinicalForms ClinicalForms { get; set; }

        public string subheading { get; set; }

        public string type { get; set; }
        public string appFunction { get; set; }
        public string customID { get; set; } 
        public string defaultValue { get; set; }

        [ForeignKey("ID")]
        public long? practiceID { get; set; }
        public virtual Practice Practice { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
