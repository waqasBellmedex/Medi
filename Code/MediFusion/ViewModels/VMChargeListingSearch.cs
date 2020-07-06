using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
 
    
        public class VMChargeListingSearch
        {


        public class CChargeListingSearch
        {
            public long  AccountNum { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public long SubscriberId { get; set; }
            public long PayerId { get; set; }
            public long VisitNum    { get; set; }
            public long ChargeNum { get; set; }
            public long BatchNum { get; set; }
            public string InsType { get; set; }
            public string SubType { get; set; }
            public DateTime DOSFrom { get; set; }
            public DateTime DOSTO { get; set; }
            public DateTime   EntryDateFrom { get; set; }
            public DateTime EntryDateTo { get; set; }
            public string Submitted { get; set; }

            public string Plan { get; set; }
            public string Practice { get; set; }
            public string Location { get; set; }
            public string Provider { get; set; }


            public int RefProvider { get; set; }

            public decimal  paid { get; set; }

        }


        public class GChargeListingSearch
        {
            public long VisitNum { get; set; }
            public DateTime DOS { get; set; }
            public DateTime EntryDate { get; set; }
            public long AccountNum { get; set; }
            public string  Patient { get; set; }
            public string Practice { get; set; }
            public string Location { get; set; }
            public decimal BilledAmount { get; set; }
            public decimal PlanAmount { get; set; }
            public decimal AllowedAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal PatResp { get; set; }
            public decimal PatPaid { get; set; }



        }

        }





    
}
