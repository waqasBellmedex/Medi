using System;
using System.ComponentModel.DataAnnotations;

namespace MediFusionPM.Models
{
   
        public class TypeOfService
    {
            public long ID { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }

            public string AddedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? UpdatedDate { get; set; }

        }
}

