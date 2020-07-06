using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientFormValue
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long? patientFormID { get; set; }


        public virtual PatientForms PatientForms { get; set; }

        [ForeignKey("ID")]
        public long? patientNotesID { get; set; }

        
        public virtual PatientNotes PatientNotes { get; set; }

        [ForeignKey("ID")]
        public long? clinicalFormsID { get; set; }

        public virtual ClinicalForms ClinicalForms { get; set; }

        [ForeignKey("ID")]
        public long? formsSubHeadingID { get; set; }

        public virtual FormsSubHeading FormsSubHeading { get; set; }
       
        public string value { get; set; }

        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
 