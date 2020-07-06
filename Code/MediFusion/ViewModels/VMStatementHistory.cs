﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMStatementHistory
    {
        public class CStatementHistory
        {
         
            public string Account { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public DateTime? StatementFromDate { get; set; }
            public DateTime? StatementToDate { get; set; }
            public long? VisitID { get; set; }
            
        }

        public class GStatementHistory
        {
            public long ID { get; set; }
            public string Account { get; set; }
            public string PatientName { get; set; }
            public long PatientID { get; set; }
            public long? VisitID { get; set; }
            public string StatementDate { get; set; }
            public int StatementCount { get; set; }

            
        }
    }
}
