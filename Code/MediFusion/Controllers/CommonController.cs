using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class CommonController : ControllerBase
    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public CommonController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        [Route("GetCityStateInfo/{zip}")]
        [HttpGet("{zip}")]
        public async Task<ActionResult<CityStateZipData>> GetCityStateInfo(string zip)
        {
            var result = VMCommon.GetCityStateZipInfo(_context, zip);
            if (result == null)
            {
                return BadRequest("InValid ZipCode");
            }
            return result;
        }

        [Route("GetCPT")]
        [HttpGet]
        public List<Data> GetCPT()
        {
           
            List<Data> CPT = (from i in _context.Cpt
                              select new Data()
                              {
                                  ID = i.ID,
                                  Value = i.Description,
                                  label = i.CPTCode,
                                  Description = i.Description,
                                  Description1 = i.DefaultUnits,
                                  Description2 = i.Amount,
                                  AnesthesiaUnits = i.AnesthesiaBaseUnits,
                                  Category = i.Category,
                                  
                              }).ToList();

        
            return CPT;
        }
        [Route("GetPOS")]
        [HttpGet]
        public List<DropDown> GetPOS()
        {
           
            List<DropDown> POS = (from p in _context.POS
                                  select new DropDown()
                                  {
                                      ID = p.ID,
                                      Description = p.PosCode + " - " + p.Name,
                                      Description2 = p.PosCode,
                                  }).ToList();

            POS.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
           
            return POS;
        }

        [Route("GetICD")]
        [HttpGet]
        public List<Data> GetICD()
        {

         
            var ICD = (from i in _context.ICD
                       select new Data()
                       {
                           ID = i.ID,
                           //Value = i.Description,
                           Value = null,
                           label = i.ICDCode,
                           //Description = i.Description,
                           Description = null
                       }).ToList();
           
            return ICD;
        }


        [Route("GetModifiers")]
        [HttpGet]
        public List<Data> GetModifiers()
        {
           
            List<Data> Modifier = (from i in _context.Modifier
                                   select new Data()
                                   {
                                       ID = i.ID,
                                       Value = i.Description,
                                       label = i.Code,
                                       Description = i.Description,
                                       AnesthesiaUnits = i.AnesthesiaBaseUnits.Value,
                                       Description2 = i.DefaultFees
                                   }).ToList();

            return Modifier;
        }

        [Route("GetTaxonomy")]
        [HttpGet]
        public List<Data> GetTaxonomy()
        {
           
            List<Data> Taxonomy = (from i in _context.Taxonomy
                                   select new Data()
                                   {
                                       ID = i.ID,
                                       Value = i.NUCCCode,
                                       label = i.NUCCCode,
                                   }).ToList();
           
            return Taxonomy;
        }


        [Route("GetTeams")]
        [HttpGet]
        public List<DropDown> GetTeams()
        {
           
            List<DropDown> Teams = (from u in _contextMain.MainTeam
                                    select new DropDown()
                                    {
                                        ID = u.ID,
                                        Description = u.Name, //+ " - " + p.Coverage, 
                                    }).ToList();
            Teams.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            return Teams;
        }

        [Route("GetDesignations")]
        [HttpGet]
        public List<DropDown> GetDesignations()
        {
            
            List<DropDown> Designations = (from u in _contextMain.MainDesignations
                                    select new DropDown()
                                    {
                                        ID = u.ID,
                                        Description = u.Name, //+ " - " + p.Coverage, 
                                    }).ToList();
            Designations.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            return Designations;
        }

        [Route("GetICDS")]
        [HttpGet]
        public List<Data> GetICDS()
        {

            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            List<Data> ICD = (from i in _context.ICD
                              select new Data()
                              {
                                  ID = i.ID,
                                  //Value = i.Description,
                                  label = i.ICDCode,
                                  //Description = i.Description,
                              }).ToList();
            ICD.Insert(0, new Data() { ID = null, Description = "Please Select" });

            //Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            //this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, ICD.Count);

            return ICD;
        }

        [Route("GetCPTS")]
        [HttpGet]
        public List<Data> GetCPTS()
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            List<Data> CPT = (from i in _context.Cpt
                              select new Data()
                              {
                                  ID = i.ID,
                                  //Value = i.Description,
                                  Value = null,
                                  label = i.CPTCode,
                                  //Description = i.Description,
                                  Description = null,
                                  Description1 = i.DefaultUnits,
                                  Description2 = i.Amount,
                                  AnesthesiaUnits = i.AnesthesiaBaseUnits,
                                  Category = i.Category
                              }).ToList();
            CPT.Insert(0, new Data() { ID = null, Description = "Please Select" });
            //Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            //this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, CPT.Count);

            return CPT;
        }

        [Route("GetLocations")]
        [HttpGet]
        public List<DropDown> GetLocations()
        {

            List<DropDown> Location = (from l in _context.Location
                              select new DropDown()
                              {
                                  ID = l.ID,
                                  Description = l.Name,
                                  Value = "",
                                  label = l.Name
                              }).ToList();
            Location.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return Location;
        }

        [Route("GetProvider")]
        [HttpGet]
        public List<DropDown> GetProvider()
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<DropDown> UserProviders = (from prac in _context.Practice
                                           join prov in _context.Provider on prac.ID equals prov.PracticeID
                                           where  prac.ID == PracticeId
                                            select new DropDown()
                                        {
                                            ID = prov.ID,
                                            ID2 = prac.ID,
                                            Description = prov.Name,
                                            Value = "",
                                            label = prov.Name
                                        }).ToList();
            UserProviders.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return UserProviders;
        }

        [Route("GetRefProvider")]
        [HttpGet]
        public List<DropDown> GetRefProvider()
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<DropDown> UserRefProviders = (from prac in _context.Practice
                                               join refProv in _context.RefProvider on prac.ID equals refProv.PracticeID
                                               where prac.ID == PracticeId
                                               select new DropDown()
                                               {
                                                   ID = refProv.ID,
                                                   ID2 = prac.ID,
                                                   Description = refProv.Name,
                                                   Value = "",
                                                   label = refProv.Name
                                               }).ToList();
            UserRefProviders.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return UserRefProviders;
        }

        [Route("GetUsserId")]
        [HttpGet]
        public ActionResult<string> GetUserId()    // This code is for window service to get practice ID and user ID
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value

            );

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return UD.UserID;
        }


        [Route("GetUserPractices")]
        [HttpGet()]
        [Authorize]
        public ActionResult<List<DropDown>> GetUserPractices()
        {
           
            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var Praclist = (from u in _contextMain.MainUserPractices
                            join v in _contextMain.MainPractice
                            on u.PracticeID equals v.ID
                            join w in _contextMain.Users
                            on u.UserID equals w.Id
                            where u.UserID == UserId.Value && u.Status == true
                            select new DropDown
                            {
                                ID = u.PracticeID,
                                // Description = 
                            }).ToList();
          
            return Praclist;
        }

        [Route("GetAutoFollowupPractices")]
        [HttpGet()]
        [Authorize]
        public ActionResult<List<DropDown>> GetAutoFollowupPractices()
        {

            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var Praclist = (from v in _contextMain.MainPractice
                            where v.IsAutoFollowup == true
                            select new DropDown
                            {
                                ID = v.ID,
                                // Description = 
                            }).ToList();
            return Praclist;
        }

        [Route("GetAutoDownloadingPractices")]
        [HttpGet()]
        [Authorize]
        public ActionResult<List<DropDown>> GetAutoDownloadingPractices()
        {

            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var Praclist = (from v in _contextMain.MainPractice
                            where v.IsAutoDownloading == true
                            select new DropDown
                            {
                                ID = v.ID,
                                // Description = 
                            }).ToList();
            return Praclist;
        }


        [Route("GetAutoEmailAppointmentReminderPractices")]
        [HttpGet()]
        [Authorize]
        public ActionResult<List<DropDown>> GetAutoEmailAppointmentReminderPractices()
        {

            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var Praclist = (from v in _contextMain.MainPractice
                            where v.IsEmailAppointmentReminder == true
                            select new DropDown
                            {
                                ID = v.ID,
                                // Description = 
                            }).ToList();
            return Praclist;
        }

        [Route("GetAutoClaimSubmissionPractices")]
        [HttpGet()]
        [Authorize]
        public ActionResult<List<DropDown>> GetAutoClaimSubmissionPractices()
        {

            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var Praclist = (from v in _contextMain.MainPractice
                            where v.IsAutoSubmission == true
                            select new DropDown
                            {
                                ID = v.ID,
                                // Description = 
                            }).ToList();
            return Praclist;
        }


        [Route("GetPatientStatusCode")]
        [HttpGet]
        public List<Data> GetPatientStatusCode()
        {

            List<Data> PatientStatusCode = (from i in _context.PatientStatusCode
                                            select new Data()
                                            {
                                                ID = i.ID,
                                                Value = i.Description,
                                                label = i.Code + " - " + i.Description,
                                                Description = i.Description,
                                            }).ToList();
           
            return PatientStatusCode;
        }
        [Route("GetBillClassification")]
        [HttpGet]
        public List<Data> GetBillClassification()
        {
           
            List<Data> BillClassification = (from i in _context.BillClassification
                                            select new Data()
                                            {
                                                ID = i.ID,
                                                Value = i.Description,
                                                label = i.Code + " - " + i.Description,
                                                Description = i.Description,
                                            }).ToList();
           
            return BillClassification;
        }
        [Route("GetTypeOfFacility")]
        [HttpGet]
        public List<Data> GetTypeOfFacility()
        {
           

            List<Data> TypeOfFacility = (from i in _context.TypeOfFacility
                                             select new Data()
                                             {
                                                 ID = i.ID,
                                                 Value = i.Description,
                                                 label = i.Code + " - " + i.Description,
                                                 Description = i.Description,
                                             }).ToList();

            return TypeOfFacility;
        }
        [Route("GetAdmissionSourceCode")]
        [HttpGet]
        public List<Data> GetAdmissionSourceCode()
        {
           
            List<Data> AdmissionSourceCode = (from i in _context.AdmissionSourceCode
                                         select new Data()
                                         {
                                             ID = i.ID,
                                             Value = i.Description,
                                             label = i.Code + " - " + i.Description,
                                             Description = i.Description,
                                         }).ToList();
          
            return AdmissionSourceCode;
        }
        [Route("GetOccurrenceCode")]
        [HttpGet]
        public List<Data> GetOccurrenceCode()
        {
           
            List<Data> OccurrenceCode = (from i in _context.OccurrenceCode
                                         select new Data()
                                              {
                                                  ID = i.ID,
                                                  Value = i.Description,
                                                  label = i.Code + " - " + i.Description,
                                                  Description = i.Description,
                                              }).ToList();

           
            return OccurrenceCode;
        }
       
        [Route("GetOccurrenceSpanCode")]
        [HttpGet]
        public List<Data> GetOccurrenceSpanCode()
        {
           
            List<Data> OccurrenceSpanCode = (from i in _context.OccurrenceSpanCode
                                             select new Data()
                                          {
                                             ID = i.ID,
                                             Value = i.Description,
                                             label = i.Code + " - " + i.Description,
                                             Description = i.Description,
                                         }).ToList();

            
            return OccurrenceSpanCode;
        }
        [Route("GetRevenueCode")]
        [HttpGet]
        public List<Data> GetRevenueCode()
        {
           
            List<Data> RevenueCode = (from i in _context.RevenueCode
                                             select new Data()
                                             {
                                                 ID = i.ID,
                                                 Value = i.Description,
                                                 label = i.Code + " - " + i.Description,
                                                 Description = i.Description,
                                             }).ToList();
           
            return RevenueCode;
        }

        [Route("GetExternalInjuryCode")]
        [HttpGet]
        public List<Data> GetExternalInjuryCode()
        {
            List<Data> ExternalCode = (from i in _context.ExternalInjuryCode
                                      select new Data()
                                      {
                                          ID = i.ID,
                                          Value = i.Description,
                                          label = i.Code + " - " + i.Description,
                                          Description = i.Description,
                                      }).ToList();

           
            return ExternalCode;
        }

        [Route("GetValueCode")]
        [HttpGet]
        public List<Data> GetValueCode()
        {

            List<Data> ValueCode = (from i in _context.ValueCode
                                       select new Data()
                                       {
                                           ID = i.ID,
                                           Value = i.Description,
                                           label = i.Code + " - " + i.Description,
                                           Description = i.Description,
                                       }).ToList();
           
            return ValueCode;
        }

        [Route("GetConditionCode")]
        [HttpGet]
        public List<Data> GetConditionCode()
        {
           
            List<Data> ConditionCode = (from i in _context.ConditionCode
                                    select new Data()
                                    {
                                        ID = i.ID,
                                        Value = i.Description,
                                        label = i.Code + " - " + i.Description,
                                        Description = i.Description,
                                    }).ToList();

          
            return ConditionCode;
        }
        [Route("GetAdmissionTypeCode")]
        [HttpGet]
        public List<Data> GetAdmissionTypeCode()
        {
           
            List<Data> GetAdmissionTypeCode = (from i in _context.AdmissionTypeCode
                                               select new Data()
                                               {
                                                   ID = i.ID,
                                                   Value = i.Description,
                                                   label = i.Code + " - " + i.Description,
                                                   Description = i.Description,
                                               }).ToList();
          
            return GetAdmissionTypeCode;
        }
        [Route("GetVisitReason")]
        [HttpGet]
        public List<Data> GetVisitReason()
        {

            List<Data> GetVisitReason = (from i in _context.VisitReason
                                               select new Data()
                                               {
                                                   ID = i.ID,
                                                   Value = i.Description,
                                                   label = i.Name + " - " + i.Description,
                                                   Description = i.Description,
                                               }).ToList();

            return GetVisitReason;
        }

        [Route("GetSubmitter")]
        [HttpGet]
        public Submitter GetSubmitter(long PracticeId)
        {
          

            var submitter = (from sub in _context.Submitter
                             join reciv in _context.Receiver on sub.ReceiverID equals reciv.ID
                             join clnt in _context.Client on sub.ClientID equals clnt.ID
                             join prac in _context.Practice on clnt.ID equals prac.ClientID
                             where prac.ID == PracticeId
                             select sub).FirstOrDefault();

           
            return submitter;
        }

        [Route("FindSubmitters")]
        [HttpGet()]
        [Authorize]
        public ActionResult<List<Submitter>> FindSubmitters(long PracticeID)
        {
           

            var submitter = (from sub in _context.Submitter
                             join reciv in _context.Receiver on sub.ReceiverID equals reciv.ID
                             join clnt in _context.Client on sub.ClientID equals clnt.ID
                             join prac in _context.Practice on clnt.ID equals prac.ClientID
                             where prac.ID == PracticeID
                             select sub).ToList();

           
            return submitter;
        }
        //public string TranslateAppointmentStatusCode(string StatusCode, List<GeneralItems> generalItems)
        //{
        //    long StatusCodeValue = long.Parse(StatusCode);
        //    string status = generalItems.Where(w => w.Value == StatusCodeValue).Select(s => s.Name).FirstOrDefault();
        //    return status;
        //}
        [Route("GeneralItems")]
        [HttpPost()] 
        public ActionResult<List<GeneralItems>> GeneralItems(CGeneralItems item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value

           );
            if(item.Type.Equals("0"))
                {
                var generalItemsType0 = (from sub in _context.GeneralItems 
                                    where ExtensionMethods.IsNull_Bool(sub.Inactive) != true
                                    && sub.Type.Equals("0")
                                    select new GeneralItems()
                                    {
                                        ID = sub.ID,
                                        Name = sub.Name,
                                        Value = sub.Value,
                                        Description = sub.Description,
                                        Type = "General Items Types",
                                        TypeFilter = sub.Type,
                                        AddedBy = sub.AddedBy,
                                        AddedDate = sub.AddedDate,
                                        Inactive = sub.Inactive,
                                        position = sub.position
                                    }).ToList();
                return generalItemsType0; 
            }
            var generalItems = (from sub in _context.GeneralItems
                                join gi2 in _context.GeneralItems.Where(w=>w.Type.Equals("0")) on sub.Type equals gi2.Type into giTable
                                from gi2T in giTable.DefaultIfEmpty()
                                where ExtensionMethods.IsNull_Bool(sub.Inactive)!=true 
                             select new  GeneralItems()
                             {
                                ID= sub.ID,
                                 Name= sub.Name,
                                 Value= sub.Value,
                                 Description=  sub.Description,
                                 Type= gi2T!=null? gi2T.Name:sub.Type,
                                 TypeFilter =  sub.Type,
                                 AddedBy = sub.AddedBy,
                                 AddedDate=sub.AddedDate,
                                 Inactive= sub.Inactive,
                                 position=sub.position 
                             }) ;
            if (item==null || item.Type =="" || item.Type != "0")
            {
                generalItems = generalItems.Where(w => w.TypeFilter.Trim().ToLower() != "0");
            }
            if (item!=null)
            {
               
                if (!ExtensionMethods.IsNull(item.Type)) 
                {
                   
                    generalItems = generalItems.Where(w => w.TypeFilter.Trim().ToLower().Equals (item.Type.Trim().ToLower()) );
                }
                 
                if (!ExtensionMethods.IsNull( item.Name ))
                {
                    generalItems = generalItems.Where(w => w.Name.Trim().ToLower().Contains (item.Name.Trim().ToLower()));
                }
                if (!ExtensionMethods.IsNull(item.Description ))
                {
                    generalItems = generalItems.Where(w => w.Description.Trim().ToLower().Contains (item.Description.Trim().ToLower()));
                }
                if (!ExtensionMethods.IsNull(item.Value ))
                {
                    generalItems = generalItems.Where(w => w.Value  == item.Value );
                }
                if (!ExtensionMethods.IsNull(item.position))
                {
                    generalItems = generalItems.Where(w => w.position  == item.position );
                }
            }
            
            var lst = generalItems.ToList();
            return lst;
        }

        [Route("SaveGeneralItems")]
        [HttpPost]
        public async Task<ActionResult<GeneralItems>>  SaveGeneralItems(GeneralItems generalItems)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(generalItems);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            var g = (from c in _context.GeneralItems
                      where ExtensionMethods.IsNull_Bool(c.Inactive) == false && c.Value == generalItems.Value
                       && c.ID != generalItems.ID
                     &&   c.Type.Trim().Equals(generalItems.Type.Trim())
                      select new
                      {
                          c.ID,
                          c.Name 
                      }).ToList();
            if (g != null && g.Count > 0 && g[0].ID > 0)
            {
                return BadRequest("Can not add same Value and Type. Please change value/Type.");
            }
            if (generalItems.ID<= 0)
            {
                if (generalItems.Value=="" || generalItems.Value.Trim().Length==0)
                {
                    List<DropDown> Rooms = (from m in _context.GeneralItems
                                            where m.Type.Trim().Equals(generalItems.Type.Trim()) && ExtensionMethods.IsNull_Bool(m.Inactive) != true
                                            select new DropDown()
                                            {
                                                ID = m.ID,
                                                Description = m.Name
                                            }).ToList();
                    if (Rooms == null)
                        generalItems.Value = "1";
                    else
                        generalItems.Value = (Rooms.Count + 1).ToString();
                }
                
                generalItems.AddedBy = UD.Email;
                generalItems.AddedDate = DateTime.Now;
                _context.GeneralItems.Add(generalItems); 
            }
            else  
            { 
                _context.GeneralItems.Update(generalItems); 
            }
            _context.SaveChanges();
            return Ok(generalItems);
        }
        public ActionResult DownloadAllDocument(UserInfoData UD, string FolderName, string[] Allurl)
        {
            // UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            //string resourcesPath = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement/");

            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
               settings.DocumentServerDirectory, UD.PracticeID.ToString(), FolderName);
            //string DirectoryPath = System.IO.Path.Combine("\\\\", "C:",place
            //settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");



            if (Allurl != null && Allurl.Length == 1)
            {
                string documenturl = System.IO.Path.Combine(DirectoryPath, Allurl[0].Replace("**", "\\"));
                if (!System.IO.File.Exists(documenturl))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(documenturl);
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index = documenturl.LastIndexOf("\\");
                string filename = documenturl.Substring(index + 1, documenturl.Length - index - 1);
                //Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                //this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

                return File(stream, "application/octec-stream", filename);
            }
            if (Allurl != null && Allurl.Length > 1)
            {

                var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                string sourcePath = DirectoryPath;
                string targetPath = System.IO.Path.Combine(DirectoryPath, "Allzipdf\\" + datetime);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string finalTargetPath = System.IO.Path.Combine(targetPath, datetime);
                if (!Directory.Exists(finalTargetPath))
                {
                    Directory.CreateDirectory(finalTargetPath);
                }
                foreach (var file in Allurl)
                {
                    string sourceFile = System.IO.Path.Combine(DirectoryPath, file.Replace("**", "\\"));
                    string destFile = System.IO.Path.Combine(finalTargetPath + file.Replace("\\", "").Replace("View**", ""));
                    //if (!Directory.Exists(destFile))
                    //{
                    //    Directory.CreateDirectory(destFile);
                    //}
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
                string zipFilePath = System.IO.Path.Combine(DirectoryPath, "Allzipdf\\zipfiles" + datetime + ".zip");

                ZipFile.CreateFromDirectory(targetPath, zipFilePath);
                Byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
                if (fileBytes == null)

                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index = zipFilePath.LastIndexOf("\\");
                string filename = zipFilePath.Substring(index + 1, zipFilePath.Length - index - 1);
                //Directory.Delete(System.IO.Path.Combine(DirectoryPath, "Allzipdf"), true);
                return File(stream, "application/octec-stream", filename);

            }
            return NotFound();
        }



        public PatientDocument SavePatientDocument(PatientDocument item, string Email, long ClientID, long PracticeID)
        {
            if (item.uploadeddocument != "" && item.uploadeddocument != null)
            {
                Models.Settings settings = _context.Settings.Where(s => s.ClientID == ClientID).SingleOrDefault();
                string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
          settings.DocumentServerDirectory, PracticeID.ToString(), "PatientDocument");//settings.DocumentServerURL

                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                var datetime =  DateTime.Now.Year.ToString() + "\\\\" + DateTime.Now.Month.ToString() + "\\\\" + DateTime.Now.Day.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                var datetimeurl = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                string patientDocumenturl = "";

                var finaldirectory = System.IO.Path.Combine(DirectoryPath , datetime);
                if (!Directory.Exists(finaldirectory))
                {
                    Directory.CreateDirectory(finaldirectory);
                }
                int index = item.name.LastIndexOf(".");
                if (index > 0)
                    item.name = item.name.Substring(0, index);
                string base64String = item.uploadeddocument.Split(',').Last();
                string ext = item.uploadeddocument.Split(';')[0].Split('/')[1];
                patientDocumenturl = System.IO.Path.Combine(finaldirectory, datetimeurl + "_" + item.name+"."+ext).Replace(" ", "");
                item.name = item.name +"."+ ext;




                byte[] Stbytes = System.Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(patientDocumenturl, Stbytes);
                item.url = System.IO.Path.Combine(datetime, datetimeurl + "_" + item.name+ ext);
                float filesize = new FileInfo(patientDocumenturl).Length;
                if (filesize < 1000)
                    item.size = filesize.ToString() + " KB";
                if (filesize > 1000)
                    filesize = filesize / 1000;
                item.size = filesize.ToString("0.00") + " MB";



            }
            item.uploadeddocument = "";

            if (item.id <= 0)
            {

                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.practiceID = PracticeID;
                item.inActive = false;
                _context.PatientDocument.Add(item);

            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PatientDocument.Update(item);
            }
            _context.SaveChanges();
            return item;
        }
        public void sendEmail(IConfiguration config, string ToEmail,string body,string subject, string CC,byte[] attachmentBytes,string name)
        {
            string FromEmail = config.GetSection("AutoEmailAppointmentReminders").GetSection("FromEmail").Value;
            string Host = config.GetSection("AutoEmailAppointmentReminders").GetSection("Host").Value;
            string Pass = config.GetSection("AutoEmailAppointmentReminders").GetSection("FromPassword").Value;
            if(CC==null || CC == "")
             CC = config.GetSection("AutoEmailAppointmentReminders").GetSection("CC").Value;

            ContentType ct = new ContentType(MediaTypeNames.Image.Jpeg);
          
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmail); //From Email Id  
            mailMessage.Subject = subject; //Subject of Email  
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body; //body or message of Email  
            Attachment att = new Attachment(new MemoryStream(attachmentBytes), name, MediaTypeNames.Image.Jpeg);
            mailMessage.Attachments.Add(att);

            string[] ToMuliId = ToEmail.Split(',');
            foreach (string ToEMailId in ToMuliId)
            {
                mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
            }
            if (CC.Trim().Length > 0)
            {
                string[] CCId = CC.Split(',');
                foreach (string CCEmail in CCId)
                {
                    mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
                }
            }
            SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
            smtp.Host = Host; //host of emailaddress for example smtp.gmail.com etc  
                              //network and security related credentials  
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = Pass;
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailMessage); //sending Email }


        }
     

    }
    public class DdocumentAll
    {
        public string[] document_address { get; set; }


    }
}
