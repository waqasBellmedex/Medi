using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMRemarkCode
    {
       


        public class CRemarkCode
        {
            public string RemarkCode { get; set; }
            public string Description { get; set; }
           

        }

        public class GRemarkCode
        {

            public long ID { get; set; }
            public string RemarkCode { get; set; }
            public string Description { get; set; }


        }

    }
}
