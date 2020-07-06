using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MediFusionPM.Models.Deleted
{
    public class ActionDeleted
    {
        [Required]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string EventPerformedUser { get; set; }
        public DateTime? EventTime { get; set; }
        public string HostName { get; set; }
    }
}
