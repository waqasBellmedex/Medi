using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;
using static MediFusionPM.ViewModels.VMPlanFollowUp;

namespace MediFusionPM.Models.TodoApi
{
    public class PlanFollowup
    {
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long ?VisitID { get; set; }
        public virtual Visit Visit { get; set; }
       
        [ForeignKey("ID")]
        public long? GroupID { get; set; }
        public virtual Group Group { get; set; }
        [ForeignKey("ID")]
        public long ?ReasonID { get; set; }
        public virtual Reason Reason { get; set; }
        [ForeignKey("ID")]
        public long? ActionID { get; set; }
        public virtual Action Action { get; set; }
        [ForeignKey("ID")]
        public long? AdjustmentCodeID { get; set; }
        public virtual AdjustmentCode AdjustmentCode { get; set; }
        [ForeignKey("ID")]
        public long ?VisitStatusID { get; set; }
        public virtual VisitStatus VisitStatus { get; set; }

        public string Notes { get; set; }
        [NotMapped]
        public string Age { get; set; }

        [ForeignKey("PlanFollowupID")]
        public ICollection<PlanFollowupCharge> PlanFollowupCharge { get; set; }
        [NotMapped]
        public ICollection<GFollowupCharge> GFollowupCharge { get; set;}

        [ForeignKey("PlanFollowupID")]
        public ICollection<Notes> Note { get; set; }

        [ForeignKey("ID")]
        public long ?PaymentVisitID { get; set; }
        public virtual PaymentVisit PaymentVisit { get; set; }

        public DateTime? TickleDate { get; set; }
        public bool Resolved { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ?AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ?UpdatedDate { get; set; }
        public string RemitCode { get; internal set; }
    }
}
