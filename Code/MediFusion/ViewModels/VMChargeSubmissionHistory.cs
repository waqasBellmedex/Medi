using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;


namespace MediFusionPM.ViewModels
{
    public class VMChargeSubmissionHistory
    {


        public class FindChargeSubmissionHistory
        {
           
            public long ID { get; set; }
            public long? ChargeID { get; set; }
            public string Receiver { get; set; }
            public string SubmitType { get; set; }
            public string FormType { get; set; }
            public long? PatientPlanID { get; set; }
            public string Plan { get; set; }
            public decimal? Amount { get; set; }
            public string AddedBy { get; set; }
            public string AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public string UpdatedDate { get; set; }




        }

        public class GChargeSubmissionHistory
        {
            public long ID { get; set; }
            public long?  ChargeID { get; set; }
            public string AddedDate { get; set; }   
            public string  SubmitType { get; set; }
            public string Receiver { get; set; }
            public string User { get; set; }  // User is a value from AddedBy Column 
            public string FormType { get; set; }
            public string Coverage { get; set; }
            public string Plan { get; set; }
        }





















    }
}
