using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMVisit
    {
        public List<Visit> Visit { get; set; }
        public List<DropDown> Practice { get; set; }
        public List<DropDown> Location { get; set; }
        public List<DropDown> Provider { get; set; }
        public List<DropDown> RefProvider { get; set; }
        //public List<DropDown> POS { get; set; }
        //public List<Data> ICD { get; set; }
        //public List<Data> Modifier { get; set; }
        //public List<Data> CPT { get; set; }
        public List<PatientInfoDropDown> PatientInfo { get; set; }


        public void GetProfiles(ClientDbContext pMContext, long PracticeID)
        {
            //Practice = (from f in pMContext.Practice
            //             select new DropDown()
            //             {
            //                 ID = f.ID,
            //                 Description = f.Name + " - " + f.OrganizationName
            //             }).ToList();
            //Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //Location = (from l in pMContext.Location
            //            select new DropDown()
            //            {
            //                ID = l.ID,
            //                Description = l.Name + " - " + l.OrganizationName
            //            }).ToList();
            //Location.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //Provider  = (from p in pMContext.Provider
            //           select new DropDown()
            //           {
            //               ID = p.ID,
            //               Description = p.Name
            //           }).ToList();
            //Provider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //RefProvider = (from p in pMContext.RefProvider
            //            select new DropDown()
            //            {
            //                ID = p.ID,
            //                Description = p.Name
            //            }).ToList();
            //RefProvider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //POS = (from p in pMContext.POS
            //       select new DropDown()
            //       {
            //           ID = p.ID,
            //           Description = p.PosCode + " - " + p.Name,
            //           Description2 = p.PosCode,
            //       }).ToList();
            //POS.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //ICD = (from i in pMContext.ICD
            //       select new Data()
            //       {
            //           ID = i.ID,
            //           Value = i.Description,
            //           label = i.ICDCode,
            //           Description = i.Description,

            //       }).ToList();
            ////ICD.Insert(0, new Data() { ID = null, Value = "Please Select" });

            //CPT = (from i in pMContext.Cpt
            //       select new Data()
            //       {
            //           ID = i.ID,
            //           Value = i.Description,
            //           label = i.CPTCode,
            //           Description = i.Description,
            //           Description1 = i.DefaultUnits,
            //           Description2 = i.Amount,
            //           AnesthesiaUnits = i.AnesthesiaBaseUnits,
            //           Category = i.Category
            //       }).ToList();

            //Modifier = (from i in pMContext.Modifier
            //            select new Data()
            //            {
            //                ID = i.ID,
            //                Value = i.Description,
            //                label = i.Code,
            //                Description = i.Description,
            //                AnesthesiaUnits = i.AnesthesiaBaseUnits.Value,
            //                Description2 = i.DefaultFees
            //            }).ToList();


            PatientInfo = (from p in pMContext.Patient
                           join l in pMContext.Location
                           on p.LocationId equals l.ID
                           join prac in pMContext.Practice
                           on p.PracticeID equals prac.ID
                           //join up in pMContext.UserPractices
                           //on prac.ID equals up.PracticeID
                           //join u in pMContext.Users
                           //on up.UserID equals u.Id
                           where prac.ID == PracticeID  //&& u.Id.ToString() == UserID
                           select new PatientInfoDropDown()
                           {
                               PatientID = p.ID.ToString(),
                               PatientName = p.LastName + ", " + p.FirstName,
                               AccountNumber = p.AccountNum,
                               DOB = p.DOB.Format("yyyy-dd-MM"),
                               Gender = p.Gender,
                               PracticeID = p.PracticeID,
                               LocationID = p.LocationId,
                               POSID = l.POSID,
                               providerID = p.ProviderID,
                               RefProviderID = p.RefProviderID,
                               label = p.LastName + " " + p.FirstName + " " + p.AccountNum,
                               Value = p.LastName + " " + p.FirstName + " " + p.AccountNum,

                           }).ToList();
            PatientInfo.Insert(0, new PatientInfoDropDown() { PatientID = null, PatientName = "Please Select" });
        }

        public class CVisit
        {
            // Note Values creating Duplication For Moment commented//////
            //public long ChargeID { get; set; }
            //public string InsuredID { get; set; }
            public string AccountNum { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public long? VisitID { get; set; }
            public long? ChargeID { get; set; }
            public long? BatchID { get; set; }   
            public DateTime? DosFrom { get; set; }
            public DateTime? DosTo  { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public string Practice { get; set; }
            public string Location { get; set; }
            public string Provider { get; set; }
            public string Plan { get; set; }    
            public string RefProvider { get; set; }
            public string InsuranceType { get; set; }
            public string SubmissionType { get; set; }
            public string IsSubmitted { get; set; }
            public long? PayerID { get; set; }
            public string IsPaid { get; set; }
            public string Status { get; set; }
            public string CPTCode { get; set; }
            public string SubscriberID { get; set; }
            public string AgeType { get; set; }
            public DateTime? SubmittedFromDate { get; set; }
            public DateTime? SubmittedToDate { get; set; }
            public string VisitType { get; set; }
            public int pageNo { get; set; }
            public int PerPage { get; set; }
        }
        public class GVisit
        {
            public long? VisitID { get; set; }
            public long? ChargeID { get; set; }
            public string DOS { get; set; }
            public string EntryDate { get; set; }
            public string AccountNum { get; set; }
            public long? patientID { get; set; }
            public string Patient { get; set; }
            public long? PracticeID { get; set; }
            public string Practice { get; set; }
            public long? LocationID { get; set; }
            public string Location { get; set; }
            public long? ProviderID { get; set; }
            public string Provider { get; set; }
            public string InsurancePlanName { get; set; }
            public long? InsurancePlanID { get; set; }
            public string SubscriberID { get; set; }
            public string PrimaryPatientPlanID { get; set; }

            public string SubmittedDate { get; set; }
            [NotMapped]
            public string ClaimAge { get; set; }
            public string PrimaryStatus { get; set; }
            public decimal? BilledAmount { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? AdjustmentAmount { get; set; }
            public decimal? PrimaryPlanBalance { get; set; }
            public decimal? PrimaryPatientBalance { get; set; }
            public string Rejection { get; set; }
            public decimal? Amount { get; set; }

            // Secondary Fields
            public string SecondaryStatus { get; set; }
            public decimal? SecondaryPlanBalance { get; set; }
            public decimal? SecondaryPatientBalance { get; set; }
            public string Status { get; set; }

            public long? Edi837PayerID { get; set; }
            public string VisitType { get; set; }
            public string RejectionReason { get; set; }
        }

        public class GVMVisit
        {
            public long? ChargeID { get; set; }
            public long? patientID { get; set; }
            public long? CPTID { get; set; }
            public long? PrimaryPatientPlanID { get; set; }
            public string AuthorizationNum { get; set; }
            public long? ProviderID { get; set; }

        }

    }
}
