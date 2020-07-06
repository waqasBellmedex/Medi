using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientDocument
    {
        public long id { get; set; }
        public string name { get; set; }
        [ForeignKey("ID")]
        public long? DocumentCategoryID { get; set; }
       public virtual DocumentCategory DocumentCategory { get; set; }

        [ForeignKey("ID")]
        public long? PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        public string size { get; set; }
        public DateTime? UploadedDate { get; set; }

        public string uploadeBy { get; set; }
        public DateTime? documentDate { get; set; }
        public string url { get; set; }
        public string notes { get; set; }

        public bool? inActive { get; set; }
        public long practiceID { get; set; }

        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public string uploadeddocument { get; set; }
        [NotMapped]
        public string uploadeddocumenttype { get; set; }




    }
}
