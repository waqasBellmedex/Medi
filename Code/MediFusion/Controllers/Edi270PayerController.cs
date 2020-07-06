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
using static MediFusionPM.ViewModels.VMEdi270Payer;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class Edi270PayerController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public Edi270PayerController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetEdi270Payers")]
        public async Task<ActionResult<IEnumerable<Edi270Payer>>> GetEdi270Payers()
        {
            return await _context.Edi270Payer.ToListAsync();
        }

        [Route("FindEdi270Payer/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Edi270Payer>> FindEdi270Payer(long id)
        {
            var Edi270Payer = await _context.Edi270Payer.FindAsync(id);

            if (Edi270Payer == null)
            {
                return NotFound();
            }
            return Edi270Payer;
        }


        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMEdi270Payer>> GetProfiles(long id)
        {
            ViewModels.VMEdi270Payer obj = new ViewModels.VMEdi270Payer();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpPost]
        [Route("FindEdi270Payers")]
        public async Task<ActionResult<IEnumerable<GEdi270Payer>>> FindEdi270Payers(CEdi270Payer CEdi270Payer)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.EDIEligiBilitySearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindEdi270Payers(CEdi270Payer, PracticeId);
        }
        private List<GEdi270Payer> FindEdi270Payers(CEdi270Payer CEdi270Payer, long PracticeId)
        {

            List<GEdi270Payer> data = (from e in _context.Edi270Payer
                          join r in _context.Receiver on  e.ReceiverID equals r.ID
                          where
                         (CEdi270Payer.PayerName.IsNull() ? true : e.PayerName.ToUpper().Contains(CEdi270Payer.PayerName)) &&
                         (CEdi270Payer.PayerId.IsNull() ? true : e.PayerID.Equals(CEdi270Payer.PayerId)) &&
                         (CEdi270Payer.ReceiverId.IsNull() ? true : r.ID.Equals(CEdi270Payer.ReceiverId))

                          select new GEdi270Payer()
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
        public async Task<IActionResult> Export(CEdi270Payer CEdi270Payer)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GEdi270Payer> data = FindEdi270Payers(CEdi270Payer, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CEdi270Payer, "Edi270Payer Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CEdi270Payer CEdi270Payer)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GEdi270Payer> data = FindEdi270Payers(CEdi270Payer, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveEdi270Payer")]
        [HttpPost]
        public async Task<ActionResult<Edi270Payer>> SaveEdi270Payer(Edi270Payer item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.EDIEligiBilityCreate == false)
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
                _context.Edi270Payer.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.EDIEligiBilityEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Edi270Payer.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }

        [Route("DeleteEdi270Payer/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteEdi270Payer(long id)
        {
            var Edi270Payer = await _context.Edi270Payer.FindAsync(id);

            if (Edi270Payer == null)
            {
                return NotFound();
            }

            _context.Edi270Payer.Remove(Edi270Payer);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{Edi270PayerID}")]
        [HttpGet("{Edi270PayerID}")]
        public List<Edi270PayerAudit> FindAudit(long Edi270PayerID)
        {
            List<Edi270PayerAudit> data = (from pAudit in _context.Edi270PayerAudit
                                        where pAudit.Edi270PayerID == Edi270PayerID
                                        orderby pAudit.AddedDate descending
                                        select new Edi270PayerAudit()
                                        {
                                            ID = pAudit.ID,
                                            Edi270PayerID = pAudit.Edi270PayerID,
                                            TransactionID = pAudit.TransactionID,
                                            ColumnName = pAudit.ColumnName,
                                            CurrentValue = pAudit.CurrentValue,
                                            NewValue = pAudit.NewValue,
                                            CurrentValueID = pAudit.CurrentValueID,
                                            NewValueID = pAudit.NewValueID,
                                            HostName = pAudit.HostName,
                                            AddedBy = pAudit.AddedBy,
                                            AddedDate = pAudit.AddedDate,
                                        }).ToList<Edi270PayerAudit>();

            return data;
        }
    }
}