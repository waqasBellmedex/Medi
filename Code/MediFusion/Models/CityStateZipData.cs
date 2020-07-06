using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class CityStateZipData
    {
        public int ID { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string State_id { get; set; }
        public string State_name { get; set; }
        public string Zip { get; set; }
        public int Population { get; set; }
        public double Density { get; set; }
        public string city { get; set; }
    }
}
