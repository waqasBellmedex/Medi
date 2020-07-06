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
using static MediFusionPM.ViewModels.VMAdjustmentCode;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;
using WebApplication1.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class AdjustmentCodeController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public AdjustmentCodeController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }
        [Route("GetProfiles")]
        [HttpGet]
        public async Task<ActionResult<VMAdjustmentCode>> GetProfiles(long id)
        {
            ViewModels.VMAdjustmentCode obj = new ViewModels.VMAdjustmentCode();
            obj.GetProfiles(_context);
            return obj;
        }



        [HttpGet]
        [Route("GetAdjustmentCodes")]
        public async Task<ActionResult<IEnumerable<AdjustmentCode>>> GetAdjustmentCodes()
        {
            try
            {
                return await _context.AdjustmentCode.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("FindAdjustmentCode/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AdjustmentCode>> FindAdjustmentCode(long id)
        {
            var AdjustmentCode = await _context.AdjustmentCode.FindAsync(id);

            if (AdjustmentCode == null)
            {
                return NotFound();
            }
           
            return AdjustmentCode;
        }

        [HttpPost]
        [Route("FindAdjustmentCode")]
        public async Task<ActionResult<IEnumerable<GAdjustmentCode>>> FindAdjustmentCode(CAdjustmentCode CAdjustmentCode)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.AdjustmentCodesSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindAdjustmentCode(CAdjustmentCode, PracticeId);
        }
        private List<GAdjustmentCode> FindAdjustmentCode(CAdjustmentCode CAdjustmentCode, long PracticeId)
        {
            List<GAdjustmentCode> data = (from aCode in _context.AdjustmentCode
                          where
                          (CAdjustmentCode.AdjustmentCode.IsNull() ? true : aCode.Code.Equals(CAdjustmentCode.AdjustmentCode)) &&
                          (CAdjustmentCode.Description.IsNull() ? true : aCode.Description.Contains(CAdjustmentCode.Description)) 
                         
                          select new GAdjustmentCode()
                          {
                            ID = aCode.ID,
                            AdjustmentCode = aCode.Code,
                            Description = aCode.Description,
                            Type = aCode.Type,
                          }).ToList();
            return data;
        }


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CAdjustmentCode CAdjustmentCode)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GAdjustmentCode> data = FindAdjustmentCode(CAdjustmentCode, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CAdjustmentCode, "Adjustment Code Report");
        }


        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CAdjustmentCode CAdjustmentCode)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GAdjustmentCode> data = FindAdjustmentCode(CAdjustmentCode, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SaveAdjustmentCode")]
        [HttpPost]
        public async Task<ActionResult<AdjustmentCode>> SaveAdjustmentCode(AdjustmentCode item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.AdjustmentCodesCreate == false)
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
                _context.AdjustmentCode.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.AdjustmentCodesEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.AdjustmentCode.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }


        [Route("DeleteAdjustmentCode/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAdjustmentCode(long id)
        {
            var AdjustmentCode = await _context.AdjustmentCode.FindAsync(id);

            if (AdjustmentCode == null)
            {
                return NotFound();
            }
            _context.AdjustmentCode.Remove(AdjustmentCode);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{AdjustmentCodeID}")]
        [HttpGet("{AdjustmentCodeID}")]
        public List<AdjustmentCodeAudit> FindAudit(long AdjustmentCodeID)
        {
           
            List<AdjustmentCodeAudit> data = (from pAudit in _context.AdjustmentCodeAudit
                                              where pAudit.AdjustmentCodeID == AdjustmentCodeID orderby pAudit.AddedDate descending
                                             select new AdjustmentCodeAudit()
                                             {
                                                 ID = pAudit.ID,
                                                 AdjustmentCodeID = pAudit.AdjustmentCodeID,
                                                 TransactionID = pAudit.TransactionID,
                                                 ColumnName = pAudit.ColumnName,
                                                 CurrentValue = pAudit.CurrentValue,
                                                 NewValue = pAudit.NewValue,
                                                 CurrentValueID = pAudit.CurrentValueID,
                                                 NewValueID = pAudit.NewValueID,
                                                 HostName = pAudit.HostName,
                                                 AddedBy = pAudit.AddedBy,
                                                 AddedDate = pAudit.AddedDate,
                                             }).ToList<AdjustmentCodeAudit>();
           
            return data;
        }

    }
}