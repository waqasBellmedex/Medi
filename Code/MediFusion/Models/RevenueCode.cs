﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MediFusionPM.Models
{
        public class RevenueCode
        {
            [Required]
            public long ID { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public string OutPatientPaymentDisposition { get; set; }
            public string OutPatientUnitRestrictions { get; set; }
            public string OtherOutPatientBillingLimitations { get; set; }
            public string AddedBy { get; set; }
            public DateTime? AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
        }
    
}