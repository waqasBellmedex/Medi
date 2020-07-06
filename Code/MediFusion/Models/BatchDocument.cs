using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.ViewModels;

namespace MediFusionPM.Models
{
    public class BatchDocument
    {

        public long ID { get; set; }
        public string Description { get; set; }
        [ForeignKey("ID")]
        public long? PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        [ForeignKey("ID")]
        public long? LocationID { get; set; }
        public virtual Location Location { get; set; }
        [ForeignKey("ID")]
        public long? ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [NotMapped]
        public FileUploadViewModel DocumentInfo { get; set; }
        public string DocumentFilePath { get; set; }
        public string FileName { get; set; }
        public long NumberOfPages { get; set;}
        public string FileSize { get; set; }
        public string FileType { get; set; }
        [ForeignKey("ID")]
        public long? DocumentTypeID { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //Wasim Enter Start
        public string ResponsibleParty { get; set; }
        public long? NoOfDemographics { get; set; }
        public long? NoOfDemographicsEntered { get; set; }
        [ForeignKey("BatchDocumentNoID")]
        public ICollection<BatchDocumentCharges> BatchDocumentCharges { get; set; }

        [ForeignKey("BatchDocumentNoID")]
        public ICollection<BatchDocumentPayment> BatchDocumentPayment { get; set; }
        [ForeignKey("BatchDocumentNoID")]
        public ICollection<Notes> Note { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
