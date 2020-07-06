

using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientFormController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public PatientFormController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;


        }

        [HttpPost]
        [Route("SavePatientFormValues")]
        public async Task<ActionResult<PatientFormValue>> SavePatientFormValues(IEnumerable<PatientFormValue> patientForm)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
         User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
         User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            bool succ = TryValidateModel(patientForm);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            foreach (PatientFormValue item in patientForm)
            {
                if (item.ID <= 0)
                {
                    item.AddedBy = UD.Email;
                    item.AddedDate = DateTime.Now;
                      _context.PatientFormValue.Add(item);  
                }
                else
                { 
                    item.UpdatedBy = UD.Email;
                    item.UpdatedDate = DateTime.Now;
                    _context.PatientFormValue.Update(item);
                } 
            }
             
                _context.SaveChanges(); 
            return Ok(patientForm);
        }

 
      
        [Route("PatientFormValues/{formId}")]
        [HttpGet("{formId}")]
        public async Task<IActionResult>  PatientFormValues(long? formId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formsubheading_data = (from pn in _context.PatientNotes
                                       join pf in _context.PatientForms on pn.AppointmentID equals pf.PatientAppointmentID
                                       join pfV in _context.PatientFormValue on pf.ID equals pfV.patientFormID
                                       join fs in _context.FormsSubHeading on pfV.formsSubHeadingID equals fs.ID
                                       where pf.ID == formId &&   ExtensionMethods.IsNull_Bool(pn.Inactive) == false
                                        && ExtensionMethods.IsNull_Bool(pfV.Inactive) == false
                                        && ExtensionMethods.IsNull_Bool(pf.Inactive) == false && ExtensionMethods.IsNull_Bool(fs.Inactive) == false
                                       select new
                                       {
                                           pfV.ID,
                                           pfV.patientFormID,
                                           pfV.patientNotesID,
                                           pfV.clinicalFormsID,
                                           pfV.formsSubHeadingID,
                                           pfV.value,
                                           pfV.Inactive,
                                           pfV.AddedBy,
                                           pfV.AddedDate,
                                           pfV.UpdatedBy,
                                           pfV.UpdatedDate
                                       }).ToList();

            if (formsubheading_data == null)
            {
                return NotFound();
            }

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(formsubheading_data);
        }

        [Route("SavePatientForm")]
        // POST: api/PatientNotes
        [HttpPost]
        public IActionResult SavePatientForm(PatientForms form)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            form.UpdatedBy = UD.Email;
            form.UpdatedDate = DateTime.Now;
            if (form.ID <= 0)
            {
                form.AddedBy = UD.Email;
                form.AddedDate = DateTime.Now;
                //form.PatientID = item.PatientID;
                //form.PatientAppointmentID = item.ID;
                _context.PatientForms.Add(form);
            }
            else
            {
                _context.PatientForms.Update(form);
            }
            _context.SaveChanges();

            return Ok(form);

        }

        [Route("SignPatientForm")]
        // POST: api/PatientNotes
        [HttpPost]
        public IActionResult SignPatientForm(long? patientFormID, bool? sign)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            PatientForms patientForms = _context.PatientForms.Find(patientFormID);
            if (ExtensionMethods.IsNull(patientForms.ID) || patientForms.ID <= 0)
            {
                return Json("Please select Form");
            }
            else
            {
                patientForms.Signed = sign ?? false;
                patientForms.SignedBy = UD.Email;
                patientForms.SignedDate = DateTime.Now;
                patientForms.UpdatedBy = UD.Email;
                patientForms.UpdatedDate = DateTime.Now;
                _context.PatientForms.Update(patientForms);
            }
            _context.SaveChanges();

            return Ok(patientForms);

        }

    }
}