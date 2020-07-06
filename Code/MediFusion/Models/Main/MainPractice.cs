using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.ViewModel;
using MediFusionPM.ViewModels.Main;

namespace MediFusionPM.Models.Main
{
    public class MainPractice
    {
        [Required]
        public long ID { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        //[Required]
        [ForeignKey("MainClient")]
        public long? ClientID { get; set; }


        [Required]
        [MaxLength(200)]
        public string OrganizationName { get; set; }
        [StringLength(9)]
        public string TaxID { get; set; }
        [MaxLength(20)]
        public string CLIANumber { get; set; }
        [StringLength(10)]
        public string NPI { get; set; }
        // [Required]
        [StringLength(9)]
        public string SSN { get; set; }
        public string Type { get; set; }
        [MaxLength(10)]
        public string TaxonomyCode { get; set; }
        [MaxLength(55)]
        public string Address1 { get; set; }
        [MaxLength(55)]
        public string Address2 { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(9)]
        public string ZipCode { get; set; }
        [StringLength(10)]
        public string OfficePhoneNum { get; set; }
        [StringLength(10)]
        public string FaxNumber { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string Email { get; set; }
        public string Website { get; set; }

        [MaxLength(55)]
        public string PayToAddress1 { get; set; }
        [MaxLength(55)]
        public string PayToAddress2 { get; set; }
        [MaxLength(20)]
        public string PayToCity { get; set; }
        [MaxLength(2)]
        public string PayToState { get; set; }
        [MaxLength(9)]
        public string PayToZipCode { get; set; }

        public long? DefaultLocationID { get; set; }
        public string WorkingHours { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string AddedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }

        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        public ICollection<MainAuthIdentityCustom> MainAuthIdentityCustom { get; set; }
        [MaxLength(35)]
        public string ProvLastName { get; set; }
        [MaxLength(35)]
        public string ProvFirstName { get; set; }
        [MaxLength(3)]
        public string ProvMiddleInitial { get; set; }
        public string StatementExportType { get; set; }
        public string StatementMessage { get; set; }
        public long? StatementAgingDays { get; set; }
        public long? StatementMaxCount { get; set; }

        //Add New Fields Dated03202020
        [StringLength(10)]
        public string CellNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string InvoicePercentage { get; set; }
        public decimal? MinimumMonthlyAmount { get; set; }
        public int? NumberOfFullTimeEmployees { get; set; }
        public decimal? FTEPerDayRate { get; set; }
        public decimal? FTEPerWeekRate { get; set; }
        public decimal? FTEPerMonthRate { get; set; }
        public bool? IncludePatientCollection { get; set; }
        public string ClientCategory { get; set; }
        public string RefferedBy { get; set; }
        public string PMSoftwareName { get; set; }
        public string EHRSoftwareName { get; set; }

        //Add New Fields-04-02-2020 // Aziz-Sab
        [StringLength(10)]
        public string StatementPhoneNumber { get; set; }
        [StringLength(4)]
        public string StatementPhoneNumberExt { get; set; }
        [StringLength(10)]
        public string AppointmentPhoneNumber { get; set; }
        [StringLength(4)]
        public string AppointmentPhoneNumberExt { get; set; }
        [StringLength(10)]
        public string StatementFaxNumber { get; set; }
        public bool IsAutoFollowup { get; set; }
        public bool IsAutoDownloading { get; set; }
        public bool IsAutoSubmission { get; set; }
        

        //[ForeignKey("MainPracticeID")]
        //public ICollection<MainPracticeResponsibilities> MainPracticeResponsibilities { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string PLDDirectory { get; set; }

        public bool? IsEmailAppointmentReminder { get; set; }
        public bool? IsSMSAppointmentReminder { get; set; }
        public bool? isGoogleCalenderEnable { get; set; }
        public string googleCalenderSecret { get; set; }


        public string googleSheetID { get; set; }
        public string googleSheetSecret { get; set; }

        public string CalenderID { get; set; }

        public int? GoogleSheetRows { get; set; }
        public bool? isGoogleSheetEnable { get; set; }


    }
}
