using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class Rooms
    {

        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? PracticeID { get; set; }
        public long? ProviderID { get; set; }
        public bool Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
