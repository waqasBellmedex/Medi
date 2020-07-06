using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPatientFollowup;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientDocumentController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public IConfiguration _config;

        List<long> checkid = new List<long>();
        List<DocumentCategory> types = new List<DocumentCategory>();
        
        public PatientDocumentController(ClientDbContext context, MainContext contextMain, IConfiguration config)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
            this._config = config;
        }

        [Route("SavePatientDocument")]
        [HttpPost]
        public IActionResult SavePatientDocument(PatientDocument item)
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
            CommonController obj = new CommonController(_context, _contextMain);
            var result = obj.SavePatientDocument(item, Email, UD.ClientID, UD.PracticeID);


            return Ok(result);
        }
        [Route("DeletePatientDocument")]
        [HttpPost]
        public IActionResult DeletePatientDocument(ListModel listValues)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            if (listValues != null && listValues.Ids != null && listValues.Ids.Length > 0)
            {

                List<PatientDocument> result = (from p in _context.PatientDocument
                                                where listValues.Ids.Contains(p.id)
                                                select p).ToList();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.inActive = true;
                        item.UpdatedBy = UD.Email;
                        item.UpdatedDate = DateTime.Now;
                        _context.PatientDocument.Update(item);
                    }

                    _context.SaveChanges();

                    return Ok("Succesfully Deleted");
                }
            }
            return Ok("No record found");
        }
        [Route("SearchCategory")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentCategory>>> SearchCategory(string criteria)
        {

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);



            if (ExtensionMethods.IsNull(criteria))
            {
                return BadRequest("Please add criteria.");

            }
            List<DocumentCategory> data = (from dc in _context.DocumentCategory
                                           where dc.practiceID == PracticeId && dc.inActive == false &&

                                   (dc.name.ToLower().Contains(criteria.ToLower()))


                                           select new DocumentCategory()
                                           {
                                               id = dc.id,
                                               name = dc.name,

                                           }).ToList();

            return data;
        }

        [Route("Documents")]
        [HttpGet]
        public ActionResult Documents(long? patientID)
        {
            //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (ExtensionMethods.IsNull(patientID))
            {
                return BadRequest("Please select Patient.");
            }

            var PatientDocument = (from pd in _context.PatientDocument
                                   join p in _context.Patient on pd.PatientID equals p.ID
                                   join dc in _context.DocumentCategory on pd.DocumentCategoryID equals dc.id into table4
                                   from dcT in table4.DefaultIfEmpty()

                                       // join pa in _context.PatientAppointment on pn.AppointmentID equals pa.ID
                                   where pd.PatientID == patientID && ExtensionMethods.IsNull_Bool(pd.inActive) == false
                                   select new
                                   {
                                       pd.id,
                                       pd.name,
                                       pd.DocumentCategoryID,
                                       pd.PatientID,
                                       pd.size,
                                       categoryName = dcT != null ? dcT.name : "",
                                      UploadedDate = pd.UploadedDate !=null? pd.UploadedDate.Format("MM/dd/yyyy hh:mm:s tt"):"",
                                       pd.uploadeBy,
                                       pd.url,
                                       pd.notes,
                                       pd.inActive,
                                       pd.practiceID,
                                       pd.AddedBy,
                                       pd.AddedDate,
                                       pd.UpdatedBy,
                                       pd.UpdatedDate,
                                       documentDate = pd.documentDate != null ? pd.documentDate.Format("MM/dd/yyyy") : "",

                                   }).ToList();


            //else
            return Ok(PatientDocument);
        }

        [Route("SaveDocumentCategory")]
        [HttpPost]
        public ActionResult SaveDocumentCategory(DocumentCategory item)
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

            if (item.iconcontent != "" && item.iconcontent != null)
            {
                Models.Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
          settings.DocumentServerDirectory, UD.PracticeID.ToString(), "Categoryicons");//settings.DocumentServerURL

                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                var datetime = DateTime.Now.Year.ToString() + "\\\\" + DateTime.Now.Month.ToString() + "\\\\" + DateTime.Now.Day.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                var datetimeurl = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                string patientDocumenturl = "";
                var finaldirectoy = System.IO.Path.Combine(DirectoryPath, datetime);
                if (!Directory.Exists(finaldirectoy))
                {
                    Directory.CreateDirectory(finaldirectoy);
                }



                patientDocumenturl = System.IO.Path.Combine(finaldirectoy, datetimeurl + "_" + item.name).Replace(" ", "");



                string base64String = item.iconcontent.Split(',').Last();
                System.IO.File.WriteAllBytes(patientDocumenturl, Convert.FromBase64String(base64String));
                item.url = System.IO.Path.Combine(datetime, datetimeurl + "_" + item.name);


            }
            item.iconcontent = "";





            if (item.id <= 0)
            {

                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.practiceID = PracticeId;
                item.inActive = false;
                _context.DocumentCategory.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.DocumentCategory.Update(item);
            }
            _context.SaveChanges();

            return Ok(item);
        }
        public string iconbase64(string directorypath, string url)
        {if (url == "" || url == null)
                return "";
            var results = Convert.ToBase64String(System.IO.File.ReadAllBytes(System.IO.Path.Combine(directorypath, url)));


            return results;
        }
        [Route("DocumentCategory")]
        [HttpGet]
        public ActionResult DocumentCategory()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            Models.Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
      settings.DocumentServerDirectory, UD.PracticeID.ToString(), "Categoryicons");//settings.DocumentServerURL


            var documentCatogry = (from dc1 in _context.DocumentCategory

                                   join dc2 in _context.DocumentCategory on dc1.ParentcategoryID equals dc2.id into table4
                                   from dcT in table4.DefaultIfEmpty()


                                   where dc1.practiceID == UD.PracticeID && ExtensionMethods.IsNull_Bool(dc1.inActive) == false
                                   select new DocumentCategory()
                                   {
                                       id = dc1.id,
                                       text = dc1.name,
                                       description = dc1.description,
                                       type = dc1.type,
                                       parentName = dcT != null ? dcT.name : "",
                                       ParentcategoryID = dc1.ParentcategoryID,
                                       inActive = dc1.inActive,
                                       practiceID = dc1.practiceID,
                                       url = dc1.url,
                                       AddedBy = dc1.AddedBy,
                                       AddedDate = dc1.AddedDate,
                                       UpdatedBy = dc1.UpdatedBy,
                                       UpdatedDate = dc1.UpdatedDate,
                                       iconcontent = iconbase64(DirectoryPath, dc1.url)

                                   }).ToList();

            checkid = new List<long>();
            types = new List<DocumentCategory>();

            List<DocumentCategory> result = Recursivefortree(documentCatogry, null);

            return Ok(result);

        }
        private List<DocumentCategory> Recursivefortree(List<DocumentCategory> documentCatogry, DocumentCategory child = null)
        {
            foreach (var parent in documentCatogry)
            {
                if (checkid.Contains(parent.id))
                    continue;
                /*  if (parent.nodes == null)
                      parent.nodes = new List<DocumentCategory>();*/
                DocumentCategory child1 = RecursiveforNodes(documentCatogry, parent);

                // parent.nodes.Add(child1);
                if (child1 != null)
                {
                    types.Add(child1);
                    checkid.Add(child1.id);
                }


            }
            return types;
        }

        private DocumentCategory RecursiveforNodes(List<DocumentCategory> documentCatogry, DocumentCategory Catogry)
        {
            var nodes = documentCatogry.Where(w => w.ParentcategoryID == Catogry.id).ToList();
            if (nodes != null && nodes.Count > 0)
            {
                Catogry.nodes = nodes;
                foreach (var cc in nodes)
                {

                    checkid.Add(cc.id);
                    DocumentCategory last = RecursiveforNodes(documentCatogry, cc);
                    if (last == null || last.id <= 0)
                    {
                        return Catogry;
                    }
                    else
                        last = RecursiveforNodes(documentCatogry, cc);
                }

            }
            else
            {
                return Catogry;
            }
            return Catogry;
        }






        [Route("DownloadPatientDocument/{documentURL}")]
        [HttpGet]
        //  public ActionResult DownloadPatientDocument(string documentURL)
        //  {
        //      UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

        //      Models.Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
        //      string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
        //settings.DocumentServerDirectory, UD.PracticeID.ToString(), "PatienDocument");//settings.DocumentServerURL
        //      var documentContent= System.IO.File.ReadAllBytes(System.IO.Path.Combine(DirectoryPath, documentURL));
        //      return Ok(documentContent);


        //  }
        [HttpPost]
        [Route("DownloadPatientDocument")]

        public ActionResult DownloadPatientDocument(DdocumentAll document)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            CommonController obj = new CommonController(_context, _contextMain);

            var stream = obj.DownloadAllDocument(UD, "PatientDocument", document.document_address);
            return stream;



        }
        [HttpPost]
        [Route("PatientEmail")]

        public ActionResult PatientEmail(VMEmail VMEmail)
        {

                UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            EmailController obj = new EmailController(_context, _contextMain,_config);
            var result = obj.sendEmail(VMEmail);
        
            if (result.ToString() == "Ok")
            {
                obj.savelog(VMEmail, UD.Email, UD.ClientID, UD.PracticeID);

                return Ok("Successfully sent");


            }
            else
                return BadRequest(result);
            
               // return BadRequest("Attachment Cannot be greater then 10 MB");

        }





    }

}
