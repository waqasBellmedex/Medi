using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using MediFusionPM.BusinessLogic.EraParsing;
using MediFusionPM.Models.TodoApi;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using static MediFusionPM.ViewModels.VMReceiver;
using static MediFusionPM.ViewModels.VMCommon;
using System.Transactions;
using System.Text;
using System.IO;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ReceiverController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public ReceiverController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [HttpGet]
        [Route("GetReceiver")]
        public async Task<ActionResult<IEnumerable<Receiver>>> GetReasons()
        {
            return await _context.Receiver.ToListAsync();
        }

        [Route("FindReceiver/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Receiver>> FindReceiver(long id)
        {
            var Receiver = await _context.Receiver.FindAsync(id);

            if (Receiver == null)
            {
                return NotFound();
            }

            return Receiver;
        }

        [HttpPost]
        [Route("FindReceiver")]
        public async Task<ActionResult<IEnumerable<GReceiver>>> FindReceiver(CReceiver CReceiver)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.receiverSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindReceiver(CReceiver, PracticeId);
        }

        private List<GReceiver> FindReceiver(CReceiver CReceiver, long PracticeId)
        {
            List<GReceiver> data = (from rec in _context.Receiver

                          where
                         (CReceiver.Name.IsNull() ? true : rec.Name.Contains(CReceiver.Name))&& 
                         (CReceiver.Address.IsNull() ? true : rec.Address1.Contains(CReceiver.Address)) 
                          select new GReceiver()
                          {
                              ID = rec.ID,
                              Name = rec.Name,
                              Address = rec.Address1 + ", " + rec.City + ", " + rec.State + ", " + rec.ZipCode,
                              ReceiverID = rec.ID,
                              OrganizationName = rec.X12_837_NM1_40_ReceiverName
                          }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CReceiver CReceiver)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GReceiver> data = FindReceiver(CReceiver, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CReceiver, "Receiver Reports");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CReceiver CReceiver)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GReceiver> data = FindReceiver(CReceiver, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SaveReceiver")]
        [HttpPost]
        public async Task<ActionResult<Receiver>> SaveReceiver(Receiver item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.receiverCreate == false)
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
                _context.Receiver.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.receiverupdate == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Receiver.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }

        [Route("DeleteReceiver/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteReceiver(long id)
        {
            var Receiver = await _context.Receiver.FindAsync(id);

            if (Receiver == null)
            {
                return NotFound();
            }

            _context.Receiver.Remove(Receiver);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("FindAudit/{ReceiverID}")]
        [HttpGet("{ReceiverID}")]
        public List<ReceiverAudit> FindAudit(long ReceiverID)
        {
            List<ReceiverAudit> data = (from pAudit in _context.ReceiverAudit
                                         where pAudit.ReceiverID == ReceiverID
                                        select new ReceiverAudit()
                                         {
                                             ID = pAudit.ID,
                                             ReceiverID= pAudit.ReceiverID,
                                             TransactionID = pAudit.TransactionID,
                                             ColumnName = pAudit.ColumnName,
                                             CurrentValue = pAudit.CurrentValue,
                                             NewValue = pAudit.NewValue,
                                             CurrentValueID = pAudit.CurrentValueID,
                                             NewValueID = pAudit.NewValueID,
                                             HostName = pAudit.HostName,
                                             AddedBy = pAudit.AddedBy,
                                             AddedDate = pAudit.AddedDate,
                                         }).ToList<ReceiverAudit>();
            return data;
        }



    }
}