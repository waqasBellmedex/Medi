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
using static MediFusionPM.ViewModels.VMPos;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PosController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public PosController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;

        }

        [HttpGet]
        [Route("GetPos")]
        public async Task<ActionResult<IEnumerable<POS>>> GetPos()
        {
            return await _context.POS.ToListAsync();
        }

        [Route("FindPos/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<POS>> FindPos(long id)
        {
            var POS = await _context.POS.FindAsync(id);

            if (POS == null)
            {
                return NotFound();
            }

            return POS;
        }

        [HttpPost]
        [Route("FindPos")]
        public async Task<ActionResult<IEnumerable<GPOS>>> FindPos(CPOS CPOS)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.POSSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPos(CPOS, PracticeId);
        }
        private List<GPOS> FindPos(CPOS CPOS, long PracticeId)
        {
            List<GPOS> data = (from p in _context.POS
                           where
                            (CPOS.POSCode.IsNull() ? true : p.PosCode.Contains(CPOS.POSCode)) &&
                            (CPOS.Description.IsNull() ? true : p.Description.Contains(CPOS.Description)) &&
                            (CPOS.Name.IsNull() ? true : p.Name.Contains(CPOS.Name))
                           select new GPOS
                           {
                               Id = p.ID,
                               POSCode = p.PosCode,
                               Description = p.Description,
                               Name = p.Name
                           }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPOS CPOS)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPOS> data = FindPos(CPOS, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CPOS, "POS Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPOS CPOS)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPOS> data = FindPos(CPOS, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SavePos")]
        [HttpPost]
        public async Task<ActionResult<POS>> SavePos(POS item)
        {
           UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
           if (UD == null || UD.Rights == null || UD.Rights.POSCreate == false)
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
                _context.POS.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.POSEdit == true)
            {
                 item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.POS.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);


        }

        [Route("DeletePos/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeletePos(long id)
        {
            var POS = await _context.POS.FindAsync(id);

            if (POS == null)
            {
                return NotFound();
            }

            _context.POS.Remove(POS);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("FindAudit/{PosID}")]
        [HttpGet("{PosID}")]
        public List<POSAudit> FindAudit(long PosID)
        {
            List<POSAudit> data = (from pAudit in _context.POSAudit
                                         where pAudit.POSID == PosID
                                   select new POSAudit()
                                         {
                                             ID = pAudit.ID,
                                             POSID = pAudit.POSID,
                                             TransactionID = pAudit.TransactionID,
                                             ColumnName = pAudit.ColumnName,
                                             CurrentValue = pAudit.CurrentValue,
                                             NewValue = pAudit.NewValue,
                                             CurrentValueID = pAudit.CurrentValueID,
                                             NewValueID = pAudit.NewValueID,
                                             HostName = pAudit.HostName,
                                             AddedBy = pAudit.AddedBy,
                                             AddedDate = pAudit.AddedDate,
                                         }).ToList<POSAudit>();
            return data;
        }

    }
}