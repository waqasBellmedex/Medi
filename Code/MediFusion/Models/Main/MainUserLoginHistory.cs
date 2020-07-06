using MediFusionPM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models.Main
{
    public class MainUserLoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MainAuthIdentityCustom")]
        public string UserId { get; set; }
        public string TokenId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastActivityTime { get; set; }
        public bool Status { get; set; }
    }
}
