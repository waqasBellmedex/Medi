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
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMReason;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ReasonController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public ReasonController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [HttpGet]
        [Route("GetReasons")]
        public async Task<ActionResult<IEnumerable<Reason>>> GetReasons()
        {
            try
            {
                return await _context.Reason.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMReason>> GetProfiles(long id)
        {
            ViewModels.VMReason obj = new ViewModels.VMReason();
            obj.GetProfiles(_context);
            return obj;
        }

        [Route("FindReason/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Reason>> FindReason(long id)
        {
            var Reason = await _context.Reason.FindAsync(id);

            if (Reason == null)
            {
                return NotFound();
            }

            return Reason;
        }

        [HttpPost]
        [Route("FindReasons")]
        public async Task<ActionResult<IEnumerable<GReason>>> FindReasons(CReason CReason)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.reasonSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindReasons(CReason, PracticeId);
        }

        private List<GReason> FindReasons(CReason CReason, long PracticeId)
        {
            List<GReason>  data = (from r in _context.Reason
                          join u in _context.Users on r.UserID equals u.Id into Table1
                          from t1 in Table1.DefaultIfEmpty()

                          where
                         (CReason.Name.IsNull() ? true : r.Name.Contains(CReason.Name)) &&
                         (CReason.Description.IsNull() ? true : r.Description.Contains(CReason.Description))&&
                          (CReason.User.IsNull() ? true : t1.LastName.Contains(CReason.User))
                          select new GReason()
                          {
                              ID = r.ID,
                              Name = r.Name,
                              Description = r.Description,
                              User = t1.LastName + ", " + t1.FirstName,
                              UserID = t1.Id,
                          }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CReason CReason)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GReason> data = FindReasons(CReason, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CReason, "Reason Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CReason CReason)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GReason> data = FindReasons(CReason, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        [Route("SaveReason")]
        [HttpPost]
        public async Task<ActionResult<Reason>> SaveReason(Reason item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
           if (UD == null || UD.Rights == null || UD.Rights.reasonCreate == false)
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
                _context.Reason.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.reasonUpdate == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Reason.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }

        [Route("DeleteReason/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteReason(long id)
        {
            var Reason = await _context.Reason.FindAsync(id);

            if (Reason == null)
            {
                return NotFound();
            }

            _context.Reason.Remove(Reason);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [Route("FindAudit/{ReasonID}")]
        [HttpGet("{ReasonID}")]
        public List<ReasonAudit> FindAudit(long ReasonID)
        {
            List<ReasonAudit> data = (from pAudit in _context.ReasonAudit
                                      where pAudit.ReasonID == ReasonID
                                      select new ReasonAudit()
                                      {
                                          ID = pAudit.ID,
                                          ReasonID = pAudit.ReasonID,
                                          TransactionID = pAudit.TransactionID,
                                          ColumnName = pAudit.ColumnName,
                                          CurrentValue = pAudit.CurrentValue,
                                          NewValue = pAudit.NewValue,
                                          CurrentValueID = pAudit.CurrentValueID,
                                          NewValueID = pAudit.NewValueID,
                                          HostName = pAudit.HostName,
                                          AddedBy = pAudit.AddedBy,
                                          AddedDate = pAudit.AddedDate,
                                      }).ToList<ReasonAudit>();
            return data;
        }


    }
}