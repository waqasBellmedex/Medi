using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMProviderSchedule
    {
        public List<DropDown> Practice { get; set; }
        public List<DropDown> VisitReason { get; set; }
        // public List<DropDown> Location { get; set; }
        //public List<DropDown> Provider { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {

            Practice = (from f in pMContext.Practice
                        select new DropDown()
                        {
                            ID = f.ID,
                            Description = f.Name + " - " + f.OrganizationName
                        }).ToList();
            Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            //Location = (from l in pMContext.Location
            //            select new DropDown()
            //            {
            //                ID = l.ID,
            //                Description = l.Name + " - " + l.OrganizationName
            //            }).ToList();
            //Location.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            //Provider = (from p in pMContext.Provider
            //            select new DropDown()
            //            {
            //                ID = p.ID,
            //                Description = p.Name
            //            }).ToList();
            //Provider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            VisitReason = (from vR in pMContext.VisitReason
                           select new DropDown()
                           {
                               ID = vR.ID,
                               Description = vR.Name,
                               Description2 = vR.Description
                           }).ToList();
            VisitReason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


        }

        public class CProviderSchedule
        {
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime ?ToDate { get; set; }

        }

        public class GProviderSchedule
        {
            public long? ID { get; set; }
            public long? AppointmentID { get; set; }
            public long? ProviderID { get; set; }
            public string Provider { get; set; }
            public long?LocationID { get; set; }
            public string Location { get; set; }
            public string TimeInterval { get; set; }
            public string FromTime { get; set; }
            public string ToTime { get; set; }
            public string AppointmentDate { get; set; }
            public string AppointmentStatus { get; set; }


        }
      

    }
}
