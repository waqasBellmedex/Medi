using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientAlertsController : Controller 
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public PatientAlertsController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now; 
        }
        [Route("PatientAlerts")]
        [HttpGet]
        public ActionResult PatientAlerts(long? patientId,bool? resolved)
        {
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            //if (ExtensionMethods.IsNull(patientId))
            //{
            //    return BadRequest("Please select Patient.");
            //}
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var QUERY = (from pA in _context.PatientAlerts
                         join gi in _context.GeneralItems on pA.type equals gi.Value into giTable
                         from giT in giTable.DefaultIfEmpty()
                         join pat in _context.Patient on pA.patientID equals pat.ID
                         where pA.practiceId==PracticeId && ExtensionMethods.IsNull_Bool(pA.inactive)!=true

                                 select new
                                 {
                                     pA.ID,
                                     pA.patientID,
                                     patientName=pat.LastName+", "+pat.FirstName,
                                     type= giT != null ? giT.Name : "",
                                     alertDate= pA.date!=null?pA.date.Value.ToString("MM/dd/yyyy"):"",
                                     date = pA.date != null ? pA.date.Value : (DateTime?)null,
                                     pA.assignedTo,
                                     pA.notes,
                                     pA.practiceId,
                                     pA.inactive,
                                     pA.AddedDate,
                                     pA.AddedBy,
                                     pA.UpdatedDate,
                                     pA.UpdatedBy,
                                     pA.resolved,
                                     pA.resolvedBy,
                                     pA.resolvedDate,
                                     pA.resolveComments

                                 });
            if(patientId>0)
            {
                QUERY = QUERY.Where(w => w.patientID == patientId);
            }
            if (resolved!=null && resolved.Value)
            {
                QUERY = QUERY.Where(w => w.resolved == resolved.Value);
            }
            else
            {
                QUERY = QUERY.Where(w => ExtensionMethods.IsNull_Bool(w.resolved) != true);
            }
            var patientAlerts = QUERY.ToList();
           
            return Ok(patientAlerts);
        }

        [Route("SavePatientAlerts")]
        [HttpPost]
        public ActionResult SavePatientAlerts(PatientAlerts item)
        {

            // UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (ExtensionMethods.IsNull(item.patientID))
            {
                return Json("Please select Patient");
            }
            if (item.ID <= 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.practiceId = PracticeId;
                item.inactive = false;
                _context.PatientAlerts.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PatientAlerts.Update(item);
            }
            _context.SaveChanges();
            return Ok(item);
        }
        [Route("ResolvePatientAlerts")]
        [HttpPost]
        public ActionResult ResolvePatientAlerts(PatientAlerts patientAlert)
        {

             string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            if (patientAlert == null || patientAlert.ID <= 0)
            {
                return Json("Please select valid alert.");
            }
            PatientAlerts patientAlerts = _context.PatientAlerts.Find(patientAlert.ID);
            if (patientAlerts==null)
            {
                return Json("Alert not found.");
            }

            patientAlerts.UpdatedBy = Email;
            patientAlerts.UpdatedDate = DateTime.Now;
            patientAlerts.resolved =true;
            patientAlerts.resolvedDate = patientAlert.resolvedDate == null? DateTime.Now: patientAlert.resolvedDate;
            patientAlerts.resolvedBy = Email;
            patientAlerts.resolveComments = patientAlert.resolveComments;

            _context.PatientAlerts.Update(patientAlerts);
            
            _context.SaveChanges();
            return Ok(patientAlerts);
        }
    }
}