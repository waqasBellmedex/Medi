using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.ViewModel;

namespace MediFusionPM.Models
{
    public class Action
    {
        [Required]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("AuthIdentityCustom")]
        public string UserID { get; set; }
        public  AuthIdentityCustom AuthIdentityCustom { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
