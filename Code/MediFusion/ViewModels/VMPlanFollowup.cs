using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMPlanFollowUp
    {
        public List<DropDown> Reason { get; set; }
        public List<DropDown> Group { get; set; }
        public List<DropDown> Action { get; set; }
        public List<DropDown> AdjusmentCode { get; set; }
        public List<DropDown> Location { get; set; }
        //public List<DropDown> Practice { get; set; }
        //public List<DropDown> Provider { get; set; }
        //public List<DropDown> RefProvider { get; set; }
        //public List<DropDown> SupProvider { get; set; }

        public PatientInfoDropDown PatientInfo { get; set; }
        public SubmissionInfoDropDown SubmissionInfo { get; set; }


        public void GetProfiles(ClientDbContext pMContext, long PlanFollowUpID)
        {
            Reason = (from r in pMContext.Reason
                      select new DropDown()
                      {
                          ID = r.ID,
                          Description = r.Name,
                      }).ToList();
            Reason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Group = (from g in pMContext.Group
                     select new DropDown()
                     {
                         ID = g.ID,
                         Description = g.Name,
                     }).ToList();
            Group.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Action = (from a in pMContext.Action
                      select new DropDown()
                      {
                          ID = a.ID,
                          Description = a.Name,
                      }).ToList();
            Action.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            AdjusmentCode = (from a in pMContext.AdjustmentCode
                             select new DropDown()
                             {
                                 ID = a.ID,
                                 Description = a.Code + " - " + a.Description,
                             }).ToList();
            AdjusmentCode.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //Practice = (from f in pMContext.Practice
            //            select new DropDown()
            //            {
            //                ID = f.ID,
            //                Description = f.Name + " - " + f.OrganizationName
            //            }).ToList();
            //Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //Location = (from l in pMContext.Location
            //            select new DropDown()
            //            {
            //                ID = l.ID,
            //                Description = l.Name + " - " + l.OrganizationName
            //            }).ToList();
            //Location.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //Provider = (from p in pMContext.Provider
            //            select new DropDown()
            //            {
            //                ID = p.ID,
            //                Description = p.Name
            //            }).ToList();
            //Provider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //RefProvider = (from p in pMContext.RefProvider
            //               select new DropDown()
            //               {
            //                   ID = p.ID,
            //                   Description = p.Name
            //               }).ToList();
            //RefProvider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //SupProvider = (from p in pMContext.Provider

            //               select new DropDown()
            //               {
            //                   ID = p.ID,
            //                   Description = p.Name
            //               }).ToList();
            //SupProvider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            if (PlanFollowUpID > 0)
            {

                PatientInfo = (from v in pMContext.Visit
                               join pf in pMContext.PlanFollowUp
                               on v.ID equals pf.VisitID
                               join p in pMContext.Patient
                               on v.PatientID equals p.ID
                               join pp in pMContext.PatientPlan on v.PrimaryPatientPlanID equals pp.ID
                               join ip in pMContext.InsurancePlan
                              on pp.InsurancePlanID equals ip.ID
                               join prac in pMContext.Practice
                               on v.PracticeID equals prac.ID
                               join loc in pMContext.Location
                               on v.LocationID equals loc.ID
                               join pos in pMContext.POS
                               on v.POSID equals pos.ID
                               join prov in pMContext.Provider
                               on v.ProviderID equals prov.ID
                               join refProv in pMContext.RefProvider
                               on v.RefProviderID equals refProv.ID into Table5
                               from t5 in Table5.DefaultIfEmpty()
                               join sProv in pMContext.Provider
                              on v.SupervisingProvID equals sProv.ID into Table6
                               from t6 in Table6.DefaultIfEmpty()
                               where pf.ID == PlanFollowUpID
                               select new PatientInfoDropDown()
                               {
                                   PatientID = v.PatientID.ToString(),
                                   PatientName = p.LastName + ", " + p.FirstName,
                                   AccountNumber = p.AccountNum,
                                   DOB = p.DOB.Format("MM/dd/yyyy"),
                                   Gender = p.Gender,
                                   PlanName = ip.PlanName,
                                   Coverage = pp.Coverage == "P" ? "Primary" : (pp.Coverage == "S" ? "Secondary" : (pp.Coverage == "T" ? "Teritary" : "")),
                                   //InsuredName = ip.Description,
                                   //InsuredID = pp.InsurancePlanID,
                                   SubscriberID = pp.SubscriberId,
                                   SubscriberName = pp.LastName + ", " + pp.FirstName,
                                   PracticeID = prac.ID,
                                   PracticeName = prac.Name,
                                   LocationID = loc.ID,
                                   LocationName = loc.Name,
                                   POSID = pos.ID,
                                   POSName = pos.Name,
                                   providerID = prov.ID,
                                   ProviderName = prov.Name,
                                   RefProviderID = t5.ID,
                                   RefProviderName = t5.Name,
                                   SupProviderID = t6.ID,
                                   SupProviderName = t6.Name,

                               }).SingleOrDefault();


                var visit = (from pf in pMContext.PlanFollowUp
                             join v in pMContext.Visit on pf.VisitID equals v.ID
                             where pf.ID == PlanFollowUpID
                             select v).SingleOrDefault();

                if (visit.PrimaryBal.Val() > 0)
                {
                    SubmissionInfo = (from pf in pMContext.PlanFollowUp
                                      join v in pMContext.Visit on pf.VisitID equals v.ID
                                      join pp in pMContext.PatientPlan on v.PrimaryPatientPlanID equals pp.ID
                                      join ip in pMContext.InsurancePlan
                                      on pp.InsurancePlanID equals ip.ID
                                      join edi837 in pMContext.Edi837Payer
                                      on ip.Edi837PayerID equals edi837.ID into Table1
                                      from t1 in Table1.DefaultIfEmpty()
                                      join rec in pMContext.Receiver
                                      on t1.ReceiverID equals rec.ID into Table2
                                      from t2 in Table2.DefaultIfEmpty()
                                      join ins in pMContext.Insurance
                                      on ip.InsuranceID equals ins.ID
                                      //join pv1 in pMContext.PaymentVisit
                                      //on v.ID equals pv1.VisitID into pv
                                      //from pVisit in pv.DefaultIfEmpty()
                                      where pf.ID == PlanFollowUpID
                                      select new SubmissionInfoDropDown()
                                      {
                                          PayerName = (t1.PayerName.IsNull() ? ip.Description : t1.PayerName),
                                          PayerID = t1.PayerID,
                                          Receiver = t2.Name,
                                          Coverage = pp.Coverage == "P" ? "Primary" : (pp.Coverage == "S" ? "Secondary" : (pp.Coverage == "T" ? "Teritary" : "")),
                                          TelePhoneNumber = ins.OfficePhoneNum,
                                          //  PRTelephoneNumber = pVisit.PayerContactNumber, // Comment join bec More than one Paymentvisits are comming in result
                                          SubmitDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                                          DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                          BilledAmount = v.PrimaryBilledAmount,
                                          PlanBalance = v.PrimaryBal,
                                      }).SingleOrDefault();  //
                }
                else if (visit.SecondaryBal.Val() > 0)
                {
                    SubmissionInfo = (from pf in pMContext.PlanFollowUp
                                      join v in pMContext.Visit on pf.VisitID equals v.ID
                                      join pp in pMContext.PatientPlan on v.SecondaryPatientPlanID equals pp.ID
                                      join ip in pMContext.InsurancePlan
                                      on pp.InsurancePlanID equals ip.ID
                                      join edi837 in pMContext.Edi837Payer
                                      on ip.Edi837PayerID equals edi837.ID into Table1
                                      from t1 in Table1.DefaultIfEmpty()
                                      join rec in pMContext.Receiver
                                      on t1.ReceiverID equals rec.ID into Table2
                                      from t2 in Table2.DefaultIfEmpty()
                                      join ins in pMContext.Insurance
                                      on ip.InsuranceID equals ins.ID
                                      //join pv1 in pMContext.PaymentVisit
                                      //on v.ID equals pv1.VisitID into pv
                                      //from pVisit in pv.DefaultIfEmpty()
                                      where pf.ID == PlanFollowUpID
                                      select new SubmissionInfoDropDown()
                                      {
                                          PayerName = (t1.PayerName.IsNull() ? ip.Description : t1.PayerName),
                                          PayerID = t1.PayerID,
                                          Receiver = t2.Name,
                                          Coverage = pp.Coverage == "P" ? "Primary" : (pp.Coverage == "S" ? "Secondary" : (pp.Coverage == "T" ? "Teritary" : "")),
                                          TelePhoneNumber = ins.OfficePhoneNum,
                                          //   PRTelephoneNumber = pVisit.PayerContactNumber, // Comment join bec More than one Paymentvisits are comming in result
                                          SubmitDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                                          DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                          BilledAmount = v.PrimaryBilledAmount,
                                          PlanBalance = v.PrimaryBal,
                                      }).SingleOrDefault();


                }
                else if (visit.TertiaryBal.Val() > 0)
                {

                    SubmissionInfo = (from pf in pMContext.PlanFollowUp
                                      join v in pMContext.Visit on pf.VisitID equals v.ID
                                      join pp in pMContext.PatientPlan on v.TertiaryPatientPlanID equals pp.ID
                                      join ip in pMContext.InsurancePlan
                                      on pp.InsurancePlanID equals ip.ID
                                      join edi837 in pMContext.Edi837Payer
                                      on ip.Edi837PayerID equals edi837.ID into Table1
                                      from t1 in Table1.DefaultIfEmpty()
                                      join rec in pMContext.Receiver
                                      on t1.ReceiverID equals rec.ID into Table2
                                      from t2 in Table2.DefaultIfEmpty()
                                      join ins in pMContext.Insurance
                                      on ip.InsuranceID equals ins.ID
                                      //join pv1 in pMContext.PaymentVisit
                                      //on v.ID equals pv1.VisitID into pv
                                      //from pVisit in pv.DefaultIfEmpty()
                                      where pf.ID == PlanFollowUpID
                                      select new SubmissionInfoDropDown()
                                      {
                                          PayerName = (t1.PayerName.IsNull() ? ip.Description : t1.PayerName),
                                          PayerID = t1.PayerID,
                                          Receiver = t2.Name,
                                          Coverage = pp.Coverage == "P" ? "Primary" : (pp.Coverage == "S" ? "Secondary" : (pp.Coverage == "T" ? "Teritary" : "")),
                                          TelePhoneNumber = ins.OfficePhoneNum,
                                          //  PRTelephoneNumber = pVisit.PayerContactNumber,// Comment join bec More than one Paymentvisits are comming in result
                                          SubmitDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                                          DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                          BilledAmount = v.PrimaryBilledAmount,
                                          PlanBalance = v.PrimaryBal,
                                      }).SingleOrDefault();
                }
            }
        }
        public class CPlanFollowup
        {
            public long? ReasonID { get; set; }
            public long? GroupID { get; set; }
            public long? ActionID { get; set; }
            public string Practice { get; set; }
            public string Location { get; set; }
            public string PlanName { get; set; }
            public long? VisitID { get; set; }
            public string AccountNum { get; set; }
            public DateTime? SubmitDate { get; set; }
            public DateTime? TickleDate { get; set; }
            public DateTime? DOS { get; set; }
            public bool? Resolved { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }


        }
        public class GPlanFollowup
        {
            // We are hiding first column in Grid.
            // So Adding ID column extra, so that VisitID could not be hidden.
            public long ID { get; set; }
            public long PlanFollowUpID { get; set; }
            public string EntryDate { get; set; }
            public long? VisitID { get; set; }
            public string DOS { get; set; }
            public string AccountNum { get; set; }
            public string Patient { get; set; }
            public long PatientID { get; set; }
            public string Practice { get; set; }
            public long PracticeID { get; set; }
            public string Location { get; set; }
            public long LocationID { get; set; }
            public string Provider { get; set; }
            public long ProviderID { get; set; }
            public decimal? PlanBalance { get; set; }
            public string Group { get; set; }
            public long ?GroupID { get; set; }
            public string Reason { get; set; }
            public long ?ReasonID { get; set; }
            public string Action { get; set; }
            public long ?ActionID { get; set; }
            public string RemitCode { get; set; }
            public string SubmitDate { get; set; }
            public string TickleDate { get; set; }
            public string FollowupAge { get; set; }
            public string IsSubmitted { get; set; }
            public string PlanName { get; set; }
            public long InsurancePlanID { get; set; }
            public long? AdjustmentCodeID { get; set; }
            public string AdjustmentCode { get; set; }
            public decimal? BilledAmount { get; set; }

        }

        public class GFollowupCharge
        {
            public long? ChargeID { get; set; }
            public string DOS { get; set; }
            public string CPT { get; set; }
            public string SubmitDate { get; set; }
            public long? AdjustmentCodeID { get; set; }
            public string AdjustmentCode { get; set; }
            public decimal? BilledAmount { get; set; }
            public string Coverage { get; set; }
            public long? RemarkCode { get; set; }
            public decimal? PlanBalance { get; set; }


        }
    }
}
