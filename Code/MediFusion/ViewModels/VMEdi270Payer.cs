using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
   

    public class VMEdi270Payer
    {
        public List<DropDown> ReceiverName { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {
            ReceiverName = (from r in pMContext.Receiver

                            select new DropDown()
                            {
                                ID = r.ID,
                                Description = r.Name,
                            }).ToList();

            ReceiverName.Insert(0, new DropDown() { ID = 0, Description = "Please Select" });

        }
            public class CEdi270Payer
        {
            public string PayerName { get; set; }
            public string PayerId { get; set; }
            public long ReceiverId { get; set; }



        }

        public class GEdi270Payer
        {
            public long Id { get; set; }
            public string PayerId { get; set; }
            public string PayerName { get; set; }

            public string ReceiverName { get; set; }

            

        }


    }
}
