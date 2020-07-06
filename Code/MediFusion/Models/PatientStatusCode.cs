﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;
namespace MediFusionPM.Models

{
    public class PatientStatusCode
    {
        [Required]
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
