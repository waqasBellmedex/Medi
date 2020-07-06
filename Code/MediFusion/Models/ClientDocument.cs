using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class ClientDocument
    {
        [Required]
        public long ID { get; set; }
        public long? ClientID { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        public string Content { get; set; }
        public string DocumentPath { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
