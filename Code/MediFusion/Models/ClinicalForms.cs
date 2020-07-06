using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class ClinicalForms
    {
        [Required]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public virtual ICollection<ClinicalFormsCPT> CPTs { get; set; }
        public long? ProviderID { get; set; }
        public long? PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string url { get; set; }
        [NotMapped]
        public string formContent { get; set; }
        [NotMapped]
        public string ProviderName { get; set; }

    }
}
