using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMPaymentLedger;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentLedgerController : ControllerBase
    {
        private readonly ClientDbContext _context;

        public PaymentLedgerController(ClientDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentLedgers
        [HttpGet]
        [Route("GetPaymentLedger")]
        public IEnumerable<PaymentLedger> GetpaymentLedger()
        {
            return _context.PaymentLedger;
        }

        //GET: api/PaymentLedgers/5
        [HttpGet("{ChargeID}")]
        [Route("GetPaymentLedger/{ChargeID}")]
        public List<GPaymentLedger> GetpaymentLedger(long ChargeID)
        {
         List<GPaymentLedger> Payment = (from pLedg in _context.PaymentLedger
                          join aCode in _context.AdjustmentCode on pLedg.AdjustmentCodeID equals aCode.ID into Table1
                          from t1 in Table1.DefaultIfEmpty()
                          //join pChrge in _context.PaymentCharge on pLedg.PaymentChargeID equals pChrge.ID into Table2
                          //from t2 in Table2.DefaultIfEmpty()
                          //join pVisit in _context.PaymentVisit on t2.PaymentVisitID equals pVisit.ID into Table3 
                          //from t3 in Table3.DefaultIfEmpty()
                          //join pChck in _context.PaymentCheck on t3.PaymentCheckID equals pChck.ID into Table4
                          //from t4 in Table4.DefaultIfEmpty()
                          join paychr in _context.PaymentCharge on pLedg.PaymentChargeID equals paychr.ID into TempPayChr
                          from paychrleft in TempPayChr.DefaultIfEmpty()
                          join payvisit in _context.PaymentVisit on paychrleft.PaymentVisitID equals payvisit.ID into TempPayVisit
                          from payvisitleft in TempPayVisit.DefaultIfEmpty()
                          join paychk in _context.PaymentCheck on payvisitleft.PaymentCheckID equals paychk.ID into TempPayCheck
                          from paychkleft in TempPayCheck.DefaultIfEmpty()
                          //join patpaymentcharge in _context.PatientPaymentCharge on pLedg.PatientPaymentChargeID equals patpaymentcharge.ID into Temp1
                          //from patpaychr in Temp1.DefaultIfEmpty()
                          //join patientpayment in _context.PatientPayment on patpaychr.PatientPaymentID equals patientpayment.ID into Temp2
                          //from patpay in Temp2.DefaultIfEmpty()
                          join pPlan in _context.PatientPlan on pLedg.PatientPlanID equals pPlan.ID
                           where pLedg.ChargeID == ChargeID orderby pLedg.LedgerDate descending
                            select new GPaymentLedger
                          {
                            ID = pLedg.ID,
                            ChargeID =pLedg.ChargeID,
                            VisitID = pLedg.VisitID,
                            PatientPlanID = pLedg.PatientPlanID,
                            Covrage = (pPlan.Coverage == "P" ? "PRIMARY" :(pPlan.Coverage == "S" ? "SECONDARY" :(pPlan.Coverage == "T" ? "TERITARY" : ""))),
                            PaymentChargeID = pLedg.PaymentChargeID,
                            AdjustmentCodeID = pLedg.AdjustmentCodeID,
                            AdjustmentCode = t1.Code,
                            LedgerBy = pLedg.LedgerBy,
                            LedgerType = pLedg.LedgerType,
                            LedgerDescription = pLedg.LedgerDescription,
                            LedgerDate = pLedg.LedgerDate.Format("MM/dd/yyyy"),
                            Amount = pLedg.Amount,
                            AddedBy = pLedg.AddedBy,
                            CheckNumber = paychkleft.CheckNumber
                            }).ToList<GPaymentLedger>() ;
            return Payment;
        }
        [Route("FindAudit/{PaymentLedgerID}")]
        [HttpGet("{PaymentLedgerID}")]
        public List<PaymentLedgerAudit> FindAudit(long PaymentLedgerID)
        {
            List<PaymentLedgerAudit> data = (from pAudit in _context.PaymentLedgerAudit
                                             where pAudit.PaymentLedgerID == PaymentLedgerID
                                             select new PaymentLedgerAudit()
                                             {
                                                 ID = pAudit.ID,
                                                 PaymentLedgerID = pAudit.PaymentLedgerID,
                                                 TransactionID = pAudit.TransactionID,
                                                 ColumnName = pAudit.ColumnName,
                                                 CurrentValue = pAudit.CurrentValue,
                                                 NewValue = pAudit.NewValue,
                                                 CurrentValueID = pAudit.CurrentValueID,
                                                 NewValueID = pAudit.NewValueID,
                                                 HostName = pAudit.HostName,
                                                 AddedBy = pAudit.AddedBy,
                                                 AddedDate = pAudit.AddedDate,
                                             }).ToList<PaymentLedgerAudit>();
            return data;
        }

    }
}