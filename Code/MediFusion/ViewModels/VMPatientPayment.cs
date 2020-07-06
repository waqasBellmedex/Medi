using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMPatientPayment
    {
        public class CPatientPayment
        {
            public DateTime? PaymentDate { get; set; }
            public string CheckNumber { get; set; }
            public string Status { get; set; }
            public long ?PatientID { get; set; }
        }

        public class GPatientPayment
        {
            public long ID { get; set; }
            public string paymentDate { get; set; }
            public string PaymentMethod { get; set; }
            public decimal? PaymentAmount { get; set; }
            public decimal? AllocatedAmount { get; set; }
            public decimal? RemainingAmount { get; set; }
            public string Status { get; set; }
        }

        public class PatientPaymentGrid
        {
            public long ID { get; set; }
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
            public long? VisitID{ get; set; }
            public long? ChargeID { get; set; }
            public decimal? AllocatedAmount { get; set; }
            public string Status { get; set; }

        }




        }
}
