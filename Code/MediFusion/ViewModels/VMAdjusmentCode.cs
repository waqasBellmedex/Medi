using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMAdjustmentCode
    {
        public List<DropDown> Reason { get; set; }
        public List<DropDown> Group { get; set; }
        public List<DropDown> Action { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {
            Reason = (from r in pMContext.Reason
                      select new DropDown()
                      {
                          ID = r.ID,
                          Description = r.Name,
                      }).ToList();
            Reason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Group = (from g in pMContext.Group
                     select new DropDown()
                     {
                         ID = g.ID,
                         Description = g.Name,
                     }).ToList();
            Group.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Action = (from a in pMContext.Action
                      select new DropDown()
                      {
                          ID = a.ID,
                          Description = a.Name,
                      }).ToList();
            Action.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }
        public class CAdjustmentCode
        {
            public string AdjustmentCode { get; set; }
            public string Description { get; set; }
        }

        public class GAdjustmentCode
        {
            public long ID { get; set; }
            public string AdjustmentCode { get; set; }
            public string Description { get; set; }
            public string ISValid { get; set; }
            public string Type { get; set; }

        }

    }
}
