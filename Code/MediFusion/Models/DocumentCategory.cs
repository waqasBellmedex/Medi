using MediFusionPM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class DocumentCategory
    {
        public long id { get; set; }
        public string name { get; set; }
        [NotMapped]
        public string text { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public long? ParentcategoryID { get; set; }
        public virtual DocumentCategory ParentCategory { get; set; }
        
        public bool? inActive { get; set; }
        public long? practiceID { get; set; }

        public string AddedBy { get; set; }
        public string url { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public string parentName { get; set; }
        [NotMapped]
        public List<DocumentCategory> nodes { get; set; }
        [NotMapped]
        public string iconcontent { get; set; }




    }
}
