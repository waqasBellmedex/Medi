using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMDocumentType
    {

        public class CDocumentType
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class GDocumentType
        {

            public long ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string AddedBy { get; set; }
            public string AddedDate { get; set; }

        }
    }
}
