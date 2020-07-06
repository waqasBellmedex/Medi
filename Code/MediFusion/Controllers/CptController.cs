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
using static MediFusionPM.ViewModels.VMCpt;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class CptController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public CptController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
            // Only For Testing
            if (_context.Cpt.Count() == 0)
            {
              //  _context.cpts.Add(new Cpt { Description = "" });
                //_context.SaveChanges();
            }
        }

        [HttpGet]
        [Route("GetCpts")]
        public async Task<ActionResult<IEnumerable<Cpt>>> GetCpts()
        {
            return await _context.Cpt.ToListAsync();
        }

        [Route("FindCpt/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Cpt>> FindCpt(long id)
        {
           
            var Cpt = await _context.Cpt.FindAsync(id);

            if (Cpt == null)
            {
                return NotFound();
            }
           
            return Cpt;
        }

        [HttpGet]
        [Route("FindCPTbyCode")]
        public ActionResult FindCPTbyCode(string CPTcode)
        {
            
            var cpts = (from cpt in _context.Cpt
                        join mod in _context.Modifier on cpt.Modifier1ID equals mod.ID into table1
                        from mod1T in table1.DefaultIfEmpty()
                        join mod2 in _context.Modifier on cpt.Modifier2ID equals mod2.ID into table2
                        from mod2T in table2.DefaultIfEmpty()
                        join mod3 in _context.Modifier on cpt.Modifier3ID equals mod3.ID into table3
                        from mod3T in table3.DefaultIfEmpty()
                        join mod4 in _context.Modifier on cpt.Modifier4ID equals mod4.ID into table4
                        from mod4T in table4.DefaultIfEmpty()
                        where (cpt.CPTCode.Contains(CPTcode)
                        || cpt.Description.Contains(CPTcode)) && cpt.IsActive == true && cpt.IsDeleted == false
                        select new
                        {
                            CPTID = cpt.ID,
                            cpt.CPTCode,
                            Description = cpt.ShortDescription.Length > 0 ? cpt.ShortDescription : cpt.Description,
                            cpt.Amount ,
                            NDCUnits= cpt.NDCUnits??"",
                            NDCDescription=cpt.NDCDescription??"",
                            Modifier1 = mod1T.Code??"", 
                            modifier2 = mod2T.Code??"" , 
                            Units = cpt.DefaultUnits ??""
                        }
                      ).OrderBy(w => w.Description).Take(20)
                      .ToList();
            if (cpts == null)
            {
                return NotFound();
            }
           
            return Json(cpts);
        }
        [HttpGet]
        [Route("FindMostFavouriteCPT")]
        public ActionResult FindMostFavouriteCPT(string code, long? ProviderID, long? VisitReason, string type)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var FvrtCPT = (from mvcpt in _context.CPTMostFavourite
                           join cpt in _context.Cpt on mvcpt.CPTID equals cpt.ID
                           join Prvd in _context.Provider on mvcpt.ProviderID equals Prvd.ID into table1 from pvd in table1.DefaultIfEmpty()
                           join vr in _context.VisitReason on mvcpt.VisitReasonID equals vr.ID into table2 from VstRsn in table2.DefaultIfEmpty()
                           where ExtensionMethods.IsNull_Bool(mvcpt.Inactive) != true && mvcpt.PracticeID == PracticeId
                           select new
                           {
                               chk=false,
                               mvcpt.ID,
                               mvcpt.PracticeID,
                               mvcpt.ProviderID,
                               mvcpt.CPTID,
                               mvcpt.VisitReasonID,
                               mvcpt.Type,
                               mvcpt.AddedBy,
                               mvcpt.AddedDate,
                               mvcpt.UpdatedBy,
                               mvcpt.UpdatedDate,
                               cpt.CPTCode,
                               Units= cpt.DefaultUnits,
                               cpt.Description,
                               cpt.Amount,
                               PName = pvd.FirstName + ", " + pvd.LastName,
                               VisitReason = VstRsn.Name,
                               VisitDescription = VstRsn.Description,
                           }
                           );
            if (!ExtensionMethods.IsNull(code))
                FvrtCPT = FvrtCPT.Where(cpt => cpt.CPTCode.Contains(code));
            if (!ExtensionMethods.IsNull(ProviderID))
                FvrtCPT = FvrtCPT.Where(cpt => cpt.ProviderID == ProviderID);
            if (!ExtensionMethods.IsNull(VisitReason))
                FvrtCPT = FvrtCPT.Where(cpt => cpt.VisitReasonID == VisitReason);
            if (!ExtensionMethods.IsNull(type)) 
                FvrtCPT = FvrtCPT.Where(cpt => (cpt.Type == type));
            var lst = FvrtCPT.ToList();
            if (lst == null)
            {
                return NotFound();
            }
           
            return Json(lst);
        }
        [HttpPost]
        [Route("SaveMostFavouriteCPT")]
        public async Task<ActionResult<CPTMostFavourite>> SaveMostFavouriteCPT(CPTMostFavourite mostfavcpt)
        {
        UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.PatientCreate == false)
                return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            bool succ = TryValidateModel(mostfavcpt);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (mostfavcpt.ID == 0)
            {
                CPTMostFavourite ctp = _context.CPTMostFavourite.Where( w=> w.PracticeID == UD.PracticeID
           && w.ProviderID == mostfavcpt.ProviderID && w.VisitReasonID == mostfavcpt.VisitReasonID && ExtensionMethods.IsNull_Bool(mostfavcpt.Inactive) == false).FirstOrDefault();
                if (ctp != null && ctp.ID > 0)
                {
                    return Json("CPT Code with same Provider,Visit Reason and Type , already exist.");
                }
                mostfavcpt.AddedBy = UD.Email.ToString();
                mostfavcpt.AddedDate = DateTime.Now;
                mostfavcpt.PracticeID = UD.PracticeID;
                _context.CPTMostFavourite.Add(mostfavcpt);
            }
            else //if (UD.Rights.PatientEdit == true)
            {
                if(mostfavcpt.Inactive!=true)
                {
                    CPTMostFavourite ctp = _context.CPTMostFavourite.Where(w => w.CPTID == mostfavcpt.CPTID && w.PracticeID == UD.PracticeID
           && w.ProviderID == mostfavcpt.ProviderID && w.VisitReasonID == mostfavcpt.VisitReasonID
            && w.ID != mostfavcpt.ID && ExtensionMethods.IsNull_Bool(mostfavcpt.Inactive) == false).FirstOrDefault();
                if (ctp != null && ctp.ID > 0)
                {
                    return Json("CPT Code with same Provider,Visit Reason and Type , already exist.");
                }
                } 
                mostfavcpt.UpdatedBy = UD.Email.ToString();
                mostfavcpt.UpdatedDate = DateTime.Now;
                _context.CPTMostFavourite.Update(mostfavcpt);
            }
            await _context.SaveChangesAsync();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(mostfavcpt);
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMCpt>> GetProfiles(long id)
        {
            ViewModels.VMCpt obj = new ViewModels.VMCpt();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpPost]
        [Route("FindCpts")]
        public async Task<ActionResult<IEnumerable<GCpt>>> FindCpts(CCpt cCpt)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.CPTSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindCpts(cCpt, PracticeId);
        }
        private List<GCpt> FindCpts(CCpt cCpt, long PracticeId)
        {
            List<GCpt> data = (from p in _context.Cpt
                               where
                              (cCpt.Description.IsNull() ? true : p.Description.Contains(cCpt.Description)) &&
                               (cCpt.CPTCode.IsNull() ? true : p.CPTCode.Contains(cCpt.CPTCode)) &&
                               (cCpt.NDCNumber.IsNull() ? true : p.NDCNumber.Contains(cCpt.NDCNumber)) &&
                               (cCpt.NDCDescription.IsNull() ? true : p.NDCDescription.Contains(cCpt.NDCDescription))



                               select new GCpt
                               {
                                 Description = p.ShortDescription,
                                 CPTCode = p.CPTCode,
                                 Amount = p.Amount,
                                 Modifiers = (p.Modifier1ID != null ? p.Modifier1.Code
                                + (p.Modifier2ID != null ? ", " + p.Modifier2.Code +
                                 (p.Modifier3ID != null ? ", " + p.Modifier3.Code +
                                 (p.Modifier4ID != null ? ", " + p.Modifier4.Code : "") : "") : "") : ""),
                                   ID = p.ID,
                                   NDCNumber = p.NDCNumber,
                                   TypeOfService = p.TypeOfService != null ? p.TypeOfService.Code : ""
                               }).ToList();
          
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CCpt CCpt)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GCpt> data = FindCpts(CCpt, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD,CCpt,"Cpt Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CCpt CCpt)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GCpt> data = FindCpts(CCpt, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveCpt")]
        [HttpPost]
        public async Task<ActionResult<Cpt>> SaveCpt(Cpt item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.CPTCreate == false)
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
                _context.Cpt.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.CPTEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Cpt.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
           
            return Ok(item);
        }
        [Route("DeleteCpt/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteCpt(long id)
        {
            
            var Cpt = await _context.Cpt.FindAsync(id);

            if (Cpt == null)
            {
                return NotFound();
            }
            _context.Cpt.Remove(Cpt);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [Route("FindAudit/{CptID}")]
        [HttpGet("{CptID}")]
        public List<CptAudit> FindAudit(long CptID)
        {

            List<CptAudit> data = (from pAudit in _context.CptAudit
                                   where pAudit.CptID == CptID orderby pAudit.AddedDate descending
                                   select new CptAudit()
                                   {
                                       ID = pAudit.ID,
                                       CptID = pAudit.CptID,
                                       TransactionID = pAudit.TransactionID,
                                       ColumnName = pAudit.ColumnName,
                                       CurrentValue = pAudit.CurrentValue,
                                       NewValue = pAudit.NewValue,
                                       CurrentValueID = pAudit.CurrentValueID,
                                       NewValueID = pAudit.NewValueID,
                                       HostName = pAudit.HostName,
                                       AddedBy = pAudit.AddedBy,
                                       AddedDate = pAudit.AddedDate,
                                   }).ToList<CptAudit>();
            return data;
        }


    }
}