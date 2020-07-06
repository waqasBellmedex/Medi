using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.IO;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicalFormsController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public ClinicalFormsController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;


        }

        // GET: api/ClinicalForms
        [HttpGet]
        public IEnumerable<ClinicalForms> GetClinicalForms()
        {
            return _context.ClinicalForms;

        }

        // GET: api/ClinicalForms/5
        //HttpGet("{id}")]
        [HttpGet]
        [Route("GetClinicalForms")]
        public async Task<IActionResult> GetClinicalForms(long id)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clinicalForms = await _context.ClinicalForms.FindAsync(id);

            if (clinicalForms == null)
            {
                return NotFound();
            }

           
            return Ok(clinicalForms);
        }

        // GET: api/ClinicalFormCPT/
        // [HttpGet("{id}")]
        [HttpGet]
        [Route("GetClinicalFormCPT")]
        public async Task<IActionResult> GetClinicalFormCPT(long ClinicalFormID)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clinicalFormCPT = _context.ClinicalFormsCPT
                                      .Where(s => s.ClinicalFormID == ClinicalFormID)
                                      .ToList();
            //var clinicalFormCPT = await _context.ClinicalFormCPT.FindAsync(ClinicalFormID);

            if (clinicalFormCPT == null)
            {
                return NotFound();
            }
           
            return Ok(clinicalFormCPT);
        }
        // POST: api/ClinicalFormsCPT
        [HttpPost]
        [Route("SaveClinicalFormsCPT")]
        public async Task<ActionResult<ClinicalFormsCPT>> SaveClinicalFormsCPT(ClinicalFormsCPT clinicalFormsCPT)
        {
        UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.PatientCreate == false)
                return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            bool succ = TryValidateModel(clinicalFormsCPT);

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
          
            //else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            await _context.SaveChangesAsync();

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(clinicalFormsCPT);

        }

        // PUT: api/ClinicalForms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClinicalForms([FromRoute] long id,  ClinicalForms clinicalForms)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clinicalForms.ID)
            {
                return BadRequest();
            }

            _context.Entry(clinicalForms).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicalFormsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet]
        [Route("SearchClinicalForms")]
        public ActionResult SearchClinicalForms(string name)
        {
            if (name == null || name.Length == 0)
            {
                return Json("");
            }
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.LocationSearch == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            var data = (from cf in _context.ClinicalForms
                        join prac in _context.Practice on cf.PracticeID equals prac.ID
                        //join up in _context.UserPractices on prac.ID equals up.PracticeID
                        //join u in _context.Users on up.UserID equals u.Id

                        where prac.ID == PracticeId  //&& u.Id.ToString() == UD.UserID &&
                       && ExtensionMethods.IsNull_Bool(cf.Inactive) == false && cf.Name.ToLower().Contains(name.ToLower().Trim())                               //u.IsUserBlock == false && 
                        select new
                        {
                            cf.ID,
                            cf.Name,
                            cf.Description,
                            cf.Type
                        }).ToList();
            return Json(data);
        }
        [HttpGet]
        [Route("AllClinicalForms")]
        public ActionResult AllClinicalForms()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            
            //if (UD == null || UD.Rights == null || UD.Rights.LocationSearch == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            List<ClinicalForms> data = (from cf in _context.ClinicalForms
                        join prac in _context.Practice on cf.PracticeID equals prac.ID
                                        //join cfCpts in _context.ClinicalFormsCPT on cf.ID equals cfCpts.ClinicalFormID into Table2
                                        //                   from cfCptsT in Table2.DefaultIfEmpty()
                                        //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                        //join u in _context.Users on up.UserID equals u.Id
                                         
                                        join p in _context.Provider on cf.ProviderID equals p.ID into ps
                                        from pt in ps.DefaultIfEmpty()
                                        where prac.ID == UD.PracticeID && ExtensionMethods.IsNull_Bool(cf.Inactive) == false 
                                       
                                        select new ClinicalForms()
                        {
                            ID=  cf.ID ,
                            Name = cf.Name,
                            Description = cf.Description,
                            Type = cf.Type,
                            ProviderID = cf.ProviderID,
                            PracticeID = cf.PracticeID,
                            Inactive = cf.Inactive,
                            AddedBy = cf.AddedBy,
                            AddedDate = cf.AddedDate,
                            UpdatedBy = cf.UpdatedBy,
                            UpdatedDate = cf.UpdatedDate,
                                            url = cf.url,
                                            ProviderName = pt!=null? pt.Name:"",
                                            // ,CPTs= GetChildren(cf.ID)
                                        }).Include(a => a.CPTs).ToList();
            List<ClinicalForms> hierarchy = data.Select(
                cf => new ClinicalForms()
                {
                    ID = cf.ID,
                    Name = cf.Name,
                    Description = cf.Description,
                    Type = cf.Type,
                    ProviderID = cf.ProviderID,
                    PracticeID = cf.PracticeID,
                    Inactive = cf.Inactive,
                    AddedBy = cf.AddedBy,
                    AddedDate = cf.AddedDate,
                    UpdatedBy = cf.UpdatedBy,
                    UpdatedDate = cf.UpdatedDate,
                    CPTs = GetChildren(cf.ID) ,
                    url =   cf.url ,
                    ProviderName = cf.ProviderName 
                }
                ).ToList();
            //var lst = new {data= data, };
            return Json(hierarchy);
        }
        public List<ClinicalFormsCPT> GetChildren(  long parentId)
        {
            List<ClinicalFormsCPT> lst = (from c in _context.ClinicalFormsCPT
                       join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                          where c.ClinicalFormID==parentId && c.Inactive != true
                       select new ClinicalFormsCPT()
                       {
                           ID = c.ID,
                           ClinicalFormID = c.ClinicalFormID,
                           Price = c.Price,
                           CPTID = c.CPTID,
                           Modifier = c.Modifier, 
                           PracticeID = c.PracticeID,
                           description = cpt.Description, 
                           Inactive = c.Inactive,
                           AddedBy = c.AddedBy,
                           AddedDate = c.AddedDate,
                           ModifiedBy = c.ModifiedBy,
                           ModifiedDate = c.ModifiedDate,
                           cptCode = cpt.CPTCode 
                       })
                       .ToList();
            return lst;  
         
        }
         
        // POST: api/ClinicalForms 
        [HttpPost]
        [Route("SaveClinicalForms")]
        public async Task<ActionResult<ClinicalForms>> SaveClinicalForms(ClinicalForms clinicalForms)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.PatientCreate == false)
                return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
          
            bool succ = TryValidateModel(clinicalForms);

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            var cf = (from c in _context.ClinicalForms
                      where  ExtensionMethods.IsNull_Bool(c.Inactive) == false && c.PracticeID == UD.PracticeID && c.ID != clinicalForms.ID && c.Name.Trim() == clinicalForms.Name.Trim() && c.ProviderID == clinicalForms.ProviderID
                      select new
                      {
                          c.ID,
                          c.Name,
                          c.ProviderID 

                      }).ToList();
            if (cf != null && cf.Count > 0 && cf[0].ID > 0)
            {
                return BadRequest("Can not add same form name for this provider. Please change name.");
            }
            if (clinicalForms.formContent!=null && clinicalForms.formContent!="")
                {
                Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
          settings.DocumentServerDirectory, UD.PracticeID.ToString(), "ClinicalForm");

                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                string HTMLfileName = "";


                HTMLfileName = System.IO.Path.Combine(DirectoryPath, clinicalForms.Name + "_" + datetime + ".html").Replace(" ", "");


                string StatmentHtml =    Encoding.UTF8.GetString(Convert.FromBase64String((clinicalForms.formContent).Remove(0,22)));
                 System.IO.File.WriteAllText(HTMLfileName, StatmentHtml);
                clinicalForms.url = (clinicalForms.Name + "_" + datetime + ".html").Replace(" ","");
                _context.ClinicalForms.Add(clinicalForms);
            }
             
            if ( clinicalForms.ID <=0)
            {
                clinicalForms.AddedBy = UD.Email;
                clinicalForms.AddedDate = DateTime.Now;
                clinicalForms.PracticeID =UD.PracticeID;
                _context.ClinicalForms.Add(clinicalForms);

            }
            else //if (UD.Rights.PatientEdit == true)
            {

                clinicalForms.UpdatedBy = UD.Email;
                clinicalForms.UpdatedDate = DateTime.Now;
                _context.ClinicalForms.Update(clinicalForms);

            }
            //else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
              _context.SaveChanges();
            if(clinicalForms.CPTs!=null && clinicalForms.CPTs.Count>0)
            {
                foreach(ClinicalFormsCPT cpt in clinicalForms.CPTs)
                {
                    if (cpt.ID == 0)
                    {
                        cpt.AddedBy = UD.Email.ToString();
                        cpt.PracticeID = UD.PracticeID;
                        cpt.ClinicalFormID = clinicalForms.ID;
                        cpt.AddedDate = DateTime.Now;
                        _context.ClinicalFormsCPT.Add(cpt);

                    }
                    else //if (UD.Rights.PatientEdit == true)
                    {

                        cpt.ModifiedBy = UD.Email.ToString();
                        cpt.ModifiedDate = DateTime.Now;
                        _context.ClinicalFormsCPT.Update(cpt);

                    }
                }
              
            }
            _context.SaveChanges();
            return Ok(clinicalForms);

        }

        [Route("FormSubHeadings/{formId}")]
        [HttpGet("{formId}")]
        public async Task<IActionResult> FormSubHeadings(long? formId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
             
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formsubheading_data = (from p in _context.ClinicalForms
                                       join sub in _context.FormsSubHeading on p.ID equals sub.clinicalFormsID
                                       where ExtensionMethods.IsNull_Bool(p.Inactive) == false &&
                                       ExtensionMethods.IsNull_Bool(sub.Inactive) == false && p.ID == formId
                                       select new
                                       {
                                           sub.ID,
                                           sub.clinicalFormsID,
                                           sub.subheading,
                                           sub.type,
                                           sub.defaultValue,
                                           sub.appFunction,
                                           sub.customID,
                                           sub.practiceID,
                                           sub.Inactive,
                                           sub.AddedBy,
                                           sub.AddedDate,
                                           sub.UpdatedBy,
                                           sub.UpdatedDate
                                       }).ToList();

            if (formsubheading_data == null)
            {
                return NotFound();
            }

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(formsubheading_data);
        }

        [HttpPost]
        [Route("SaveSubHeadings")] 
        public async Task<ActionResult<FormsSubHeading>> SaveSubHeadings(FormsSubHeading formsSubHeading)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
         User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
         User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(formsSubHeading);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (formsSubHeading.ID <= 0)
            {
                formsSubHeading.AddedBy = UD.Email;
                formsSubHeading.AddedDate = DateTime.Now;
                formsSubHeading.practiceID = UD.PracticeID;
                _context.FormsSubHeading.Add(formsSubHeading);
            }
            else
            {
                formsSubHeading.UpdatedBy = UD.Email;
                formsSubHeading.UpdatedDate = DateTime.Now;
                _context.FormsSubHeading.Update(formsSubHeading);
            }
            _context.SaveChanges();
            return Ok(formsSubHeading);
        }
        // DELETE: api/ClinicalForms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinicalForms([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clinicalForms = await _context.ClinicalForms.FindAsync(id);
            if (clinicalForms == null)
            {
                return NotFound();
            }

            _context.ClinicalForms.Remove(clinicalForms);
            await _context.SaveChangesAsync();

            return Ok(clinicalForms);
        }

        private bool ClinicalFormsExists(long id)
        {
            return _context.ClinicalForms.Any(e => e.ID == id);
        }
        [Route("DownloadFormText/{ID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFormText(long ID)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
       settings.DocumentServerDirectory, UD.PracticeID.ToString(), "ClinicalForm");

            ClinicalForms clinicalForms = await _context.ClinicalForms.FindAsync(ID);
            if (clinicalForms != null)
            {
                string url = (DirectoryPath + "\\" + clinicalForms.url);
                if (!System.IO.File.Exists(url))
                {
                    return NotFound();
                }
                string formText = System.IO.File.ReadAllText(url);

                if (formText == null)
                    return NotFound();
             
                
                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
                return Ok(formText);
            }
            return NotFound();
        }

    }
}