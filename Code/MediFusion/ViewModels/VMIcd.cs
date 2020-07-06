using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMIcd
    {
        public List<DropDown> visitReason { get; set; }
        public List<DropDown> icdType { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        { 
            visitReason = (from m in pMContext.VisitReason

                           select new DropDown()
                           {
                               ID = m.ID,
                               Description = m.Name
                           }).ToList();
            visitReason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            icdType = (from m in pMContext.GeneralItems
                       where m.Type.Equals("icd_type")
                       select new DropDown()
                       {
                           ID = m.ID,
                           Description = m.Name
                       }).ToList();
            icdType.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

        }
        public class CIcd
        {
            public string ICDCode { get; set; }
            public string Description { get; set; }

        }

        public class GIcd
        {
            public long Id { get; set; }
            public string ICDCode { get; set; }
            public string Description { get; set; }
            
            public bool? IsValid { get; set; }



        }
    }
}
