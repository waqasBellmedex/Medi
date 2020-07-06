using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;


namespace MediFusionPM.ViewModels
{
    public class VMReceiver
    {

        public class CReceiver
        {
            public string Name { get; set; }
            public string Address { get; set; }

        }

        public class GReceiver
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string Address { get; set; }
            public long ReceiverID { get; set; }
        }


    }
}
