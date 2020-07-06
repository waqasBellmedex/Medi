using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MediFusionPM.Models.Main
{
    public class AutoPlanFollowUpLog
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long ClientID { get; set; }
        public virtual MainClient MainClient { get; set; }
        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        public virtual MainPractice MainPractice { get; set; }
        public long TotalRecords { get; set; }
        public long FollowUpCreated { get; set; }
        public string LogMessage { get; set; }
        public string Exception { get; set; }
        //  public DateTime ServiceStartTime { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
