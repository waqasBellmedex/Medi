using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.TestModel
{
    public class InputObject
    {
        public IFormFile InputFile { get; set; }
        public string PracticeID { get; set; }
        public string LocationID { get; set; }
        public string ProviderID { get; set; }
    }

    public class InputObject1
    {
        public IFormFile InputFile { get; set; }
    }
}
