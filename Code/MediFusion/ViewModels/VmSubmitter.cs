using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMSubmitter
    {
        public List<DropDown> Receiver { get; set; }
        public void GetProfiles(ClientDbContext pMContext)
        {

            Receiver = (from r in pMContext.Receiver
                        select new DropDown()
                        {
                            ID = r.ID,
                            Description = r.Name
                        }).ToList();
            Receiver.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }
        public class CSubmitter
        {
            public string Name { get; set; }
            public String Address { get; set; }

        }

        public class GSubmitter
        {

            public long ID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public long ReceiverID { get; set; }
            public string ReceiverName { get; set; }

        }
    }
}
