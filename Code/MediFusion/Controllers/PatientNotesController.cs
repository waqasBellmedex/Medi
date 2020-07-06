using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PatientNotesController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public PatientNotesController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        // GET: api/PatientNotes
        /*  [HttpGet]
          public IEnumerable<PatientNotes> GetPatientNotes()
          {
              return _context.PatientNotes;
          }*/

        // GET: api/PatientNotes/5 
        [Route("Notes/{patientId}")]
        [HttpGet("{patientId}")]
        public ActionResult Notes(long? patientId)
        {
          //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
          //  //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientId))
            {
                return BadRequest("Please select Patient.");
            }
            var data = (from pn in _context.PatientNotes
                                join pr in _context.Provider on pn.ProviderID equals pr.ID into table1 from prT in table1.DefaultIfEmpty()
                                join loc in _context.Location on pn.LocationID equals loc.ID into table2  from locT in table2.DefaultIfEmpty()
                                join app in _context.PatientAppointment on pn.AppointmentID equals app.ID into table3  from appT in table3.DefaultIfEmpty()
                                join ar in _context.VisitReason on appT.VisitReasonID equals ar.ID into table4 from arT in table4.DefaultIfEmpty()
                                //join pf in _context.PatientForms on appT.ID equals pf.PatientAppointmentID into table5  from pfT in table5.DefaultIfEmpty()
                                //join cf in _context.ClinicalForms on pfT.ClinicalFormID equals cf.ID into table6  from cfT in table6.DefaultIfEmpty()
                                where pn.PatientID == patientId && ExtensionMethods.IsNull_Bool(pn.Inactive) == false
                                 
                        select new
                                {
                                    dos = pn.DOS.ToString("MM/dd/yyyy"),
                                    pn.Signed,
                                    pn.SignedBy,
                                    SignedDate= (pn.Signed!=null && pn.Signed.Value)?pn.SignedDate.Value.ToString("MM/dd/yyyy hh:mm tt"):"",
                                    provider = prT!=null ?(prT.LastName + ", " + prT.FirstName):"",
                                    location = locT != null ? locT.Name :"",
                                    patientNotesID = pn.ID,
                                    visitReason = arT != null ? arT.Name:"", 
                                    PatientAppointmentID = pn.AppointmentID //, // arT != null ? pfT.PatientAppointmentID : 0,
                                    //PatientFormID =   pfT==null?0: pfT.ID ,
                                    // form = pfT != null ? (pfT.PatientAppointmentID == null ? "" : cfT.Name):"", 
                                }
                                ) //GroupBy(e => e.PatientAppointmentID)
                                .ToList();
            var patientNotes = data.Select(
                                    e =>
                                      new
                                      {
                                          dos = e.dos,
                                          patientNotesID = e.patientNotesID,
                                          provider = e.provider,
                                          Signed = e.Signed,
                                          SignedBy = e.SignedBy,
                                          SignedDate = e.SignedDate,
                                          location = e.location,
                                          visitReason = e.visitReason, 
                                          form =( e.PatientAppointmentID!=null && e.PatientAppointmentID>0) ? GetChildren(e.PatientAppointmentID):null


                                      }
                                    );


            if (patientNotes == null)
            {
                return NotFound();
            }
            var lst = patientNotes.ToList();
            return Json(lst);
        }
        public List<VMPatientForms> GetChildren(long? PatientAppointmentID)
        {
            List<VMPatientForms> lst = (from pf in _context.PatientForms
                                       join cf in _context.ClinicalForms on pf.ClinicalFormID equals cf.ID
                                       where pf.PatientAppointmentID == PatientAppointmentID
                                       select new VMPatientForms()
                                       {
                                           ID = pf.ID,
                                           PatientAppointmentID= pf.PatientAppointmentID, 
                                           PatientID = pf.PatientID,
                                           PracticeID = pf.PracticeID,
                                           reportHeader = pf.reportHeader,
                                           clinicalFormID = pf.ClinicalFormID, 
                                           Name = cf.Name,
                                           Description = cf.Description,
                                           Type = cf.Type,
                                           Signed = pf.Signed,
                                           SignedBy = pf.SignedBy,
                                           SignatureUrl = pf.SignatureUrl,
                                           SignedDate = pf.SignedDate,
                                           CoSigned = pf.CoSigned,
                                           CoSignedBy = pf.CoSignedBy,
                                           CoSignatureUrl = pf.CoSignatureUrl,
                                           CoSignedDate = pf.CoSignedDate 
                                       })
                       .ToList();

            return lst;

        }

        // Patient Vitals..
        [Route("PatientVitals")]
        [HttpGet]
        public ActionResult PatientVitals(long? patientId, long? patientNotesId)
        {
          //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientId))
            {
                return BadRequest("Please select Patient.");
            }

            var patientVitals = (from pv in _context.PatientVitals
                                 join pn in _context.PatientNotes on pv.PatientNotesId equals pn.ID
                                // join pa in _context.PatientAppointment on pn.AppointmentID equals pa.ID
                                 where pn.PatientID == patientId
                                 select new
                                 {  
                                     pv.ID,
                                     dos=pn.DOS.Format("MM/dd/yyyy"),
                                     date = pn.DOS ,
                                     pv.PatientNotesId,
                                     AppointmentID= pn.AppointmentID,
                                     pv.Height_foot,
                                     pv.Height_cm,
                                     pv.Weight_lbs,
                                     pv.Weight_pounds,
                                     pv.BMI,
                                     pv.BPSystolic,
                                     pv.BPDiastolic,
                                     pv.Temperature,
                                     pv.Pulse,
                                     pv.Respiratory_rate,
                                     pv.OxygenSaturation,
                                     pv.Pain,
                                     pv.HeadCircumference,
                                     pv.PracticeID,
                                     pv.Inactive,
                                     pv.AddedBy,
                                     pv.AddedDate,
                                     pv.UpdatedBy,
                                     pv.UpdatedDate

                                 }).OrderByDescending(o=>o.date).ToList();

            if (patientNotesId != null && patientNotesId > 0 && patientVitals.Count>0)
            { 
              var  patientVital = patientVitals.Where(w => w.PatientNotesId == patientNotesId).FirstOrDefault();
                return Ok(patientVital);
            }
            //else
            return Ok( patientVitals);
        }

        [Route("SavePatientVitals")]
        [HttpPost]
        public   ActionResult SavePatientVitals(PatientVitals item)
        {

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
            if (ExtensionMethods.IsNull(item.PatientNotesId))
            {
                PatientNotes _patientNotes = new PatientNotes();
                _patientNotes.AppointmentID = item.appointmentID;
                PatientAppointment appT = _context.PatientAppointment.Find(item.appointmentID);
                if(appT!=null && appT.ID>0)
                {
                    _patientNotes.DOS = appT.AppointmentDate.Value;
                    _patientNotes.PatientID = appT.PatientID ;
                    _patientNotes.ProviderID = appT.ProviderID.Value;
                    _patientNotes.LocationID = appT.LocationID.Value; 
                } 
                SavePatientNotes(_patientNotes).GetAwaiter().GetResult();
                item.PatientNotesId = _patientNotes.ID;
               // return Json("Please select DOS");
            }
            if (item.ID <= 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.PracticeID = PracticeId;
                item.Inactive = false;
                _context.PatientVitals.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now; 
                _context.PatientVitals.Update(item); 
            }
            _context.SaveChanges();
            
            return Ok(item);
        }
        [Route("GetPatientAllergy")]
        [HttpGet]
        public ActionResult GetPatientAllergy (long? patientId, long? patientNotesId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientId))
            {
                return BadRequest("Please select Patient.");
            }

            var patientAllergy = (from pv in _context.PatientAllergy
                                 join pn in _context.PatientNotes on pv.PatientNotesId equals pn.ID into table1
                                 from pnT in table1.DefaultIfEmpty()
                                 where pnT.PatientID == patientId
                                 select new
                                 {
                                     pv.ID,
                                     pv.PatientNotesId,
                                     pv.AllergyType,
                                     pv.SpecificDrugAllergy,
                                     pv.Reaction,
                                     pv.Severity,
                                     pv.Status,
                                     pv.PracticeID,
                                     pv.Inactive,
                                     pv.AddedBy,
                                     pv.AddedDate,
                                     pv.UpdatedBy,
                                     pv.UpdatedDate,
                                     dos=pnT.DOS.Format("MM/dd/yyyy"),

                                 }).ToList();

            if (patientNotesId != null && patientNotesId > 0 && patientAllergy.Count > 0)
            {
                var patientAllergy1 = patientAllergy.Where(w => w.PatientNotesId == patientNotesId).FirstOrDefault();
                return Ok(patientAllergy1);
            }
            else
                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(patientAllergy);
        }



        [Route("SavePatientAllergy")]
        [HttpPost]
        public ActionResult savePatientAllergy(PatientAllergy item)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (ExtensionMethods.IsNull(item.PatientNotesId))
            {
                return Json("Please select DOS");
            }
            if (item.ID <= 0)
            {
                item.AddedBy = UD.Email;
                item.AddedDate = DateTime.Now;
                item.PracticeID = UD.PracticeID;
                item.Inactive = false;
                _context.PatientAllergy.Add(item);

            }
            else
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.PatientAllergy.Update(item);
            }
            _context.SaveChanges();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }
        [Route("PatientMedicalNotes/{patientNotesId}")]
        [HttpGet("{patientNotesId}")]
        public ActionResult PatientMedicalNotes(long? patientNotesId)
        {
          //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientNotesId))
            {
                return BadRequest("Please select Visit.");
            }

            var patientMedicalNotes = (from pMN in _context.PatientMedicalNotes
                                 join pn in _context.PatientNotes on pMN.PatientNotesId equals pn.ID into table1
                                 from pnT in table1.DefaultIfEmpty()
                                 where pMN.PatientNotesId == patientNotesId
                                 select new
                                 {
                                     ID = pMN.ID,
                                     PatientNotesId = pMN.PatientNotesId,
                                     note = pMN.note,
                                     note_html = pMN.note_html,
                                     Inactive = pMN.Inactive,
                                     AddedBy = pMN.AddedBy,
                                     AddedDate = pMN.AddedDate,
                                     UpdatedBy = pMN.UpdatedBy,
                                     UpdatedDate = pMN.UpdatedDate
                                 }).FirstOrDefault();


            if (patientMedicalNotes == null)
            {
                return NotFound();
            }

            return Ok(patientMedicalNotes);
        }
        [Route("PatientlNotesHeader/{patientNotesId}")]
        [HttpGet("{patientNotesId}")]
        public ActionResult PatientlNotesHeader(long? patientNotesId)
        {
          //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientNotesId))
            {
                return BadRequest("Please select Visit.");
            }

            var patientMedicalNotes = (from pn in _context.PatientNotes
                                       join pat in _context.Patient on pn.PatientID equals pat.ID
                                       join p in _context.Practice on pat.PracticeID equals p.ID
                                       join pr in _context.Provider on pn.ProviderID equals pr.ID 
                                       join lc in _context.Location on pn.LocationID equals lc.ID into table1
                                       from lcT in table1.DefaultIfEmpty()
                                       join refp in _context.RefProvider on pat.LocationId equals refp.ID into table2
                                       from refpT in table1.DefaultIfEmpty() 
                                       where pn.ID == patientNotesId
                                       select new
                                       {
                                           PatientAccountNum = pat.AccountNum,
                                           patientGender = pat.Gender,
                                           patientAge = pat.DOB != null ? (DateTime.Now.Year - pat.DOB.Value.Year).ToString() : "",
                                           patientName = pat.LastName + ", " + pat.FirstName,
                                           providerName = pr.LastName + ", " + pr.FirstName,
                                           refProviderName = refpT!=null? refpT.Name:"" ,
                                           locationName = lcT!=null? lcT.Name:"",
                                           practiceName = p.Name,
                                           practiceEmail = p.Email,
                                           practiceAddress = p.Address1+" "+p.City+" "+p.State+" "+p.ZipCode,
                                           patientID = pat.ID,
                                           PatientNotesId = pn.ID 
                                       }).FirstOrDefault();


            if (patientMedicalNotes == null)
            {
                return NotFound();
            }

            return Ok(patientMedicalNotes);
        }
        [Route("SavePatientMedicalNotes")]
        [HttpPost]
        public async Task<ActionResult<PatientAppointment>> SavePatientMedicalNotes(PatientMedicalNotes item)
        {

              string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (ExtensionMethods.IsNull(item.PatientNotesId))
            {
                return Json("Please select DOS");
            }
            if (item.ID <= 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context.PatientMedicalNotes.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PatientMedicalNotes.Update(item);
            }
            _context.SaveChanges();
           
            return Ok(item);
        }
        // PUT: api/PatientNotes/5
        [Route("SavePatientNotes")]
        [HttpPost]
        public async Task<IActionResult> SavePatientNotes( PatientNotes patientNotes)
        {
       
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(patientNotes);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            } 
 
            if (patientNotes.ID <= 0)
            {
                patientNotes.AddedBy = Email;
                patientNotes.AddedDate = DateTime.Now;
                _context.PatientNotes.Add(patientNotes);

            }
            else
            {
                patientNotes.UpdatedBy = Email;
                patientNotes.UpdatedDate = DateTime.Now;
                _context.PatientNotes.Update(patientNotes);
            }
            _context.SaveChanges();
            return Ok(patientNotes);
          
        }
        [Route("SignPatientNotes")]
        // POST: api/PatientNotes
        [HttpPost]
        public   IActionResult SignPatientNotes(long? patientNotesId,bool? sign)
        {
           
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            PatientNotes patientNotes = _context.PatientNotes.Find(patientNotesId);
            if (ExtensionMethods.IsNull(patientNotes.ID) || patientNotes.ID <= 0)
            { 
                    return Json("Please select DOS"); 
            }
            else
            {
                patientNotes.Signed = sign??false;
                patientNotes.SignedBy = Email;
                patientNotes.SignedDate= DateTime.Now;
                patientNotes.UpdatedBy = Email;
                patientNotes.UpdatedDate = DateTime.Now;
                _context.PatientNotes.Update(patientNotes);
            }
            _context.SaveChanges();

            return Ok(patientNotes);

        }
        // POST: api/PatientNotes
        [HttpPost]
        public async Task<IActionResult> PostPatientNotes([FromBody] PatientNotes patientNotes)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PatientNotes.Add(patientNotes);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPatientNotes", new { id = patientNotes.ID }, patientNotes);
        }

        // DELETE: api/PatientNotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatientNotes([FromRoute] long id)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientNotes = await _context.PatientNotes.FindAsync(id);
            if (patientNotes == null)
            {
                return NotFound();
            }

            _context.PatientNotes.Remove(patientNotes);
            await _context.SaveChangesAsync();
            return Ok(patientNotes);
        }

        private bool PatientNotesExists(long id)
        {
            return _context.PatientNotes.Any(e => e.ID == id);
        }



        // to notes amendments..
        [Route("SaveAmendments")]
        [HttpPost]
        public ActionResult SaveAmendments(Amendments Amendment)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
         User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
         User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null)
            {
                return Json("User not found.");
            }



            if (Amendment.AttachmentData != "" && Amendment.AttachmentData.Trim().Length>0)
            { 
                var attachmentPath = string.Empty;       
                Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                settings.DocumentServerDirectory, UD.PracticeID.ToString(), "PatientNotes");

                var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                string attachement = "";
                attachmentPath = Amendment.FileName + "_" + datetime + Amendment.FileType;
                attachement = System.IO.Path.Combine(DirectoryPath, attachmentPath);



                string base64String = Amendment.AttachmentData.Split(',').Last();
                byte[] bytes = Convert.FromBase64String(base64String);





                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                System.IO.File.WriteAllBytes(attachement, bytes);
                Amendment.Attachment = attachmentPath;

            }

            if (Amendment.ID <= 0)
            {
                Amendment.AddedBy = UD.Email;
                Amendment.AddedDate = DateTime.Now;
                _context.Amendments.Add(Amendment);
            }

            else
            {
                Amendment.UpdatedBy = UD.Email;
                Amendment.UpdatedDate = DateTime.Now;
                _context.Amendments.Update(Amendment); 
            }
            _context.SaveChanges();
            return Json(Amendment);
        }

        // To view  Patient Amendments..
        [Route("Amendments/{Notesid}")]
        [HttpGet("{Notesid}")]
        public ActionResult Amendments(long? Notesid)
        {

            if (ExtensionMethods.IsNull(Notesid))
            {
                return BadRequest("Please select DOS .");
            }

            var Amendments = (from pan in _context.Amendments
                                  //join pn in _context.PatientNotes on pv.PatientNotesId equals pn.ID
                                  // join pa in _context.PatientAppointment on pn.AppointmentID equals pa.ID
                              where pan.PatientNotesId == Notesid
                              select new
                              {
                                  pan.ID,
                                  pan.Attachment,
                                  pan.FileType,
                                  pan.FileName,
                                  pan.Notes,
                                  AmendmentDate = pan.AmendmentDate.Format("MM/dd/yyyy"),
                                  pan.Status,
                                  pan.PatientNotesId,
                                  pan.Inactive,
                                  pan.AddedBy,
                                  AddedDate = pan.AddedDate.Format("MM/dd/yyyy"),
                                  pan.UpdatedBy,
                                  UpdatedDate = pan.UpdatedDate.Format("MM/dd/yyyy"),

                              }).OrderByDescending(o => o.AmendmentDate).ToList();

            
            
            return Ok(Amendments);
        }




        [Route("DownloadAttachement/{ID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadAttachement(long ID)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
         User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
         User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null)
            {
                return Json("User not found.");
            }
            Amendments AmendmentModel = _context.Amendments.Find(ID);
            if (AmendmentModel != null)
            {
                string Document = AmendmentModel.Attachment;
                Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                settings.DocumentServerDirectory, UD.PracticeID.ToString(), "PatientNotes");



                string filepath = (DirectoryPath + "\\" + Document);
                if (!System.IO.File.Exists(filepath))
                {
                    return NotFound();
                }
                var formText = System.IO.File.ReadAllBytes(filepath);
                if (formText == null)
                    return NotFound();                
                return Ok(formText);
            }
            return NotFound();
        }

        [Route("SaveFamilyHistory")]
        [HttpPost]
        public ActionResult SaveFamilyHistory(PatientFamilyHistory item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
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

            if (item.ID <= 0)
            {
                item.PracticeID = UD.PracticeID;
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.PracticeID = PracticeId;
                item.inActive = false;
                _context.PatientFamilyHistory.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PatientFamilyHistory.Update(item);
            }
            _context.SaveChanges();

            return Ok(item);
        }

        [Route("GetFamilyHistory")]
        [HttpGet]
        public ActionResult GetFamilyHistory(long? patientId, long? patientNotesId)
        {
            //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientId))
            {
                return BadRequest("Please select Patient.");
            }

            var patientFamilyHistory = (from pfh in _context.PatientFamilyHistory
                                        join pn in _context.PatientNotes on pfh.patientNotesID equals pn.ID
                                        join icd in _context.ICD on pfh.ICDID equals icd.ID into Table2
                                        from icdt in Table2.DefaultIfEmpty()
                                            // join pa in _context.PatientAppointment on pn.AppointmentID equals pa.ID
                                        where pn.PatientID == patientId && ExtensionMethods.IsNull_Bool(pfh.inActive) == false
                                        select new
                                        {
                                            pfh.ID,
                                            dos = pn.DOS.Format("MM/dd/yyyy"),
                                            date = pn.DOS,
                                            pfh.patientNotesID,
                                            pfh.PracticeID,
                                            pfh.ICDID,
                                            pfh.dosage,
                                            pfh.inActive,
                                            pfh.desc,
                                            pfh.relationship,
                                            pfh.type,
                                            icdt.ICDCode,
                                            pfh.status,
                                            pfh.drugName,
                                            pfh.startDate,
                                            pfh.endDate,
                                            pfh.AddedBy,
                                            pfh.AddedDate,
                                            pfh.UpdatedBy,

                                            pfh.UpdatedDate

                                        }).ToList();

            if (patientNotesId != null && patientNotesId > 0 && patientFamilyHistory.Count() > 0)
            {
                var patientVital = patientFamilyHistory.Where(w => w.patientNotesID == patientNotesId).ToList();
                return Ok(patientVital);
            }
            //else
            return Ok(patientFamilyHistory);
        }







        [Route("SaveProblemList")]
        [HttpPost]
        public ActionResult SaveProblemList(ProblemList item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
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

            if (item.ID <= 0)
            {
                item.PracticeID = UD.PracticeID;
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.PracticeID = PracticeId;
                item.inActive = false;
                _context.ProblemList.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.ProblemList.Update(item);
            }
            _context.SaveChanges();

            return Ok(item);
        }



        [Route("GetproblemList")]
        [HttpGet]
        public ActionResult GetproblemList(long? patientId, long? patientNotesId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            if (ExtensionMethods.IsNull(patientId))
            {
                return BadRequest("Please select Patient.");
            }

            var problemlist = (from pl in _context.ProblemList
                               join pn in _context.PatientNotes on pl.patientNotesID equals pn.ID
                               join icd in _context.ICD on pl.ICDID equals icd.ID into Table2
                               from icdt in Table2.DefaultIfEmpty()
                                   // join pa in _context.PatientAppointment on pn.AppointmentID equals pa.ID
                               where pn.PatientID == patientId && ExtensionMethods.IsNull_Bool(pl.inActive) == false
                               select new
                               {
                                   pl.ID,
                                   dos = pn.DOS.Format("MM/dd/yyyy"),
                                   date = pn.DOS,
                                   pl.patientNotesID,
                                   pl.PracticeID,
                                   pl.ICDID,
                                   pl.inActive,
                                   pl.desc,

                                   pl.type,
                                   pl.status,
                                   icdt.ICDCode,

                                   pl.strartdate,
                                   pl.endDate,
                                   pl.AddedBy,
                                   pl.AddedDate,
                                   pl.UpdatedBy,

                                   pl.UpdatedDate

                               }).OrderByDescending(o => o.date).ToList();

            if (patientNotesId != null && patientNotesId > 0 && problemlist.Count > 0)
            {
                var patientVital = problemlist.Where(w => w.patientNotesID == patientNotesId).ToList();
                return Ok(patientVital);
            }
            //else
            return Ok(problemlist);
        }



    }
}