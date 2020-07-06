using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
   
        public class Settings
        {
            [Required]
            public long ID { get; set; }

            public long ClientID { get; set; }

            public string DocumentServerURL { get; set; }
            public string DocumentServerDirectory { get; set; }

            public string DocumentServerAuthUser { get; set; }
            public string DocumentServerAuthPass { get; set; }



            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
        }


    }

