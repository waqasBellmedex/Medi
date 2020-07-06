using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMPaymentCharge;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PaymentChargeController : Controller
    {
        private readonly ClientDbContext _context;
        public PaymentChargeController(ClientDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetPaymentCharges")]
        public async Task<ActionResult<IEnumerable<PaymentCharge>>> GetPaymentCharges()
        {
            return await _context.PaymentCharge.ToListAsync();
        }

        [Route("FindPaymentCharge/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentCharge>> FindPaymentCharge(long id)
        {
            var PaymentCharge = await _context.PaymentCharge.FindAsync(id);

            if (PaymentCharge == null)
            {
                return NotFound();
            }

            return PaymentCharge;
        }

        [HttpPost]
        [Route("FindPaymentCharges")]
        public async Task<ActionResult<IEnumerable<GPaymentCharge>>> FindPaymentCharges(CPaymentCharge CPaymentCharge)
        {
            return await (from pChrge in _context.PaymentCharge
                          join pVisit in _context.PaymentVisit on pChrge.PaymentVisitID equals pVisit.ID
                          join pCheck in _context.PaymentCheck on pVisit.PaymentCheckID equals pCheck.ID
                          join prac in _context.Practice on pCheck.PracticeID equals prac.ID
                          where
                         (CPaymentCharge.EntryDateFrom != null && CPaymentCharge.EntryDateTo != null ?
                         pChrge.AddedDate.Date >= CPaymentCharge.EntryDateFrom && pChrge.AddedDate.Date <= CPaymentCharge.EntryDateTo
                         :(CPaymentCharge.EntryDateFrom != null ? pChrge.AddedDate.Date >= CPaymentCharge.EntryDateFrom : true)) &&
                         (CPaymentCharge.CheckDateFrom != null && CPaymentCharge.CheckDateTo != null ?
                         ((DateTime)pCheck.CheckDate).Date >= CPaymentCharge.CheckDateFrom && ((DateTime)pCheck.CheckDate).Date <= CPaymentCharge.CheckDateTo
                         :(CPaymentCharge.CheckDateFrom != null ? ((DateTime)pCheck.CheckDate).Date >= CPaymentCharge.CheckDateFrom : true)) &&
                         // (CPaymentCharge.CheckDateTo == null ? true :  object.Equals(pCheck.CheckDate,CPaymentCharge.CheckDateTo))&&
                         (CPaymentCharge.CheckNumber.IsNull() ? true : pCheck.CheckNumber.Equals(CPaymentCharge.CheckNumber))&&
                         (CPaymentCharge.Practice.IsNull() ? true : prac.Name.Contains(CPaymentCharge.Practice))&&
                         (CPaymentCharge.Payer.IsNull() ? true : pCheck.PayeeName.Contains(CPaymentCharge.Payer))
                          select new GPaymentCharge()
                          {
                            CheckNumber   = pCheck.CheckNumber,
                            PaymentMethod = pCheck.PaymentMethod,
                            CheckDate =  pCheck.CheckDate.Format("MM/dd/yyyy"),
                            CheckAmount = pCheck.CheckAmount,
                            Status = pCheck.Status == "N" ? "Regular" :(pCheck.Status == "P" ? "Paid" : (pCheck.Status == "C" ? "" : "")) ,
                            ReceiverID = pCheck.ReceiverID,
                            Appliedamount = pCheck.AppliedAmount,
                            PostedAmount = pCheck.PostedAmount,
                            NumberOfVisits = pCheck.NumberOfVisits,
                            NumberOfPatients= pCheck.NumberOfPatients,
                            payer = pCheck.PayerName,
                            payee =pCheck.PayeeName,
                          }).ToListAsync();
        }

        [Route("SavePaymentcharge")]
        [HttpPost]
        public async Task<ActionResult<PaymentCharge>> SavePaymentcharge(PaymentCharge item)
        {
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (item.ID.Equals(0))
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context.PaymentCharge.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PaymentCharge.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [Route("FindAudit/{PaymentChargeID}")]
        [HttpGet("{PaymentChargeID}")]
        public List<PaymentChargeAudit> FindAudit(long PaymentChargeID)
        {
            List<PaymentChargeAudit> data = (from pAudit in _context.PaymentChargeAudit
                                             where pAudit.PaymentChargeID == PaymentChargeID
                                             orderby pAudit.AddedDate descending
                                             select new PaymentChargeAudit()
                                               {
                                                   ID = pAudit.ID,
                                                   PaymentChargeID = pAudit.PaymentChargeID,
                                                   TransactionID = pAudit.TransactionID,
                                                   ColumnName = pAudit.ColumnName,
                                                   CurrentValue = pAudit.CurrentValue,
                                                   NewValue = pAudit.NewValue,
                                                   CurrentValueID = pAudit.CurrentValueID,
                                                   NewValueID = pAudit.NewValueID,
                                                   HostName = pAudit.HostName,
                                                   AddedBy = pAudit.AddedBy,
                                                   AddedDate = pAudit.AddedDate,
                                               }).ToList<PaymentChargeAudit>();
            return data;
        }
    }
}