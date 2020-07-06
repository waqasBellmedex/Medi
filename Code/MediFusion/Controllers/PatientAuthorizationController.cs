using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMBatchDocument;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;
using static MediFusionPM.ViewModels.VmPatientAuthorization;

namespace MediFusionPM.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize]
        public class PatientAuthorizationController : Controller
        {
            private readonly ClientDbContext _context;
            private readonly MainContext _contextMain;
            private DateTime _startTime = DateTime.MinValue;

        public PatientAuthorizationController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;


        }



        [Route("GetAuthorizationNumber")]
        [HttpPost]
        public string GetAuthorizationNumber(CAuthorizationNumber model)
        {
            var PatAuthList = (from pAuth in _context.PatientAuthorization
                               join cpt in _context.Cpt on pAuth.CPTID equals cpt.ID
                               join pro in _context.Provider on pAuth.ProviderID equals pro.ID
                               join ip in _context.InsurancePlan on pAuth.InsurancePlanID equals ip.ID
                               join pp in _context.PatientPlan on ip.ID equals pp.InsurancePlanID
                               where (pp.ID == model.PatientPlanId)
                               && pAuth.CPTID == model.CptId
                               && pAuth.ProviderID == model.ProviderId
                               && pAuth.PatientID == model.PatientId
                                && model.DateOfServiceFrom.Date() >= pAuth.StartDate.Date()
                               && model.DateOfServiceFrom.Date() <= pAuth.EndDate.Date()
                               && model.DateOfServiceTo.Date() >= pAuth.StartDate.Date()
                               && model.DateOfServiceTo.Date() <= pAuth.EndDate.Date()
                               select pAuth).FirstOrDefault();
            if (PatAuthList == null)
            {
                return "";
            }
            return PatAuthList.AuthorizationNumber;
        }
        [HttpGet]
        [Route("FindExpiringAuthorizations")]
        public async Task<ActionResult<IEnumerable<GExpiringAuthorizations>>> FindExpiringAuthorizations()
        {
            return await (from pAuth in _context.PatientAuthorization
                          join patient in _context.Patient on pAuth.PatientID equals patient.ID
                          join cpt in _context.Cpt on pAuth.CPTID equals cpt.ID
                          join pro in _context.Provider on pAuth.ProviderID equals pro.ID
                          where
                          (pAuth.EndDate.HasValue ? pAuth.EndDate.Value.Subtract(DateTime.Now).Days <= pAuth.RemindBeforeDays : false)
                            &&
                          (pAuth.EndDate.HasValue ? pAuth.EndDate.Value.Subtract(DateTime.Now).Days >= 0 : false)
                          && (((pAuth.VisitsAllowed.ValZero() - pAuth.VisitsUsed.ValZero()) <= pAuth.RemindBeforeRemainingVisits))
                          && ((pAuth.VisitsAllowed.ValZero() - pAuth.VisitsUsed.ValZero()) > 0)
                          select new GExpiringAuthorizations()
                          {
                              PatiendID = patient.ID,
                              PatientAuthId = pAuth.ID,
                              AccountNo = patient.AccountNum,
                              AuthorizationNo = pAuth.AuthorizationNumber,
                              VisitsAllowed = pAuth.VisitsAllowed.ValZero().ToString(),
                              VisitsUsed = pAuth.VisitsUsed.ValZero().ToString(),
                              VisitsRemaining = (pAuth.VisitsAllowed.ValZero() - pAuth.VisitsUsed.ValZero()).ToString(),
                              StartDate = pAuth.StartDate.Format("MM/dd/yyyy"),
                              ExpiryDate = pAuth.EndDate.Format("MM/dd/yyyy")

                          }).ToListAsync();
        }



        //[HttpPost]
        //[Route("FindPatientAuthorizations")]
        //public async Task<ActionResult<IEnumerable<GPatientAuthorization>>> FindPatientAuthorizations(CPatientAuthorization model)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

        //Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
        //return await (from pAuth in _context.PatientAuthorization
        //                  join patient in _context.Patient on pAuth.PatientID equals patient.ID
        //                  join cpt in _context.Cpt on pAuth.CPTID equals cpt.ID
        //                  join pro in _context.Provider on pAuth.ProviderID equals pro.ID
        //                  where
        //                  (model.AccountNo.IsNull() ? true : patient.AccountNum == model.AccountNo)
        //                  && model.AuthorizationNo.IsNull() ? true : pAuth.AuthorizationNumber == model.AuthorizationNo
        //                  && model.CPTCode.IsNull() ? true : cpt.CPTCode == model.CPTCode
        //                  && (ExtensionMethods.IsBetweenDOS(model.ExpiryDate, model.StartDate, pAuth.EndDate, pAuth.StartDate))
        //                  select new GPatientAuthorization()
        //                  {
        //                      AccountNo = patient.AccountNum,
        //                      PatientName = patient.LastName.Trim() + ", " + patient.FirstName.Trim(),
        //                      AuthorizationNo = pAuth.AuthorizationNumber,
        //                      CPTCode = cpt.CPTCode,
        //                      StartDate = pAuth.StartDate.Format("MM/dd/yyyy"),
        //                      ExpiryDate = pAuth.EndDate.Format("MM/dd/yyyy")
        //                  }).ToListAsync();
        //}


        [HttpPost]
        [Route("FindPatientAuthorizations")]
        public async Task<ActionResult<IEnumerable<GPatientAuthorization>>> FindPatientAuthorizations(CPatientAuthorization model)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPatientAuthorizations(model, PracticeId);

        }

        private List<GPatientAuthorization> FindPatientAuthorizations(CPatientAuthorization model, long PracticeId)
        {
      
            List<GPatientAuthorization> data = (from pAuth in _context.PatientAuthorization
                                                join patient in _context.Patient on pAuth.PatientID equals patient.ID
                                                //join f in _context.Practice on p.PracticeID equals f.ID into Table1 from t1 in Table1.DefaultIfEmpty()
                                                join cpt in _context.Cpt on pAuth.CPTID equals cpt.ID into Table1
                                                from cptLeft in Table1.DefaultIfEmpty()
                                                join pro in _context.Provider on pAuth.ProviderID equals pro.ID
                                                where
                                                   (model.ProviderID.IsNull() ? true : pro.ID.Equals(model.ProviderID))
                                                && (model.LocationID.IsNull() ? true : patient.LocationId.Equals(model.LocationID))
                                                && (model.Status.IsNull() ? true : pAuth.Status.IsNull() ? false : pAuth.Status.Equals(model.Status))
                                                && (ExtensionMethods.IsBetweenDOS(model.EntryDateTo, model.EntryDateFrom, pAuth.AddedDate, pAuth.AddedDate))
                                                && (ExtensionMethods.IsBetweenDOS(model.AuthorizationDateTo, model.AuthorizationDateFrom, pAuth.AuthorizationDate, pAuth.AuthorizationDate))
                                                && (model.AccountNo.IsNull() ? true : patient.AccountNum == model.AccountNo)
                                                && (model.AuthorizationNo.IsNull() ? true : pAuth.AuthorizationNumber == model.AuthorizationNo)
                                                && (model.CPTCode.IsNull() ? true : cptLeft.CPTCode.IsNull() ? false : cptLeft.CPTCode.Equals(model.CPTCode))
                                                && (ExtensionMethods.IsBetweenDOS(model.ExpiryDate, model.StartDate, pAuth.EndDate, pAuth.StartDate))
                                                &&(model.ResponsibleParty.Equals("BELLMEDEX") ? pAuth.ResponsibleParty.IsNull() ? false : pAuth.ResponsibleParty.Equals("BELLMEDEX") : model.ResponsibleParty.Equals("CLIENT") ? pAuth.ResponsibleParty.IsNull() ? false : pAuth.ResponsibleParty.Equals("CLIENT") :true)
                                                select new GPatientAuthorization()
                                                {
                                                    EntryDate = pAuth.AddedDate.Format("MM/dd/yyyy"),
                                                    AuthorizationDate = pAuth.AuthorizationDate.Format("MM/dd/yyyy"),
                                                    Status = pAuth.Status,
                                                    AccountNo = patient.AccountNum,
                                                    PatientName = patient.LastName.Trim() + ", " + patient.FirstName.Trim(),
                                                    //Plan = ,
                                                    ProviderID = pro.ID,
                                                    ProviderName = pro.LastName + ", " + pro.FirstName,
                                                    AuthorizationNo = pAuth.AuthorizationNumber,
                                                    CPTCode = cptLeft.CPTCode,
                                                    StartDate = pAuth.StartDate.Format("MM/dd/yyyy"),
                                                    ExpiryDate = pAuth.EndDate.Format("MM/dd/yyyy"),
                                                    VisitsAllowed = pAuth.VisitsAllowed,
                                                    VisitsUsed = pAuth.VisitsUsed,
                                                    PatientID = patient.ID,
                                                    ResponsibleParty = pAuth.ResponsibleParty
                                                }).ToList();

            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPatientAuthorization model)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPatientAuthorization> data = FindPatientAuthorizations(model, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, model, "Patient Authorization Report");
        }
        
        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPatientAuthorization model)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPatientAuthorization> data = FindPatientAuthorizations(model, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
    }
}