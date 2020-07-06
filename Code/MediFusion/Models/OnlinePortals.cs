using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class OnlinePortals
    {
        public long ID { get; set; }
        public long InsurancePlanID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        
        [ForeignKey("OnlinePortalsID")]
        public ICollection<OnlinePortalCredentials> OnlinePortalCredentials { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Type { get; set; }
    }
}
