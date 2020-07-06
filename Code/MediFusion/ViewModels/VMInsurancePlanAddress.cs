using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMInsurancePlanAddress
    {

        public List<DropDown> InsurancePlan { get; set; }


        public void GetProfiles(ClientDbContext pMContext)
        {
            InsurancePlan = (from ip in pMContext.InsurancePlan

                        select new DropDown()
                        {
                            ID = ip.ID,
                            Description = ip.PlanName + " - " + ip.Description,
                        }).ToList();

            InsurancePlan.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



        }



public class CInsurancePlanAddress
        {

      public string InsurancePlan { get; set; }
      public string Address { get; set; }
      public string PhoneNumber { get; set; }


        }


        public class GInsurancePlanAddress
        {
            public long Id { get; set; }
            public long  InsurancePlanID { get; set; }
            public string InsurancePlan { get; set; }
            public string Address { get; set; }    
            public string PhoneNumber { get; set; }
            public string FaxNumber { get; set; }





        }


    }
}
