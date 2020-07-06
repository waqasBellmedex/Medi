using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMTeam
    {

        public class CTeam
        {
            public long TeamId { get; set; }
            public string Name { get; set; }

        }


        public class GTeam
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string Details { get; set; }
            public string Email { get; set; }
            public string ReportTo { get; set; }
            public string Role { get; set; }

        }
    }
}
