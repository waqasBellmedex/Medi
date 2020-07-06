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
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMRemarkCode;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class RemarkCodeController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public RemarkCodeController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [HttpGet]
        [Route("GetRemarkCodes")]
        public async Task<ActionResult<IEnumerable<RemarkCode>>> GetRemarkCodes()
        {
            try
            {
                return await _context.RemarkCode.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("FindRemarkCode/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RemarkCode>> FindRemarkCode(long id)
        {
            var RemarkCode = await _context.RemarkCode.FindAsync(id);

            if (RemarkCode == null)
            {
                return NotFound();
            }
            return RemarkCode;
        }

        [HttpPost]
        [Route("FindRemarkCode")]
        public async Task<ActionResult<IEnumerable<GRemarkCode>>> FindRemarkCode(CRemarkCode CRemarkCode)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.RemarkCodesSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindRemarkCode(CRemarkCode, PracticeId);
        }
        private List<GRemarkCode> FindRemarkCode(CRemarkCode CRemarkCode, long PracticeId)
        {
            List<GRemarkCode> data = (from aCode in _context.RemarkCode
                          where
                          (CRemarkCode.RemarkCode.IsNull() ? true : aCode.Code.Equals(CRemarkCode.RemarkCode)) &&
                          (CRemarkCode.Description.IsNull() ? true : aCode.Description.Contains(CRemarkCode.Description))

                          select new GRemarkCode()
                          {
                              ID = aCode.ID,
                              RemarkCode = aCode.Code,
                              Description = aCode.Description,
                          }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRemarkCode CRemarkCode)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GRemarkCode> data = FindRemarkCode(CRemarkCode, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRemarkCode, "Remark Code Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRemarkCode CRemarkCode)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GRemarkCode> data = FindRemarkCode(CRemarkCode, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
            
        [Route("SaveRemarkCode")]
        [HttpPost]
        public async Task<ActionResult<RemarkCode>> SaveRemarkCode(RemarkCode item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.RemarkCodesCreate == false)
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
                _context.RemarkCode.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.RemarkCodesEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.RemarkCode.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }

        [Route("DeleteRemarkCode/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteRemarkCode(long id)
        {
            var RemarkCode = await _context.RemarkCode.FindAsync(id);

            if (RemarkCode == null)
            {
                return NotFound();
            }
            _context.RemarkCode.Remove(RemarkCode);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{RemarkCodeID}")]
        [HttpGet("{RemarkCodeID}")]
        public List<RemarkCodeAudit> FindAudit(long RemarkCodeID)
        {
            List<RemarkCodeAudit> data = (from pAudit in _context.RemarkCodeAudit
                                          where pAudit.RemarkCodeID == RemarkCodeID
                                          select new RemarkCodeAudit()
                                          {
                                              ID = pAudit.ID,
                                              RemarkCodeID = pAudit.RemarkCodeID,
                                              TransactionID = pAudit.TransactionID,
                                              ColumnName = pAudit.ColumnName,
                                              CurrentValue = pAudit.CurrentValue,
                                              NewValue = pAudit.NewValue,
                                              CurrentValueID = pAudit.CurrentValueID,
                                              NewValueID = pAudit.NewValueID,
                                              HostName = pAudit.HostName,
                                              AddedBy = pAudit.AddedBy,
                                              AddedDate = pAudit.AddedDate,
                                          }).ToList<RemarkCodeAudit>();
            return data;
        }


    }
}