using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    
        public class ChargeSubmissionHistory
        {
            [Required]
            public long ID { get; set; }

            [ForeignKey("ID")]
            public long? ChargeID { get; set; }
            public virtual Charge Charge { get; set; }
            [ForeignKey("ID")]
            public long? ReceiverID { get; set; }
            public virtual Receiver Receiver { get; set; }
            [ForeignKey("ID")]
            public long? SubmissionLogID { get; set; }
            public virtual SubmissionLog SubmissionLog { get; set; }
            public string SubmitType { get; set; }
            public string FormType { get; set; }
            [ForeignKey("ID")]
            public long? PatientPlanID { get; set; }
            public virtual PatientPlan PatientPlan { get; set; }
            public decimal? Amount { get; set; }


            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
        }


    }

