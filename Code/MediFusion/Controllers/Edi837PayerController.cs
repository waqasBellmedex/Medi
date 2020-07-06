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
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMEdi837Payer;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class Edi837PayerController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public Edi837PayerController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetEdi837Payers")]
        public async Task<ActionResult<IEnumerable<Edi837Payer>>> GetEdi837Payers()
        {
            return await _context.Edi837Payer.ToListAsync();
        }

        [Route("FindEdi837Payer/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Edi837Payer>> FindEdi837Payer(long id)
        {
            var Edi837Payer = await _context.Edi837Payer.FindAsync(id);

            if (Edi837Payer == null)
            {
                return NotFound();
            }
            return Edi837Payer;
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMEdi837Payer>> GetProfiles(long id)
        {
            ViewModels.VMEdi837Payer obj = new ViewModels.VMEdi837Payer();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpPost]
        [Route("FindEdi837Payers")]
        public async Task<ActionResult<IEnumerable<GEdi837Payer>>> FindEdi837Payers(CEdi837Payer CEdi837Payer)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.EDISubmitSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindEdi837Payers(CEdi837Payer, PracticeId); 
        }
        private List<GEdi837Payer> FindEdi837Payers(CEdi837Payer CEdi837Payer, long PracticeId)
        {
            List<GEdi837Payer> data = (from e in _context.Edi837Payer
                          join r in _context.Receiver on e.ReceiverID equals r.ID
                          where
                         (CEdi837Payer.PayerName.IsNull() ? true : e.PayerName.ToUpper().Contains(CEdi837Payer.PayerName)) &&
                         (CEdi837Payer.PayerId.IsNull() ? true : e.PayerID.Equals(CEdi837Payer.PayerId))&&
                         (CEdi837Payer.ReceiverId.IsNull() ? true : r.ID.Equals(CEdi837Payer.ReceiverId))

                          select new GEdi837Payer()
                          {
                              Id = e.ID,
                              PayerId = e.PayerID,
                              PayerName = e.PayerName,
                              ReceiverName = r.Name,
                          }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CEdi837Payer CEdi837Payer)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GEdi837Payer> data = FindEdi837Payers(CEdi837Payer, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CEdi837Payer, "Edi837Payer Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CEdi837Payer CEdi837Payer)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GEdi837Payer> data = FindEdi837Payers(CEdi837Payer, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveEdi837Payer")]
        [HttpPost]
        public async Task<ActionResult<Edi837Payer>> SaveEdi837Payer(Edi837Payer item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.EDISubmitCreate == false)
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
                _context.Edi837Payer.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.EDISubmitEdit == true)
            {

                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Edi837Payer.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }
        [Route("DeleteEdi837Payer/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteEdi837Payer(long id)
        {
           
            var Edi837Payer = await _context.Edi837Payer.FindAsync(id);

            if (Edi837Payer == null)
            {
                return NotFound();
            }

            _context.Edi837Payer.Remove(Edi837Payer);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("FindAudit/{Edi837PayerID}")]
        [HttpGet("{Edi837PayerID}")]
        public List<Edi837PayerAudit> FindAudit(long Edi837PayerID)
        {
            List<Edi837PayerAudit> data = (from pAudit in _context.Edi837PayerAudit
                                           where pAudit.Edi837PayerID == Edi837PayerID
                                           orderby pAudit.AddedDate descending
                                           select new Edi837PayerAudit()
                                           {
                                               ID = pAudit.ID,
                                               Edi837PayerID = pAudit.Edi837PayerID,
                                               TransactionID = pAudit.TransactionID,
                                               ColumnName = pAudit.ColumnName,
                                               CurrentValue = pAudit.CurrentValue,
                                               NewValue = pAudit.NewValue,
                                               CurrentValueID = pAudit.CurrentValueID,
                                               NewValueID = pAudit.NewValueID,
                                               HostName = pAudit.HostName,
                                               AddedBy = pAudit.AddedBy,
                                               AddedDate = pAudit.AddedDate,
                                           }).ToList<Edi837PayerAudit>();
            return data;
        }

    }
}