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


namespace MediFusionPM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PaymentVisitController : Controller
    {
        private readonly ClientDbContext _context;

        public PaymentVisitController(ClientDbContext context)
        {
            _context = context;


        }


        [Route("SavePaymentVisit")]
        [HttpPost]
        public async Task<ActionResult<PaymentVisit>> SavePaymentVisit(PaymentVisit item)
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
                _context.PaymentVisit.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PaymentVisit.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok(item);
        }


        [Route("DeletePaymentVisit/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePaymentVisit(long id)
        {
            var PaymentCharge = _context.PaymentCharge.Where(p => p.PaymentVisitID == id );
            if(PaymentCharge!=null)
            {
                foreach (PaymentCharge pc in PaymentCharge)
                {
                    _context.PaymentCharge.Remove(pc);
                     
                }
                _context.SaveChanges();
            }
            var PaymentVisit = await _context.PaymentVisit.FindAsync(id);

            if (PaymentVisit == null)
            {
                return NotFound();
            }

            long? PaymentCheckID = PaymentVisit.PaymentCheckID;
            _context.PaymentVisit.Remove(PaymentVisit);
            await _context.SaveChangesAsync();

            PaymentCheck paymentCheck = _context.PaymentCheck.Find(PaymentCheckID);
            if (paymentCheck != null)
            {
                paymentCheck.NumberOfVisits = paymentCheck.NumberOfVisits - 1;
                paymentCheck.CheckAmount = paymentCheck.CheckAmount.Val() - PaymentVisit.PaidAmount.Val();
                paymentCheck.AppliedAmount = paymentCheck.AppliedAmount.Val() - PaymentVisit.PaidAmount.Val();
                _context.PaymentCheck.Update(paymentCheck);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }


    }
}