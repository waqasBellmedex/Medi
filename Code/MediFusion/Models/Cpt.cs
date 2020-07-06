using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
   
        public class Cpt
        {
            [Required]
            public long ID { get; set; }
           // [Required]
           // [MaxLength(100)]
            public string Description { get; set; }
           [MaxLength(500)]
           public string ShortDescription { get; set; }
           public string CPTCode { get; set; }
            public decimal Amount { get; set; }
           [ForeignKey("ID")]
            public long? Modifier1ID { get; set; }
            public virtual Modifier Modifier1 { get; set; }

            [ForeignKey("ID")]
            public long? Modifier2ID { get; set; }
            public virtual Modifier Modifier2 { get; set; }

            [ForeignKey("ID")]
            public long? Modifier3ID { get; set; }
            public virtual Modifier Modifier3 { get; set; }

            [ForeignKey("ID")]
            public long? Modifier4ID { get; set; }
            public virtual Modifier Modifier4 { get; set; }

            [ForeignKey("ID")]
            public long? TypeOfServiceID { get; set; }
            public virtual TypeOfService TypeOfService { get; set; }

            public string Category { get; set; }

            public string DefaultUnits { get; set; }
            public string UnitOfMeasurement { get; set; }
            public string CLIANumber { get; set; }

            public string NDCNumber { get; set; }
            public string NDCDescription { get; set; }
            public string NDCUnits { get; set; }
            public string NDCUnitOfMeasurement { get; set; }
            public bool IsValid { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public int? AnesthesiaBaseUnits { get; set; }
            public decimal? NonFacilityAmount { get; set; }
            public decimal? MedicareAmount { get; set; }
            [ForeignKey("ID")]
            public long? POSID { get; set; }
            public virtual POS POS { get; set; }



    }


    }

