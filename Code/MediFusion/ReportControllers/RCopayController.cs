using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ReportViewModels.RVMCopayReport;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Controllers;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RCopayController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RCopayController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [HttpPost]
        [Route("FindCopayCollected")]
        public async Task<ActionResult<IEnumerable<GCopay>>> FindCopayCollected(CCopay CCopay)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  if (UD == null || UD.Rights == null || UD.Rights.AdjustmentCodesCreate == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");   // Need To Check VisitSearchRights
            return FindCopayCollected(CCopay, UD);
        }

        private List<GCopay> FindCopayCollected(CCopay CCopay, UserInfoData UD)
        {

            List<GCopay> data = (from v in _context.Visit
                                 join pat in _context.Patient on v.PatientID equals pat.ID
                                 join prac in _context.Practice on pat.PracticeID equals prac.ID
                                 //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                 //join u in _context.Users on up.UserID equals u.Id
                                 join loc in _context.Location on pat.LocationId equals loc.ID
                                 join prov in _context.Provider on pat.ProviderID equals prov.ID
                                 where prac.ID == UD.PracticeID && 
                                  v.Copay.Val() > 0 &&
                                 //u.Id.ToString() == UD.UserID
                                 (CCopay.LastName.IsNull() ? true : pat.LastName.ToUpper().Contains(CCopay.LastName)) &&
                                 (CCopay.FirstName.IsNull() ? true : pat.FirstName.ToUpper().Contains(CCopay.FirstName)) &&
                                 (CCopay.AccountNum.IsNull() ? true : pat.AccountNum.Equals(CCopay.AccountNum)) &&
                                 (CCopay.LocationID.IsNull() ? true : loc.ID.Equals(CCopay.LocationID)) &&
                                 (CCopay.ProviderID.IsNull() ? true : prov.ID.Equals(CCopay.ProviderID)) &&
                                 (ExtensionMethods.IsBetweenDOS(CCopay.DosTo, CCopay.DosFrom, v.DateOfServiceFrom, v.DateOfServiceFrom)) &&
                                 (ExtensionMethods.IsBetweenDOS(CCopay.EntryDateTo, CCopay.EntryDateFrom, v.AddedDate, v.AddedDate))
                                 //  (CCopay.PendingCopay ? p.IsActive == false : true)


                                 select new GCopay()
                                 {
                                     AccountNum = pat.AccountNum,
                                     VisitID = v.ID,
                                     PatientID = v.PatientID,
                                     PatientName = pat.LastName + ", " + pat.FirstName,
                                     DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                     Copay = v.Copay.Val(),
                                     CopayCollected = v.CopayPaid.Val(),
                                     ProviderID = prov.ID,
                                     Provider = prov.LastName + ", " + prov.FirstName,
                                     LocationID = loc.ID,
                                     Location = loc.Name

                                 }).ToList();
           
            if(CCopay.PendingCopay == true)
            {
                data = (from d in data
                        where d.CopayCollected.Val() > 0
                        select d).ToList();
            }
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CCopay CCopay)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GCopay> data = FindCopayCollected(CCopay, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CCopay, "Copay Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CCopay CCopay)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GCopay> data = FindCopayCollected(CCopay, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }













    }
}