using MediFusionPM.Models;
using MediFusionPM.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MediFusionPM.ViewModels.VMChargeSubmissionHistory;
using static MediFusionPM.ViewModels.VMPaymentLedger;

namespace MediFusionPM.ViewModels
{
    public class VMCharge
    {
        public List<GPaymentLedger> Payment { get; set; }
        public List<ChargeAudit> audit { get; set; }
        public List<ResubmitHistory> history { get; set; }
        public List<GChargeSubmissionHistory> data { get; set; }
        

        public void GetChargeDetails(ClientDbContext _context, long ChargeID)
        {

                                  Payment = (from pLedg in _context.PaymentLedger
                                            join aCode in _context.AdjustmentCode on pLedg.AdjustmentCodeID equals aCode.ID into Table1
                                            from t1 in Table1.DefaultIfEmpty()
                                            join pChrge in _context.PaymentCharge on pLedg.PaymentChargeID equals pChrge.ID into Table2
                                            from t2 in Table2.DefaultIfEmpty()
                                            join pVisit in _context.PaymentVisit on t2.PaymentVisitID equals pVisit.ID into Table3
                                            from t3 in Table3.DefaultIfEmpty()
                                            join pChck in _context.PaymentCheck on t3.PaymentCheckID equals pChck.ID into Table4
                                            from t4 in Table4.DefaultIfEmpty()
                                            join pPlan in _context.PatientPlan on pLedg.PatientPlanID equals pPlan.ID
                                            where pLedg.ChargeID == ChargeID
                                            orderby pLedg.LedgerDate descending
                                            select new GPaymentLedger
                                            {
                                                ID = pLedg.ID,
                                                ChargeID = pLedg.ChargeID,
                                                VisitID = pLedg.VisitID,
                                                PatientPlanID = pLedg.PatientPlanID,
                                                Covrage = (pPlan.Coverage == "P" ? "PRIMARY" : (pPlan.Coverage == "S" ? "SECONDARY" : (pPlan.Coverage == "T" ? "TERITARY" : ""))),
                                                PaymentChargeID = pLedg.PaymentChargeID,
                                                AdjustmentCodeID = pLedg.AdjustmentCodeID,
                                                AdjustmentCode = t1.Code,
                                                LedgerBy = pLedg.LedgerBy,
                                                LedgerType = pLedg.LedgerType,
                                                LedgerDescription = pLedg.LedgerDescription,
                                                LedgerDate = pLedg.LedgerDate.Format("MM/dd/yyyy"),
                                                Amount = pLedg.Amount,
                                                AddedBy = pLedg.AddedBy,
                                                CheckNumber = t4.CheckNumber
                                            }).ToList();

                             audit = (from cAudit in _context.ChargeAudit
                                      where cAudit.ChargeID == ChargeID
                                      orderby cAudit.AddedDate descending
                                      select new ChargeAudit()
                                      {
                                          ID = cAudit.ID,
                                          ChargeID = cAudit.ChargeID,
                                          ColumnName = cAudit.ColumnName,
                                          AddedDate = cAudit.AddedDate,
                                          AddedBy = cAudit.AddedBy,
                                          CurrentValueID = cAudit.CurrentValueID,
                                          CurrentValue = cAudit.CurrentValue,
                                          NewValueID = cAudit.NewValueID,
                                          NewValue = cAudit.NewValue,
                                          HostName = cAudit.HostName,
                                      }).ToList();
            
                               history = (from rh in _context.ResubmitHistory

                                          where rh.ChargeID == ChargeID
                                          orderby rh.AddedDate descending
                                          select new ResubmitHistory()
                                          {
                                              ID = rh.ID,
                                              ChargeID = rh.ChargeID,
                                              VisitID = rh.VisitID,
                                              AddedBy = rh.AddedBy,
                                              AddedDate = Convert.ToDateTime(rh.AddedDate.Format("MM/dd/yyyy")),
                                              //DateTime.Parse(rh.AddedDate.ToString("MM/dd/yyyy"))
                                              //DateTime.ParseExact(rh.AddedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy",null)
                                          }).ToList();

                                           data = (from csh in _context.ChargeSubmissionHistory
                                                   join r in _context.Receiver on csh.ReceiverID equals r.ID into r1
                                                   from rec in r1.DefaultIfEmpty()
                                                   join pPlan in _context.PatientPlan on csh.PatientPlanID equals pPlan.ID
                                                   join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                   where csh.ChargeID == ChargeID
                                                   orderby csh.AddedDate descending
                                                   select new GChargeSubmissionHistory()
                                                   {
                                                       ID = csh.ID,
                                                       ChargeID = csh.ChargeID,
                                                       SubmitType = (csh.SubmitType == "P" ? "Paper" :
                                                       (csh.SubmitType == "E" ? "EDI" : "")),
                                                       Receiver = rec != null ? rec.Name : csh.FormType,
                                                       AddedDate = csh.AddedDate.ToString("MM/dd/yyyy"),
                                                       User = csh.AddedBy,
                                                       FormType = csh.FormType,
                                                       Coverage = (pPlan.Coverage == "P" ? "PRIMARY" : (pPlan.Coverage == "S" ? "SECONDARY" : (pPlan.Coverage == "T" ? "TERITARY" : ""))),
                                                       Plan = iPlan.PlanName
                                                   }).ToList();
            

        }



    }
}
