using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMProfiles
    {
        public List<DropDown> Practice { get; set; }
        public void GetProfiles(ClientDbContext pMContext)
        {
            Practice = (from r in pMContext.Practice
                        select new DropDown()
                        {
                            ID = r.ID,
                            Description = r.Name,
                        }).ToList();

            Practice.Insert(0, new DropDown() { ID = 0, Description = "Please Select" });
        }
    }
}
