using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
namespace MediFusionPM.ViewModels
{
    public class VMPos
    {

        public class CPOS
        {

         public string POSCode { get; set; }
         public string Description { get; set; }
        public string Name { get; set; }


        }

        public class GPOS
        { 
            public long Id { get; set; }
        public string POSCode { get; set; }
        public string Description { get; set; }
            public string Name { get; set; }


        }
    }
}
