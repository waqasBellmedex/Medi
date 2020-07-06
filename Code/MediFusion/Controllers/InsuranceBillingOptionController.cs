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
using MediFusionPM.Models.Audit;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class InsuranceBillingOptionController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public InsuranceBillingOptionController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }


        [Route("DeleteInsuranceBillingOption/{id}")]
        [HttpDelete]
        public async Task<ActionResult> InsuranceBillingOption(long id)
        {
            var InsuranceBilling = await _context.InsuranceBillingoption.FindAsync(id);
            if (InsuranceBilling == null)
            {
                return NotFound();
            }
            _context.InsuranceBillingoption.Remove(InsuranceBilling);
            await _context.SaveChangesAsync();
            return Ok();
        }




    }
}