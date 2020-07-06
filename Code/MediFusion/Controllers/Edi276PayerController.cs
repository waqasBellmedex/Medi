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
using static MediFusionPM.ViewModels.VMEdi276Payer;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class Edi276PayerController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public Edi276PayerController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetEdi276Payers")]
        public async Task<ActionResult<IEnumerable<Edi276Payer>>> GetEdi276Payers()
        {
            return await _context.Edi276Payer.ToListAsync();
        }

        [Route("FindEdi276Payer/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Edi276Payer>> FindEdi276Payer(long id)
        {
            var Edi276Payer = await _context.Edi276Payer.FindAsync(id);

            if (Edi276Payer == null)
            {
                return NotFound();
            }
            return Edi276Payer;
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMEdi276Payer>> GetProfiles(long id)
        {
            ViewModels.VMEdi276Payer obj = new ViewModels.VMEdi276Payer();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpPost]
        [Route("FindEdi276Payers")]
        public async Task<ActionResult<IEnumerable<GEdi276Payer>>> FindEdi276Payers(CEdi276Payer CEdi276Payer)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.EDIStatusSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindEdi276Payers(CEdi276Payer, PracticeId);
        }
        private List<GEdi276Payer> FindEdi276Payers(CEdi276Payer CEdi276Payer, long PracticeId)
        {
            List<GEdi276Payer> data = (from e in _context.Edi276Payer
                                       join r in _context.Receiver on e.ReceiverID equals r.ID
                                       where
                                      (CEdi276Payer.PayerName.IsNull() ? true : e.PayerName.ToUpper().Contains(CEdi276Payer.PayerName)) &&
                                      (CEdi276Payer.PayerId.IsNull() ? true : e.PayerID.Equals(CEdi276Payer.PayerId)) &&
                                      (CEdi276Payer.ReceiverId.IsNull() ? true : r.ID.Equals(CEdi276Payer.ReceiverId))

                                       select new GEdi276Payer()
                                       {
                                           Id = e.ID,
                                           PayerId = e.PayerID,
                                           PayerName = e.PayerName,
                                           ReceiverName = r.Name
                                       }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CEdi276Payer CEdi276Payer)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GEdi276Payer> data = FindEdi276Payers(CEdi276Payer, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CEdi276Payer, "Edi276Payer Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CEdi276Payer CEdi276Payer)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GEdi276Payer> data = FindEdi276Payers(CEdi276Payer, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SaveEdi276Payer")]
        [HttpPost]
        public async Task<ActionResult<Edi276Payer>> SaveEdi276Payer(Edi276Payer item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.EDIStatusCreate == false)
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
                _context.Edi276Payer.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.EDIStatusEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Edi276Payer.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }
        [Route("DeleteEdi276Payer/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteEdi276Payer(long id)
        {
            var Edi276Payer = await _context.Edi276Payer.FindAsync(id);

            if (Edi276Payer == null)
            {
                return NotFound();
            }

            _context.Edi276Payer.Remove(Edi276Payer);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{Edi276PayerID}")]
        [HttpGet("{Edi276PayerID}")]
        public List<Edi276PayerAudit> FindAudit(long Edi276PayerID)
        {
            List<Edi276PayerAudit> data = (from pAudit in _context.Edi276PayerAudit
                                           where pAudit.Edi276PayerID == Edi276PayerID
                                           orderby pAudit.AddedDate descending
                                           select new Edi276PayerAudit()
                                           {
                                               ID = pAudit.ID,
                                               Edi276PayerID = pAudit.Edi276PayerID,
                                               TransactionID = pAudit.TransactionID,
                                               ColumnName = pAudit.ColumnName,
                                               CurrentValue = pAudit.CurrentValue,
                                               NewValue = pAudit.NewValue,
                                               CurrentValueID = pAudit.CurrentValueID,
                                               NewValueID = pAudit.NewValueID,
                                               HostName = pAudit.HostName,
                                               AddedBy = pAudit.AddedBy,
                                               AddedDate = pAudit.AddedDate,
                                           }).ToList<Edi276PayerAudit>();

            return data;
        }
    }
    }