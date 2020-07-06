using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModel
{
    public class VMPatientStatementFile
    {
            
            public long ID { get; set; }

            public string FileName { get; set; }
            public string patient { get; set; }
            public string patientId { get; set; }
            public string claimIds { get; set; }
         
            public string pdf_url { get; set; }
            public string pdf_id { get; set; }

             public string xml_url { get; set; }
       
    }
}