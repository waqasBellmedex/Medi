using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMPatientFollowup
    {
        public List<DropDown> Reason { get; set; }
        public List<DropDown> Group { get; set; }
        public List<DropDown> Action { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {
            Reason = (from r in pMContext.Reason
                      select new DropDown()
                      {
                          ID = r.ID,
                          Description = r.Name,                      // + " - " + r.Description, // Remove Description  
                      }).ToList();
            Reason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Group = (from g in pMContext.Group
                     select new DropDown()
                     {
                         ID = g.ID,
                         Description = g.Name,                     // + " - " + g.Description,// Remove Description  
                     }).ToList();
            Group.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Action = (from a in pMContext.Action
                      select new DropDown()
                      {
                          ID = a.ID,
                          Description = a.Name,                    // + " - " + a.Description,// Remove Description  
                      }).ToList();
            Action.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }

        public class CPatientFollowup
        {
            public long? PatientID { get; set; }
            public DateTime? FollowUpDate { get; set; }
            public string PatientAccount { get; set; }
            public long? ReasonID { get; set; }
            public long? ActionID { get; set; }
            public long? GroupID { get; set; }
            public bool? Resolved { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public DateTime? TickleDate { get; set; }
            public string Status { get; set; }

        }
        public class GPatientFollowup
        {
            // We are hiding first column in Grid.
            // So Adding ID column extra, so that VisitID could not be hidden.
            public long ID { get; set; }
            public long? PatientID { get; set; }
            public string PatientName { get; set; }
            public string PatientAccount { get; set; }
            public long? PatientFollowUpID { get; set; }
            public string FollowUpDate  { get; set; }
            public string TickleDate { get; set; }
            public decimal? PatientAmount { get; set; }
            public string Reason { get; set; }
            public string Action { get; set; }
            public string Group { get; set; }

            public string Status { get; set; }

        }

        public class GPatientFollowupCharge
        {

            public long ID { get; set; }
            public long? PatientFollowUpID { get; set; }
            public long? PatientID { get; set; }
            public long? VisitID { get; set; }
            public long? ChargeID { get; set; }
            public long InsurancePlanID { get; set; }
            public string Plan { get; set; }
            public string DOS { get; set; }
            public string CPT { get; set; }
            public decimal? BilledAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? Copay { get; set; }
            public decimal? Deductible { get; set; }
            public decimal? CoInsurance { get; set; }
            public decimal? PatientPaid { get; set; }
            public decimal? PatientBal { get; set; }
            public decimal? PatientAmount { get; set; }
            public string AddedBy { get; set; }
            public string AddedDate { get; set; }
            public string TickleDate { get; set; }
            public string Status { get; set; }

        }
        public class GPatientStatement
        {
            public long[] claimId { get; set; }
            public long[] patientId { get; set; }
            public string reportType { get; set; }
            public string statementMessage { get; set; }
            public string advPaymentProcMode { get; set; }

            public bool? viewOnly { get; set; } = false;


        }
        public class DPdfAll
        {
            public string[] pdf_address { get; set; }
           

        }
    }
}
