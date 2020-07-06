using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;
using static MediFusionPM.ViewModels.VMPatientFollowup;

namespace MediFusionPM.Models.TodoApi
{
    public class PatientFollowUp
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("PatientFollowUpID")]
        public ICollection<PatientFollowUpCharge> PatientFollowUpCharge { get; set; }
        [ForeignKey("ID")]
        public long? PatientID { get; set; }
        public virtual Patient Patient { get; set; }
        [ForeignKey("ID")]
        public long? ReasonID { get; set; }
        public virtual Reason Reason { get; set; }
        [ForeignKey("ID")]
        public long? ActionID { get; set; }
        public virtual Action Action { get; set; }
        [ForeignKey("ID")]
        public long? GroupID { get; set; }
        public virtual Group Group { get; set; }
        [ForeignKey("PatientFollowUpID")]
        public ICollection<Notes> Note { get; set; }
        [NotMapped]
        public ICollection<GPatientFollowupCharge> GPatientFollowupCharge { get; set; }
        public long? PaymentVisitID { get; set; }
        public long? AdjustmentCodeID { get; set; }

        //public decimal? PatientAmount { get; set; }
        public string Notes { get; set; }
        [NotMapped]
        public string Age { get; set; }
       
        [Column(TypeName = "Date")]
        public DateTime? TickleDate { get; set; }
        //[Column(TypeName = "Date")]
        public DateTime? Statement1SentDate { get; set; }
        //[Column(TypeName = "Date")]
        public DateTime? Statement2SentDate { get; set; }
        public DateTime? Statement3SentDate { get; set; }
        public bool Resolved { get; set; }

        public string Status { get; set; }
        [DataType(DataType.DateTime)]
        public string AddedBy { get; set; }
        public DateTime ?AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ?UpdatedDate { get; set; }

    }
}
