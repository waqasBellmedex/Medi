using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static MediFusionPM.ViewModels.VMRefProvider;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class RefProviderController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RefProviderController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            // Only for testing
            if (_context.RefProvider.Count() == 0)
            {
                //_context.RefProviders.Add(new RefProvider { ID = 1, Name = "DBrown",NPI = "4288866587"});
                //_context.SaveChanges();
            }

        }

        [HttpGet]
        [Route("GetRefProviders")]
        public async Task<ActionResult<IEnumerable<RefProvider>>> getRefProvider()
        {
            return await _context.RefProvider.ToArrayAsync();

        }

        [Route("FindRefProvider/{id}")]
        [HttpGet("{id}")]

        public async Task<ActionResult<RefProvider>> FindRefProvider(long id)
        {
            var RefProvider = await _context.RefProvider.FindAsync(id);
            if (RefProvider == null)
            {
                return NotFound();
            }
            return RefProvider;
        }

        [HttpPost]
        [Route("FindRefProviders")]

        public async Task<ActionResult<IEnumerable<GRefProvider>>> FindRefProvider(CRefProvider CRefProvider)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.ReferringProviderSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindRefProviders(CRefProvider, PracticeId);
        }

        private List<GRefProvider> FindRefProviders(CRefProvider CRefProvider, long PracticeId)
        {
            List<GRefProvider> data = (from rPro in _context.RefProvider
                          join prac in _context.Practice on rPro.PracticeID equals prac.ID
                          //join up in _context.UserPractices on prac.ID equals up.PracticeID
                          //join u in _context.Users on up.UserID equals u.Id
                          where prac.ID == PracticeId && 
                          //u.Id.ToString() == UD.UserID
                          //&& //p.AddedBy == Email && 
                         // u.IsUserBlock == false &&
                           (CRefProvider.Name.IsNull() ? true : rPro.Name.Contains(CRefProvider.Name)) &&
                           (CRefProvider.LastName.IsNull() ? true : rPro.LastName.Contains(CRefProvider.LastName)) &&
                           (CRefProvider.FirstName.IsNull() ? true : rPro.FirstName.Contains(CRefProvider.FirstName)) &&
                           (CRefProvider.NPI.IsNull() ? true : rPro.NPI.Equals(CRefProvider.NPI)) &&
                           (CRefProvider.SSN.IsNull() ? true : rPro.SSN .Equals(CRefProvider.SSN)) &&
                           (CRefProvider.TaxonomyCode.IsNull() ? true : rPro.TaxonomyCode .Equals(CRefProvider.TaxonomyCode))
                          select new GRefProvider()
                          {
                              ID = rPro.ID,
                              Name = rPro.Name,
                              LastName = rPro.LastName,
                              FirstName = rPro.FirstName,
                              NPI = rPro.NPI,
                              SSN = rPro.SSN,
                              TaxonomyCode = rPro.TaxonomyCode,
                              Address = rPro.Address1 + ", " + rPro.City + ", " + rPro.State + ", " + rPro.ZipCode,
                              OfficePhoneNum = rPro.OfficePhoneNum

                          }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRefProvider CRefProvider)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
           // Debug.WriteLine(UD + " UD " + UD.ClientID + "   " + _context);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GRefProvider> data = FindRefProviders(CRefProvider, PracticeId);
            Debug.WriteLine(data + "    is the list");
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRefProvider, "Reference Provider Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRefProvider CRefProvider)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            Debug.WriteLine(UD + " UD " + UD.ClientID + "   " + _context);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GRefProvider> data = FindRefProviders(CRefProvider, PracticeId);
           // Debug.WriteLine(data + "    is the list");
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SaveRefProvider")]
        [HttpPost]
        public async Task<ActionResult<RefProvider>> SaveRefProvider(RefProvider item)
        {
          UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
          if (UD == null || UD.Rights == null || UD.Rights.ReferringProviderCreate == false)
          return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            bool RefProviderExists = _context.RefProvider.Count(p => p.Name == item.Name && item.ID == 0) > 0;
            if (RefProviderExists)
            {
                return BadRequest("Ref Provider With This Name Already Exists");
            }
            if (item.ID == 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context.RefProvider.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.ReferringProviderEdit == true)
            {
                bool RefProviderExistsUpdate = _context.RefProvider.Any(p => p.Name == item.Name && item.ID != p.ID);

                if (RefProviderExistsUpdate == true)
                {
                    return BadRequest("Already Exists Please Enter Different RefProvider Name");
                }
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.RefProvider.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(item);
        }

        [Route("DeleteRefProvider/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteRefProvider(long id)
        {
            var RefProvider = await _context.RefProvider.FindAsync(id);

            if (RefProvider == null)
            {
                return NotFound();
            }

            _context.RefProvider.Remove(RefProvider);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("FindAudit/{RefProviderID}")]
        [HttpGet("{RefProviderID}")]
        public List<RefProviderAudit> FindAudit(long RefProviderID)
        {
            List<RefProviderAudit> data = (from pAudit in _context.RefProviderAudit
                                        where pAudit.RefProviderID == RefProviderID orderby pAudit.AddedDate descending
                                           select new RefProviderAudit()
                                        {
                                            ID = pAudit.ID,
                                            RefProviderID = pAudit.RefProviderID,
                                            TransactionID = pAudit.TransactionID,
                                            ColumnName = pAudit.ColumnName,
                                            CurrentValue = pAudit.CurrentValue,
                                            NewValue = pAudit.NewValue,
                                            CurrentValueID = pAudit.CurrentValueID,
                                            NewValueID = pAudit.NewValueID,
                                            HostName = pAudit.HostName,
                                            AddedBy = pAudit.AddedBy,
                                            AddedDate = pAudit.AddedDate,
                                        }).ToList<RefProviderAudit>();
            return data;
        }

    }
}