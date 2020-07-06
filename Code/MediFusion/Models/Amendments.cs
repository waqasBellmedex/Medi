using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class Amendments
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Attachment { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Notes { get; set; }
        public DateTime? AmendmentDate { get; set; }
        public string Status { get; set; } 
        [ForeignKey("ID")]
        public long? PatientNotesId { get; set; }
        public virtual PatientNotes PatientNotes { get; set; } 
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public string AttachmentData { get; set; }



    }
}
