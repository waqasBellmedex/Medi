using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.Audit;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPaymentLedger;
using static MediFusionPM.ViewModels.VMChargeSubmissionHistory;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ChargeController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public ChargeController(ClientDbContext context, MainContext contextMain)
        {
            _context = context; 
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }









        public List<GPaymentLedger> GetChargePaymentLedgers(ClientDbContext _context, long ChargeID)
        {

            List<GPaymentLedger> Payment = (from pLedg in _context.PaymentLedger
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
            return Payment;


        }

        public List<ResubmitHistory> GetChargeResubmitHistory(ClientDbContext _context, long ChargeID)
        {
            List<ResubmitHistory> history = (from rh in _context.ResubmitHistory

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
            return history;

        }

        public List<ChargeAudit> GetChargeAudit(ClientDbContext _context, long ChargeID)
        {
            List<ChargeAudit> audit = (from cAudit in _context.ChargeAudit
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
            return audit;

        }

        public List<GChargeSubmissionHistory> GetSubmissionHistory(ClientDbContext _context, long ChargeID)
        {

            List<GChargeSubmissionHistory> data = (from csh in _context.ChargeSubmissionHistory
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
            return data;

        }








        [Route("FindCharge/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Charge>> FindCharge(long id)
        {
            var Charge = await _context.Charge.FindAsync(id);

            if (Charge == null)
            {
                return NotFound();
            }

            return Charge;
        }


        [Route("GetChargeAudit/{ChargeID}")]
        [HttpGet("{ChargeID}")]
        public List<ChargeAudit> GetChargeAudit(long ChargeID)
        {
            List<ChargeAudit> data = (from pAudit in _context.ChargeAudit
                                      where pAudit.ChargeID == ChargeID orderby pAudit.AddedDate descending
                                      select new ChargeAudit()
                                      {
                                          ID = pAudit.ID,
                                          ChargeID = pAudit.ChargeID,
                                          ColumnName = pAudit.ColumnName,
                                          AddedDate = pAudit.AddedDate,
                                          AddedBy = pAudit.AddedBy,
                                          CurrentValueID = pAudit.CurrentValueID,
                                          CurrentValue = pAudit.CurrentValue,
                                          NewValueID = pAudit.NewValueID,
                                          NewValue = pAudit.NewValue,
                                          HostName = pAudit.HostName,
                                      }).ToList();

           
            return data;

        }


        [Route("SaveCharge")]
        [HttpPost]
        public async Task<ActionResult<Charge>> SaveCharge(Charge item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (item.ID == 0)
            {
                item.AddedBy = UD.Email;
                item.AddedDate = DateTime.Now;
                _context.Charge.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Charge.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok(item);
        }


        [Route("DeleteCharge/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteCharge(long id)
        {
           
            var Charge = await _context.Charge.FindAsync(id);

            if (Charge == null)
            {
                return NotFound();
            }

            _context.Charge.Remove(Charge);
            await _context.SaveChangesAsync();
            return Ok();
        }


        //[Route("{ChargeID}")]
        //[HttpGet("{ChargeID}")]
        //public List<ChargeAudit> FindAudit(long ChargeID)
        //{
        //    List<ChargeAudit> data = (from pAudit in _context.ChargeAudit
        //                              where pAudit.ChargeID == ChargeID
        //                              select new ChargeAudit()
        //                              {
        //                                  ID = pAudit.ID,
        //                                  ChargeID = pAudit.ChargeID,
        //                                  TransactionID = pAudit.TransactionID,
        //                                  ColumnName = pAudit.ColumnName,
        //                                  CurrentValue = pAudit.CurrentValue,
        //                                  NewValue = pAudit.NewValue,
        //                                  CurrentValueID = pAudit.CurrentValueID,
        //                                  NewValueID = pAudit.NewValueID,
        //                                  HostName = pAudit.HostName,
        //                                  AddedBy = pAudit.AddedBy,
        //                                  AddedDate = pAudit.AddedDate,
        //                              }).ToList<ChargeAudit>();
        //    return data;
        //}


        [Route("GetChargeDetails/{ChargeID}")]
        [HttpGet("{ChargeID}")]
        public async Task<ActionResult<VMCharge>> GetChargeDetails(long ChargeID)
        {
            ViewModels.VMCharge obj = new ViewModels.VMCharge();
            obj.GetChargeDetails(_context, ChargeID);
            return obj;
        }



    }
}