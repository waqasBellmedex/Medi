using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class GeneralItems
    {
     
        public long ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public string TypeFilter { get; set; }
        
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool Inactive { get; set; }
        public int? position { get; set; }

        
    }
}
