using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMInsurance;
using MediFusionPM.Models.Audit;


namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class InsuranceController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public InsuranceController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
            // only for Testing 
            if (_context.Insurance.Count() == 0)
            {
                //_context.Insurances.Add(new Insurance { ID = 1, Name = "ABC Insurance" });
              //  _context.SaveChanges();
            }
        }

        [HttpGet]
        [Route("GetInsurances")]
        public async Task<ActionResult<IEnumerable<Insurance>>> GetInsurance()
        {
            return await _context.Insurance.ToListAsync();
        }

        [Route("FindInsurance/{id}")]
        [HttpGet("{id}")]

        public async Task<ActionResult<Insurance>> FindInsurance(long id)
        {
            var Insurance = await _context.Insurance.FindAsync(id);
            if (Insurance == null)
            {
                return NotFound();
            }
            return Insurance;
        }




        [HttpPost]
        [Route("FindInsurances")]
        public async Task<ActionResult<IEnumerable<GInsurance>>> FindInsurances(CInsurance CInsurance)
        {
            //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //    if (UD == null || UD.Rights == null || UD.Rights.InsuranceSearch == false)
            //        return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindInsurances(CInsurance, PracticeId);
        }

        private List<GInsurance> FindInsurances(CInsurance CInsurance, long PracticeId)
        { 
            List<GInsurance> data = (from i in _context.Insurance
                                     where
                                    (CInsurance.Name.IsNull() ? true : i.Name.Contains(CInsurance.Name)) &&
                                    (CInsurance.OrganizationName.IsNull() ? true : i.OrganizationName.Contains(CInsurance.OrganizationName)) &&
                                    (CInsurance.Address.IsNull() ? true : i.Address1.Contains(CInsurance.Address)) &&
                                    (CInsurance.PhoneNumber.IsNull() ? true : i.OfficePhoneNum == (CInsurance.PhoneNumber))
                                     select new GInsurance()
                                    {
                                         ID = i.ID,
                                         Name = i.Name,
                                         OrganizationName = i.OrganizationName,
                                         Address = i.Address1 + ", " + i.City + ", " + i.State + ", " + i.ZipCode,
                                         OfficePhoneNum = i.OfficePhoneNum,
                                         Email = i.Email,
                                         Website = i.Website,
                                     }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CInsurance CInsurance)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            Debug.WriteLine(UD + " UD " + UD.ClientID + "   " + _context);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GInsurance> data = FindInsurances(CInsurance, PracticeId);
            Debug.WriteLine(data + "    is the list");
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD,CInsurance, "Insurance Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CInsurance CInsurance)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            Debug.WriteLine(UD + " UD " + UD.ClientID + "   " + _context);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GInsurance> data = FindInsurances(CInsurance, PracticeId);
            Debug.WriteLine(data + "    is the list");
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }


        [Route("SaveInsurance")]
        [HttpPost]
        public async Task<ActionResult<Insurance>> SaveInsurance(Insurance item)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.InsuranceCreate == false)
            return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
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
                _context.Insurance.Add(item);
                await _context.SaveChangesAsync();
            }
           else if (UD.Rights.InsuranceEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Insurance.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }

        [Route("DeleteInsurance/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteInsurance(long id)
        {
            var Insurance = await _context.Insurance.FindAsync(id);

            if (Insurance == null)
            {
                return NotFound();
            }

            _context.Insurance.Remove(Insurance);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("FindAudit/{InsuranceID}")]
        [HttpGet("{InsuranceID}")]
        public List<InsuranceAudit> FindAudit(long InsuranceID)
        {
            List<InsuranceAudit> data = (from pAudit in _context.InsuranceAudit
                                           where pAudit.InsuranceID == InsuranceID
                                           orderby pAudit.AddedDate descending
                                           select new InsuranceAudit()
                                           {
                                               ID = pAudit.ID,
                                               InsuranceID = pAudit.InsuranceID,
                                               TransactionID = pAudit.TransactionID,
                                               ColumnName = pAudit.ColumnName,
                                               CurrentValue = pAudit.CurrentValue,
                                               NewValue = pAudit.NewValue,
                                               CurrentValueID = pAudit.CurrentValueID,
                                               NewValueID = pAudit.NewValueID,
                                               HostName = pAudit.HostName,
                                               AddedBy = pAudit.AddedBy,
                                               AddedDate = pAudit.AddedDate,
                                           }).ToList<InsuranceAudit>();
            return data;
        }
    }
}