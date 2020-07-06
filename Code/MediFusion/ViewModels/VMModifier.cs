using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMModifier
    {


        public class CModifier
        {
           public string ModifierCode { get; set; }
           public string Description { get; set; }   

        }


        public class GModifier
        {
            public long? Id { get; set; }
            public string ModifierCode { get; set; }
            public string Description { get; set; }

        }

    }
}
