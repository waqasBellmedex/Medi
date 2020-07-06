using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMPaymentCharge
    {


        public class CPaymentCharge
        { 
        
        public DateTime? EntryDateFrom { get; set; }
        public DateTime? EntryDateTo { get; set; }
        public DateTime? CheckDateFrom { get; set; }
        public DateTime? CheckDateTo { get; set; }

        public string CheckNumber { get; set; }
        public string Practice { get; set; }
        public string Payer { get; set; }
        
        }


        public class GPaymentCharge
        {
            
            public long? ReceiverID { get; set; }
            public string CheckNumber { get; set; }
            public string PaymentMethod { get; set; }
            public string CheckDate { get; set; }
            public decimal? CheckAmount { get; set; }
            public decimal? Appliedamount { get; set; }
            public decimal? PostedAmount { get; set; }
            public decimal NumberOfVisits { get; set; }
            public decimal NumberOfPatients { get; set; }
            public string Status { get; set; }
            public string payer { get; set; }
            public string payee { get; set; }
        }












    }
}
