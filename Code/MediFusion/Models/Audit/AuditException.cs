using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class AuditException
    {
        [Required]
        public long ID { get; set; }
        public long TransactionID { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ExceptionMessage { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
