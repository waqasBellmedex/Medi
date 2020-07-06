using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMLocation
    {
        public  List<DropDown> Practice  { get; set; }
        public List<DropDown> POSCodes { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {

            Practice = (from f in pMContext.Practice

                        select new DropDown()
                        {
                            ID = f.ID,
                            Description = f.Name + " - " + f.OrganizationName
                        }).ToList();

            Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            
            // Drop Down Code for POS Dropdown..........

            POSCodes = (from p in pMContext.POS

                        select new DropDown()
                        {
                            ID = p.ID,
                            Description = p.PosCode + " - " + p.Name,
                        }).ToList();

            POSCodes.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

           


            ////ForTesting POS
            //POSCodes = new List<DropDown>()
            //    {
            //     new DropDown() { ID = 1, Description = "11", Description2= "Office"},
            //     new DropDown() { ID = 1, Description = "12", Description2= "Home"}
            //      };

        }

        public class CLocation
        {
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public  string Practice { get; set; }
            public string NPI { get; set; }
            public string PosCode { get; set; }

        }

        public class GLocation
        {
            public long? ID { get; set; }
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public long ?PracticeID { get; set; }
            public string Practice { get; set; }
            public string NPI { get; set; }
            public string PosCode { get; set; }
            public string Address { get; set; }
            

        }
    }
}
