using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
namespace MediFusionPM.ViewModels

{
    public class VMPatientPaymentCharge
    {


        public class GPatientPaymentCharge
        {
            public string DOS { get; set; }
            public string SubmitDate { get; set; }
            public string Plan { get; set; }
            public string CPT { get; set; }
            public decimal? BilledAmount { get; set; }
            public decimal? WriteOff { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? Copay { get; set; }
            public decimal? CoInsurance { get; set; }
            public decimal? Deductible { get; set; }
            public decimal? PatientBalance { get; set; }
            public long? VisitID { get; set; }
            public long? ChargeID { get; set; }
            public decimal? AllocationAmount { get; set; }

        }
    }
}
