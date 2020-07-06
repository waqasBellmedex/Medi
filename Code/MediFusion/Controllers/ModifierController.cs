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
using static MediFusionPM.ViewModels.VMModifier;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ModifierController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public ModifierController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        [HttpGet]
        [Route("GetModifiers")]
        public async Task<ActionResult<IEnumerable<Modifier>>> GetModifiers()
        {
            return await _context.Modifier.ToListAsync();
        }

        [Route("FindModifier/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Modifier>> FindModifier(long id)
        {
            var Modifier = await _context.Modifier.FindAsync(id);

            if (Modifier == null)
            {
                return NotFound();
            }
            return Modifier;
        }

        [HttpPost]
        [Route("FindModifier")]
        public async Task<ActionResult<IEnumerable<GModifier>>> FindModifier(CModifier CModifier)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.ModifiersSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindModifier(CModifier, PracticeId);
        }
        private List<GModifier> FindModifier(CModifier CModifier, long PracticeId)
        {

            List<GModifier> data = (from p in _context.Modifier
                               where
                               (CModifier.ModifierCode.IsNull() ? true : p.Code.Equals(CModifier.ModifierCode)) &&
                               (CModifier.Description.IsNull() ? true : p.Description.Contains(CModifier.Description))
                               select new GModifier
                               {
                                        Id = p.ID,
                                        ModifierCode = p.Code,
                                        Description = p.Description
                                    }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CModifier CModifier)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GModifier> data = FindModifier(CModifier, PracticeId);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CModifier, "Modifier Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CModifier CModifier)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GModifier> data = FindModifier(CModifier, PracticeId);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveModifier")]
        [HttpPost]
        public async Task<ActionResult<Modifier>> SaveModifier(Modifier item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.ModifiersCreate == false)
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
                _context.Modifier.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.ModifiersEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Modifier.Update(item);
                await _context.SaveChangesAsync();
            }

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(item);
        }

        [Route("DeleteModifier/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteModifier(long id)
        {
            var Modifier = await _context.Modifier.FindAsync(id);

            if (Modifier == null)
            {
                return NotFound();
            }

            _context.Modifier.Remove(Modifier);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [Route("FindAudit/{ModifierID}")]
        [HttpGet("{ProviderID}")]
        public List<ModifierAudit> FindAudit(long ModifierID)
        {
            
            List<ModifierAudit> data = (from pAudit in _context.ModifierAudit
                                        where pAudit.ModifierID == ModifierID
                                        orderby pAudit.AddedDate descending
                                        select new ModifierAudit()
                                        {
                                            ID = pAudit.ID,
                                            ModifierID = pAudit.ModifierID,
                                            TransactionID = pAudit.TransactionID,
                                            ColumnName = pAudit.ColumnName,
                                            CurrentValue = pAudit.CurrentValue,
                                            NewValue = pAudit.NewValue,
                                            CurrentValueID = pAudit.CurrentValueID,
                                            NewValueID = pAudit.NewValueID,
                                            HostName = pAudit.HostName,
                                            AddedBy = pAudit.AddedBy,
                                            AddedDate = pAudit.AddedDate,
                                        }).ToList<ModifierAudit>();
            return data;
        }

    }
}