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
using static MediFusionPM.ViewModels.VMIcd;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class IcdController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public IcdController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

            // only for Testing 
            if (_context.ICD.Count() == 0)
            {
                //_context.Insurances.Add(new Insurance { ID = 1, Name = "CPT for Fever" });
                 // _context.SaveChanges();
            }
        }
        [HttpGet]
        [Route("GetIcds")]
        public async Task<ActionResult<IEnumerable<ICD>>> GetIcds()
        {
            return await _context.ICD.ToListAsync();
        }
        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMIcd>> GetProfiles(long id)
        {
            ViewModels.VMIcd obj = new ViewModels.VMIcd();
            obj.GetProfiles(_context);
            return obj;
        }


        [Route("FindIcd/{id}")]
        [HttpGet("{id}")]

        public async Task<ActionResult<ICD>> FindIcd(long id)
        {
            
            var Icd = await _context.ICD.FindAsync(id);
            if (Icd == null)
            {
                return NotFound();
            }
            return Icd;
        }
        [HttpGet]
        [Route("FindICDByCode")]
        public ActionResult FindICDByCode(string ICD)
        {

            
            var ICDs = (from i in _context.ICD 
                        where (i.ICDCode.Contains(ICD)
                        || i.Description.Contains(ICD)) // && i.IsActive == true && i.IsDeleted == false
                        select new
                        {
                          ICDID=  i.ID,
                            i.ICDCode,
                            Description =  i.Description,
                        }
                      ).OrderBy(w => w.Description).Take(20)
                      .ToList();
            if (ICDs == null)
            {
                return NotFound();
            }
            
            return Json(ICDs);
        }

        [HttpPost]
        [Route("FindIcd")]
        public async Task<ActionResult<IEnumerable<GIcd>>> FindIcd(CIcd CIcd)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.LocationSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindIcd(CIcd, PracticeId);
        }
        private List<GIcd> FindIcd(CIcd CIcd, long PracticeId)
        {
            List <GIcd> data = (from icd in _context.ICD where 
                            (CIcd.ICDCode.IsNull() ? true : icd.ICDCode.Replace(".", "").Contains(CIcd.ICDCode.Replace(".", ""))) &&
                            (CIcd.Description.IsNull() ? true : icd.Description.Contains(CIcd.Description))

                select new GIcd
                {
                    Id = icd.ID,
                    Description = icd.Description,
                    ICDCode = icd.ICDCode,
                    IsValid = icd.IsValid
                }).ToList();
            return data;
        }
        [HttpGet]
        [Route("FindMostFavouriteICD")]
        public ActionResult FindMostFavouriteICD(string code, long? ProviderID, long? VisitReason, string type)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var FvrtICD = (from mvICD in _context.ICDMostFavourite
                           join icd in _context.ICD on mvICD.ICDID equals icd.ID
                           join Prvd in _context.Provider on mvICD.ProviderID equals Prvd.ID into table1  from pvd in table1.DefaultIfEmpty()
                           join vr in _context.VisitReason on mvICD.VisitReasonID equals vr.ID into table2   from VstRsn in table2.DefaultIfEmpty()
                           where ExtensionMethods.IsNull_Bool(mvICD.Inactive)!=true && (PracticeId > 0 ? mvICD.PracticeID == PracticeId : false)
                           select new
                           {
                               chk = false,
                               mvICD.ID,
                               mvICD.PracticeID,
                               mvICD.ProviderID,
                               mvICD.ICDID,
                               mvICD.VisitReasonID,
                               mvICD.Type,
                               mvICD.AddedBy,
                               mvICD.AddedDate,
                               mvICD.UpdatedBy,
                               mvICD.UpdatedDate, 
                               icd.ICDCode,
                               icd.Description, 
                               PName = pvd.FirstName + ", " + pvd.LastName,
                               VisitReason = VstRsn.Name,
                               VisitDescription = VstRsn.Description,
                           }
                           );
            if (!ExtensionMethods.IsNull(code))
                FvrtICD = FvrtICD.Where(icd => icd.ICDCode.Contains(code));
            if (!ExtensionMethods.IsNull(ProviderID))
                FvrtICD = FvrtICD.Where(icd => icd.ProviderID == ProviderID);
            if (!ExtensionMethods.IsNull(VisitReason))
                FvrtICD = FvrtICD.Where(icd => icd.VisitReasonID == VisitReason);
            if (!ExtensionMethods.IsNull(type))
                FvrtICD = FvrtICD.Where(icd => (icd.Type == type));
            var lst = FvrtICD.ToList();
            if (lst == null)
            {
                return NotFound();
            }
            return Json(lst);
        }
        [HttpPost]
        [Route("SaveMostFavouriteICD")]
        public async Task<ActionResult<CPTMostFavourite>> SaveMostFavouriteICD(ICDMostFavourite mostfavICD)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.PatientCreate == false)
                return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            bool succ = TryValidateModel(mostfavICD);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if(mostfavICD.ID<=0)
            {
              
            }
          
            if (mostfavICD.ID == 0)
            {
                ICDMostFavourite ctp = _context.ICDMostFavourite.Where(w=>w.PracticeID == UD.PracticeID
           && w.ProviderID == mostfavICD.ProviderID && w.VisitReasonID == mostfavICD.VisitReasonID && ExtensionMethods.IsNull_Bool(mostfavICD.Inactive) == false).FirstOrDefault();
                if (ctp != null && ctp.ID > 0)
                {
                    return Json("ICD Code with same Provider,Visit Reason and Type , already exist.");
                }
                mostfavICD.AddedBy = UD.Email.ToString();
                mostfavICD.AddedDate = DateTime.Now;
                mostfavICD.PracticeID = UD.PracticeID;
                _context.ICDMostFavourite.Add(mostfavICD);

            }
            else //if (UD.Rights.PatientEdit == true)
            {
                if(mostfavICD.Inactive!=true)
                {
                    ICDMostFavourite icd = _context.ICDMostFavourite.Where(w => w.ICDID != mostfavICD.ICDID && w.PracticeID == UD.PracticeID
                              && w.ProviderID == mostfavICD.ProviderID && w.VisitReasonID == mostfavICD.VisitReasonID
                              && w.ID != mostfavICD.ID && ExtensionMethods.IsNull_Bool(mostfavICD.Inactive) == false).FirstOrDefault();
                    if (icd != null && icd.ID > 0)
                    {
                        return Json("ICD Code with same Provider,Visit Reason and Type , already exist.");
                    }
                }  
                mostfavICD.UpdatedBy = UD.Email.ToString();
                mostfavICD.UpdatedDate = DateTime.Now;
                _context.ICDMostFavourite.Update(mostfavICD);
            }
            await _context.SaveChangesAsync();
            return Ok(mostfavICD);
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CIcd CIcd)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GIcd> data = FindIcd(CIcd, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CIcd, "ICD Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CIcd CIcd)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GIcd> data = FindIcd(CIcd, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveIcd")]
        [HttpPost]
        public async Task<ActionResult<ICD>> SaveIcd(ICD item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.ICDCreate == false)
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
                _context.ICD.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.ICDEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.ICD.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }

        [Route("DeleteIcd/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteIcd(long id)
        {
            var Icd = await _context.ICD.FindAsync(id);

            if (Icd == null)
            {
                return NotFound();
            }

            _context.ICD.Remove(Icd);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("FindAudit/{ICDID}")]
        [HttpGet("{ICDID}")]
        public List<ICDAudit> FindAudit(long ICDID)
        {
            List<ICDAudit> data = (from pAudit in _context.ICDAudit
                                   where pAudit.ICDID == ICDID
                                   orderby pAudit.AddedDate descending
                                   select new ICDAudit()
                                   {
                                       ID = pAudit.ID,
                                       ICDID = pAudit.ICDID,
                                       TransactionID = pAudit.TransactionID,
                                       ColumnName = pAudit.ColumnName,
                                       CurrentValue = pAudit.CurrentValue,
                                       NewValue = pAudit.NewValue,
                                       CurrentValueID = pAudit.CurrentValueID,
                                       NewValueID = pAudit.NewValueID,
                                       HostName = pAudit.HostName,
                                       AddedBy = pAudit.AddedBy,
                                       AddedDate = pAudit.AddedDate,
                                   }).ToList<ICDAudit>();
            return data;
        }


    }
}