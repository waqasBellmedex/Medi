using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModel
{
    public class VMRights
    {
        public Rights Rights { get; set; }
        public string Email { get; set; }

    }
}
