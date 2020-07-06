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
using static MediFusionPM.ViewModels.VMPatient;
using MediFusionPM.Models.Audit;
using MediFusionPM.Models.Main;
using System.Diagnostics;
using System.Data.SqlClient;
using MediFusionPM.Uitilities;
using Plivo.XML;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public PatientController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetPatients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            

            return await _context.Patient.ToListAsync();
        }

        [Route("GetProfiles/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VMPatient>> GetProfiles(long id)
        {
          
            ViewModels.VMPatient obj = new ViewModels.VMPatient();
            obj.GetProfiles(_context, id);
            return obj;
        }


        [Route("FindPatient/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> FindPatient(long id)
        {
            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
           
            patient.PatientPlans = _context.PatientPlan.Where(m => m.PatientID == id).ToList<PatientPlan>();
            //patient.Note = _context.Notes.Where(m => m.PatientID == id).ToList<Notes>();
            //patient.PatientAuthorization = _context.PatientAuthorization.Where(m => m.PatientID == id).ToList<PatientAuthorization>();
            //patient.PatientReferrals = _context.PatientReferral.Where(m => m.PatientID == id).ToList<PatientReferral>();
            return patient;
        }
        [Route("FindPatientNotes/{PatientID}")]
        [HttpGet("{PatientID}")]
        public List<Notes> FindPatientNotes(long PatientID)
        {
            List<Notes> notes = _context.Notes.Where(m => m.PatientID == PatientID).ToList<Notes>();
            if(notes == null)
            {
              BadRequest("No Record Found.");
            }
            return notes;
        }

        [Route("FindPatientAuthorization/{PatientID}")]
        [HttpGet("{PatientID}")]
        public List<PatientAuthorization> FindPatientAuthorization(long PatientID)
        {
            List<PatientAuthorization> auth = _context.PatientAuthorization.Where(m => m.PatientID == PatientID).ToList<PatientAuthorization>();
            if (auth == null)
            {
                BadRequest("No Record Found.");
            }
            return auth;
        }
        [Route("FindPatientReferral/{PatientID}")]
        [HttpGet("{PatientID}")]
        public List<PatientReferral> FindPatientReferral(long PatientID)
        {
            List<PatientReferral> reff = _context.PatientReferral.Where(m => m.PatientID == PatientID).ToList<PatientReferral>();
            if (reff == null)
            {
                BadRequest("No Record Found.");
            }
            return reff;
        }


        [Route("PatientSummary/{id}")]
        [HttpGet("{id}")]
        public   ActionResult PatientSummary(long id)
        {
            
            var patient = (from p in _context.Patient 
                           join pr in _context.Provider  on p.ProviderID equals pr.ID into table1 from prT in table1.DefaultIfEmpty()
                           join lc in _context.Location on p.LocationId equals lc.ID into table2  from lcT in table2.DefaultIfEmpty()

                           where p.ID == id  
                           select new  
                           {
                               ID = p.ID,
                               AccountNum = p.AccountNum,
                               MedicalRecordNumber = p.MedicalRecordNumber,
                               PatientName = p.LastName.Trim() + ", " + p.FirstName.Trim(),
                               provider =prT.ID>0? prT.Name.Trim() : "",
                               location = lcT.ID > 0 ? lcT.Name.Trim() : "",
                               PhoneNumber = p.PhoneNumber,
                               ProfilePic=p.ProfilePic,
                               Address = p.Address1 +" "+p.City+" "+p.State+" "+p.ZipCode,
                               SSN = p.SSN,
                               p.Email,
                               DOB = p.DOB!=null? p.DOB.Format("MM/dd/yyyy"):"",
                               age = p.DOB!=null?(DateTime.Now.Year - p.DOB.Value.Year).ToString():"",
                               p.AddedBy,
                               AddedDate = p.AddedDate != null ? p.AddedDate.Format("MM/dd/yyyy hh:mm tt"):"",
                               p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate!=null? p.UpdatedDate.Format("MM/dd/yyyy hh:mm tt"):"",

                           }).FirstOrDefault();

            if (patient == null)
            {
                return NotFound();
            }
           
            return Json(patient);
        }
        [HttpGet]
        [Route("SearchPatients")] 
        public async Task<ActionResult<IEnumerable<GPatient>>> SearchPatients(string criteria)
        {
            // var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
            long PracticeId =Convert.ToInt64( User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.PatientSearch == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");


            if (ExtensionMethods.IsNull(criteria))
            {
                return BadRequest("Please add criteria.");

            } 
            List<GPatient> data = (from p in _context.Patient  
                                   where p.PracticeID == PracticeId && p.IsActive == false &&
                                  
                                   ( p.LastName.ToLower().Contains(criteria.ToLower()) || 
                                   p.FirstName.ToLower().Contains(criteria.ToLower())  ||
                                    p.AccountNum.Contains(criteria)) // ||  object.Equals(criteria, p.DOB.Format("MM/dd/yyyy")))
                                   // &&
                                   //(CPatient.MedicalRecordNumber.IsNull() ? true : p.MedicalRecordNumber.Equals(CPatient.MedicalRecordNumber)) &&
                                   //(CPatient.SSN.IsNull() ? true : p.SSN.Equals(CPatient.SSN)) && 
                                   select new GPatient()
                                   {
                                       ID = p.ID,
                                       AccountNum = p.AccountNum,
                                       MedicalRecordNumber = p.MedicalRecordNumber,
                                       LastName = p.LastName.Trim(),
                                       FirstName = p.FirstName.Trim(),
                                       SSN = p.SSN,
                                       DOB = p.DOB!=null? p.DOB.Format("MM/dd/yyyy"):"",
                                   }).ToList();
            
            return data;
        }


        [Route("PatientDetails/{id}")]
        [HttpGet("{id}")]
        public ActionResult PatientDetails(long id)
        {
            // var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);


            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.PatientSearch == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            var PatientPlan = (from p in _context.Patient
                               join pp in _context.PatientPlan on p.ID equals pp.PatientID into Table4
                               from t4 in Table4.DefaultIfEmpty()
                               join ip in _context.InsurancePlan on t4.InsurancePlanID equals ip.ID into Table5
                               from t5 in Table5.DefaultIfEmpty()
                               where p.PracticeID == PracticeId && p.ID == id
                              && t4.Coverage == "P"
                               select new
                               {
                                   PatientID = p.ID,
                                   PlanName = t5.PlanName,
                                   PatientPlanID = t4.ID,
                                   DOB = p.DOB.Format("MM/dd/yyyy")
                               }).ToList().FirstOrDefault();
            var ret = new { PatientPlan = PatientPlan };
            return Json(ret);
        }

        [HttpPost]
        [Route("FindPatients")]
        public async Task<ActionResult<IEnumerable<GPatient>>> FindPatients(CPatient CPatient)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
           
            return FindPatients(CPatient, PracticeId, temp);
        }
        private List<GPatient> FindPatients(CPatient CPatient, long PracticeId, string temp)
        {


            string connectionstring = CommonUtil.GetConnectionString(PracticeId, temp);
            List<GPatient> data = new List<GPatient>();
            using (SqlConnection myconnection = new SqlConnection(connectionstring))
            {
                string ostring = "select p.id, p.accountnum, p.medicalrecordnumber, p.lastname, p.firstname, p.ssn ,convert(varchar,p.dob,101) as dob, p.practiceid, p.providerid, pr.name as provider, l.id as locationid, l.name as locationname, p.isactive, convert(varchar, p.addeddate, 101) as addeddate from patient as p left join provider as pr on p.providerid = pr.id left join location as l on p.locationid = l.id ";

                // Plan Search 
                if (!CPatient.Plan.IsNull())
                    ostring += " join PatientPlan pPlan on  p.id = pPlan.PatientID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID ";
                // Subscriber ID Search 
                if (!CPatient.InsuredID.IsNull())
                    ostring += " join PatientPlan pPlan on p.ID = pPlan.PatientID ";



                ostring += " where p.practiceid = {0} ";
                ostring = string.Format(ostring, PracticeId);

                // Plan Search 
                if (!CPatient.Plan.IsNull())
                    ostring += string.Format(" and iPlan.PlanName like '%{0}%'", CPatient.Plan);
                // Subscriber ID Search 
                if (!CPatient.InsuredID.IsNull())
                    ostring += string.Format(" and pPlan.SubscriberId like '%{0}%'", CPatient.InsuredID);

                if (!CPatient.Provider.IsNull())
                    ostring += string.Format(" and pr.name like '%{0}%'", CPatient.Provider);

                if (!CPatient.Location.IsNull())
                    ostring += string.Format(" and l.name like '%{0}%'", CPatient.Location);


                if (!CPatient.LastName.IsNull())
                {
                    if (CPatient.LastName.Contains("'"))
                    {
                        //Modify ContextName
                        string RLastName = CPatient.LastName.Trim();
                        RLastName = RLastName.Replace("'", "''");
                        ostring += string.Format(" and p.LastName like '%{0}%'", RLastName);
                    }
                    else
                    {
                        ostring += string.Format(" and p.LastName like '%{0}%'", CPatient.LastName);
                    }

                }

                if (!CPatient.FirstName.IsNull())
                {
                    if (CPatient.FirstName.Contains("'"))
                    {
                        //Modify ContextName
                        string RFirstName = CPatient.FirstName.Trim();
                        RFirstName = RFirstName.Replace("'", "''");
                        ostring += string.Format(" and p.FirstName like '%{0}%'", RFirstName);
                    }
                    else
                    {
                        ostring += string.Format(" and p.FirstName like '%{0}%'", CPatient.FirstName);
                    }

                }
                if (!CPatient.AccountNum.IsNull())
                    ostring += string.Format(" and p.AccountNum ='{0}'", CPatient.AccountNum);
                if (CPatient.DOB != null)
                    ostring += string.Format(" and p.DOB = '{0}'", CPatient.DOB);
                if (!CPatient.MedicalRecordNumber.IsNull())
                    ostring += string.Format(" and p.MedicalRecordNumber ='{0}' ", CPatient.MedicalRecordNumber);
                if (!CPatient.SSN.IsNull())
                    ostring += string.Format(" and p.SSN = '{0}'", CPatient.SSN);
                if (CPatient.InActive == false)
                    ostring += string.Format(" and p.IsActive ='{0}'", CPatient.InActive);

                if (CPatient.EntryDateFrom != null && CPatient.EntryDateTo != null)
                {
                    ostring += (" and (p.addeddate  >= '" + CPatient.EntryDateTo.GetValueOrDefault().Date + "' and p.addeddate  < '" + CPatient.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CPatient.EntryDateFrom != null && CPatient.EntryDateTo == null)
                {
                    ostring += (" and ( p.addeddate >= '" + CPatient.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CPatient.EntryDateFrom == null && CPatient.EntryDateTo != null)
                {
                    ostring += (" and (p.addeddate <= '" + CPatient.EntryDateTo.GetValueOrDefault().Date + "')");
                }






                SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                myconnection.Open();

                using (SqlDataReader oreader = ocmd.ExecuteReader())
                {
                    while (oreader.Read())
                    {
                        data.Add(new GPatient()
                        {
                            ID = long.Parse(oreader["id"].ToString()),
                            AccountNum = oreader["accountnum"].ToString(),
                            MedicalRecordNumber = oreader["medicalrecordnumber"].ToString(),
                            LastName = oreader["lastname"].ToString(),
                            FirstName = oreader["firstname"].ToString(),
                            SSN = oreader["ssn"].ToString(),
                            DOB = oreader["dob"].ToString(),
                            PracticeID = long.Parse(oreader["practiceid"].ToString()),
                            //  practice = t1.name,
                            ProviderID = oreader["providerid"].ToString() != "" ? long.Parse(oreader["providerid"].ToString()) : 0,
                            Provider = oreader["provider"].ToString(),
                            LocationID = oreader["locationid"].ToString() != "" ? long.Parse(oreader["locationid"].ToString()) : 0,
                            Location = oreader["locationname"].ToString(),
                            AddedDate = oreader["addeddate"].ToString()
                        });
                    }
                    myconnection.Close();
                }
            }

            //------------------------------




            return data;
        }







        //private List<GPatient> FindPatients(CPatient CPatient, long PracticeId, string temp)
        //{
        //    List<GPatient> data = (from p in _context.Patient
        //                           join pr in _context.Provider on p.ProviderID equals pr.ID into Table2
        //                           from t2 in Table2.DefaultIfEmpty()
        //                           join l in _context.Location on p.LocationId equals l.ID into Table3
        //                           from t3 in Table3.DefaultIfEmpty()
        //                           where p.PracticeID == PracticeId
        //                           && (CPatient.Provider.IsNull() ? true : t2.Name.Contains(CPatient.Provider))
        //                           && (CPatient.Location.IsNull() ? true : t3.Name.Contains(CPatient.Location))
        //                           && (CPatient.LastName.IsNull() ? true : p.LastName.Contains(CPatient.LastName))
        //                           && (CPatient.FirstName.IsNull() ? true : p.FirstName.Contains(CPatient.FirstName))
        //                           && (CPatient.AccountNum.IsNull() ? true : p.AccountNum.Equals(CPatient.AccountNum))
        //                           && (CPatient.DOB == null ? true : object.Equals(CPatient.DOB, p.DOB))
        //                           && (CPatient.MedicalRecordNumber.IsNull() ? true : p.MedicalRecordNumber.Equals(CPatient.MedicalRecordNumber))
        //                           && (CPatient.SSN.IsNull() ? true : p.SSN.Equals(CPatient.SSN))
        //                           && (CPatient.InActive ? p.IsActive == false : true)
        //                           && ((CPatient.EntryDateFrom != null && CPatient.EntryDateTo != null) ? (p.AddedDate != null ? p.AddedDate.Date <= CPatient.EntryDateTo.GetValueOrDefault().Date && p.AddedDate.Date >= CPatient.EntryDateFrom.GetValueOrDefault().Date : p.AddedDate != null ? p.AddedDate.Date >= CPatient.EntryDateFrom.GetValueOrDefault() : false) : (CPatient.EntryDateFrom != null ? (p.AddedDate != null && CPatient.EntryDateFrom.HasValue ? p.AddedDate.Date >= CPatient.EntryDateFrom.GetValueOrDefault() : true) : true))
        //                           //&& (ExtensionMethods.IsBetweenDOS(CPatient.EntryDateTo, CPatient.EntryDateFrom, p.AddedDate, p.AddedDate))
        //                           orderby p.AccountNum descending
        //                           select new GPatient()
        //                           {
        //                               ID = p.ID,
        //                               AccountNum = p.AccountNum,
        //                               MedicalRecordNumber = p.MedicalRecordNumber,
        //                               LastName = p.LastName,
        //                               FirstName = p.FirstName,
        //                               SSN = p.SSN,
        //                               DOB = p.DOB.Format("MM/dd/yyyy"),
        //                               PracticeID = p.PracticeID,
        //                               //  Practice = t1.Name,
        //                               ProviderID = t2.ID,
        //                               Provider = t2.Name,
        //                               LocationID = t3.ID,
        //                               Location = t3.Name,
        //                               IsActive = p.IsActive,
        //                               AddedDate = p.AddedDate.Format("MM/dd/yyyy")
        //                           }).ToList();
        //    // Plan Search 
        //    if (!CPatient.Plan.IsNull())
        //    {
        //        data = (from d in data
        //                join pPlan in _context.PatientPlan on d.ID equals pPlan.PatientID
        //                join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
        //                where (iPlan.PlanName.IsNull() ? true : iPlan.PlanName.Contains(CPatient.Plan))
        //                select d).ToList();
        //    }
        //    // Subscriber ID Search 
        //    if (!CPatient.InsuredID.IsNull())
        //    {
        //        data = (from d in data
        //                join pPlan in _context.PatientPlan on d.ID equals pPlan.PatientID
        //                where (pPlan.SubscriberId.IsNull() ? true : pPlan.SubscriberId.Equals(CPatient.InsuredID))
        //                select d).ToList();
        //    }


        //    //string connectionString = CommonUtil.GetConnectionString(PracticeId, temp);
        //    //List<GPatient> data = new List<GPatient>();
        //    //using (SqlConnection myConnection = new SqlConnection(connectionString))
        //    //{
        //    //    string oString = "select p.id, p.AccountNum, p.MedicalRecordNumber, p.LastName, p.FirstName, p.SSN ,convert(varchar,p.DOB,101) as DOB, p.practiceID, p.ProviderID, pr.Name as Provider, l.ID as LocationID, l.Name as LocationName, p.IsActive, convert(varchar, p.Addeddate, 101) as AddedDate from Patient as p left join Provider as pr on p.providerID = pr.ID left join Location as l on p.LocationID = l.id where p.practiceID = {0} ";
        //    //    oString = string.Format(oString, PracticeId);

        //    //    if (!CPatient.Provider.IsNull())
        //    //        oString += string.Format(" AND pr.NAME LIKE '%{0}%'", CPatient.Provider);

        //    //    SqlCommand oCmd = new SqlCommand(oString, myConnection);
        //    //    myConnection.Open();

        //    //    using (SqlDataReader oReader = oCmd.ExecuteReader())
        //    //    {
        //    //        while (oReader.Read())
        //    //        {
        //    //            data.Add(new GPatient()
        //    //            {
        //    //                ID = long.Parse(oReader["ID"].ToString()),
        //    //                AccountNum = oReader["AccountNum"].ToString(),
        //    //                MedicalRecordNumber = oReader["MedicalRecordNumber"].ToString(),
        //    //                LastName = oReader["LastName"].ToString(),
        //    //                FirstName = oReader["FirstName"].ToString(),
        //    //                SSN = oReader["SSN"].ToString(),
        //    //                DOB = oReader["DOB"].ToString(),
        //    //                PracticeID = long.Parse(oReader["PracticeID"].ToString()),
        //    //                //  Practice = t1.Name,
        //    //                ProviderID = oReader["ProviderID"].ToString() != "" ? long.Parse(oReader["ProviderID"].ToString()) : 0,
        //    //                Provider = oReader["Provider"].ToString(),
        //    //                LocationID = oReader["LocationId"].ToString() != "" ? long.Parse(oReader["LocationId"].ToString()) : 0,
        //    //                Location = oReader["LocationName"].ToString(),
        //    //                AddedDate = oReader["AddedDate"].ToString()
        //    //            });
        //    //        }
        //    //        myConnection.Close();
        //    //    }
        //    //}

        //    return data;
        //}

        //private List<GPatient> FindPatients(CPatient CPatient, long PracticeId)
        //{

        //    List<GPatient> data = _context.Patient.FromSql("select p.id,p.AccountNum,p.MedicalRecordNumber,p.LastName,p.FirstName,p.SSN,convert(varchar,p.DOB,101) as DOB,p.practiceID, p.ProviderID, pr.Name as Provider, l.ID as LocationID, l.Name as LocationName, p.IsActive, convert(varchar, p.Addeddate, 101) as AddedDate from Patient as p left join Provider as pr on p.providerID = pr.ID left join Location as l on p.LocationID = l.id  ").ToList();
        //    return data;
        //    //string whereClause = "";
        //    //if (!CPatient.Provider.IsNull()) whereClause += " PROVIDER LIKE '%%'";
        //}

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPatient CPatient)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            List<GPatient> data = FindPatients(CPatient, PracticeId, temp);
            ExportController controller = new ExportController(_context);
          
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD,CPatient, "Patient Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPatient CPatient)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            List<GPatient> data = FindPatients(CPatient, PracticeId, temp);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SavePatient")]
        [HttpPost]
        public async Task<ActionResult<Patient>> SavePatient(Patient item)
        {

            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            try
            {
                long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
                var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
                var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;

            Patient OldPatient = _context.Patient.Where(p => p.ID == item.ID).AsNoTracking().FirstOrDefault();

            MainRights rights = (from u in _contextMain.MainRights
                    where u.Id == UserId
                               select u).FirstOrDefault();

            bool succ = TryValidateModel(item);
          
            //Checking Existing Patient On Name/DOB/SSN
                bool AccountExists = _context.Patient.Count(p => p.LastName == item.LastName && p.FirstName == item.FirstName && (item.DOB == null ? p.DOB == null : p.DOB == item.DOB) && item.ID == 0) > 0;
            if (AccountExists)
            {
                return BadRequest("Patient With Same Information Already Exists.");
            }


                //BatchDocument Batch = _context.BatchDocument.Where(bd => bd.ID == item.BatchDocumentID).FirstOrDefault();
                long BatchDocumentIDToUse = 0;
                BatchDocument Batch = null;
                if(item.BatchDocument != null)
                {
                    Batch = item.BatchDocument;
                }
                if (Batch == null)
                {
                    if (item.BatchDocumentID.GetValueOrDefault() != 0)
                    {
                        Batch = _context.BatchDocument.Where(bd => bd.ID == item.BatchDocumentID.GetValueOrDefault()).FirstOrDefault();
                    }
                    else
                    {
                        if (OldPatient != null)
                        {
                            if (OldPatient.BatchDocumentID.GetValueOrDefault() != 0)
                            {
                                Batch = _context.BatchDocument.Where(bd => bd.ID == OldPatient.BatchDocumentID.GetValueOrDefault()).FirstOrDefault();
                            }
                        }
                    }
                }
                if (Batch != null)
                {
                    if (OldPatient != null)
                    {
                        bool CounterChangedOnce = false;
                        if ((OldPatient.PageNumber != null && OldPatient.PageNumber != "") && (item.PageNumber == null && item.PageNumber == ""))
                        {
                            Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() - 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                            CounterChangedOnce = true;
                        }
                        else if ((OldPatient.PageNumber == null && OldPatient.PageNumber == "") && (item.PageNumber != null && item.PageNumber != ""))
                        {
                            Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() + 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                            CounterChangedOnce = true;
                        }
                        if (OldPatient.BatchDocumentID.GetValueOrDefault() != 0 && item.BatchDocumentID.GetValueOrDefault() == 0)
                        {
                            if (CounterChangedOnce == false)
                                Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() - 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                            item.BatchDocument = null;
                        }
                        else if ((OldPatient.BatchDocumentID.GetValueOrDefault() == 0 && item.BatchDocumentID.GetValueOrDefault() != 0))
                        {
                            if (CounterChangedOnce == false)
                                Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() + 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                        }
                    }
                }
                if(item.ID==0)
                {
                    if (item.BatchDocumentID.GetValueOrDefault() != 0)
                    {
                        Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() + 1;
                        if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                        item.DocumentBatchApplied = true;
                    }
                }
                if (Batch != null)
                {
                    _context.BatchDocument.Update(Batch);
                }
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return BadRequest(messages);
                }
                //if (item.BatchDocumentID != null && !item.PageNumber.IsNull())
                //{
                //    BatchDocument batch = _context.BatchDocument.Find(item.BatchDocumentID);
                //    if (batch != null && item.DocumentBatchApplied != true)
                //    {
                //        batch.NoOfDemographicsEntered = batch.NoOfDemographicsEntered.ValZero() + 1;
                //        if (batch.StartDate == null) batch.StartDate = DateTime.Now;

            //        _context.BatchDocument.Update(batch);
            //    }
            //}
            //else if (item.ID > 0 && item.BatchDocumentID == null && item.PageNumber.IsNull() && item.DocumentBatchApplied == true)
            //{
            //    Patient pat = _context.Patient.Find(item.ID);
            //    BatchDocument batch = _context.BatchDocument.Find(pat.BatchDocumentID);
            //    if (batch != null && batch.NoOfDemographicsEntered.ValZero() > 0)
            //    {
            //        batch.NoOfDemographicsEntered = batch.NoOfDemographicsEntered.ValZero() - 1;
            //        _context.BatchDocument.Update(batch);
            //    }
            //    item.DocumentBatchApplied = false;
            
            if (item.ID == 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
              
                item.AccountNum = _context.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");
                item.NewAccountNum = item.AccountNum;
                _context.Patient.Add(item);

                if (item.Note != null)
                {
                    foreach (Notes notes in item.Note)
                    {
                        if (notes.ID <= 0)
                        {
                            notes.PatientID = item.ID;
                            notes.AddedBy = Email;
                            notes.AddedDate = DateTime.Now;
                            notes.NotesDate = DateTime.Now;
                            _context.Notes.Add(notes);
                        }
                    }
                }
                
                if (item.PatientPlans != null)
                {

                    var countPrimary = (from pPlan in item.PatientPlans where pPlan.Coverage == "P" && pPlan.IsActive == true select pPlan).ToList().Count();
                    if (countPrimary > 1)
                    {
                        return BadRequest("Can Not Select Multiple Primary  Covrages As Active.");
                    }
                    var countSecondary = (from pPlan in item.PatientPlans where pPlan.Coverage == "S" && pPlan.IsActive == true select pPlan).ToList().Count();
                    if (countSecondary > 1)
                    {
                        return BadRequest("Can Not Select Multiple Secondary  Covrages As Active.");
                    }
                    var countTertiary = (from pPlan in item.PatientPlans where pPlan.Coverage == "T" && pPlan.IsActive == true select pPlan).ToList().Count();
                    if (countTertiary > 1)
                    {
                        return BadRequest("Can Not Select Multiple Tertiary  Covrages As Active.");
                    }

                    foreach (PatientPlan patientPlan in item.PatientPlans)
                    {
                        bool PlanExists = _context.PatientPlan.Count(
                        p => p.Coverage == patientPlan.Coverage && p.PatientID == patientPlan.PatientID && p.IsActive == true && item.ID == 0) > 0;
                        if (PlanExists)
                        {
                            return BadRequest("Patient With  Covrage  -" + patientPlan.Coverage + " Already Exists In PatientPlan ");
                        }


                        if (patientPlan.ID <= 0)
                        {
                            patientPlan.PatientID = item.ID;
                            patientPlan.AddedBy = Email;
                            patientPlan.AddedDate = DateTime.Now;
                            //   patientPlan.NotesDate = DateTime.Now;
                            _context.PatientPlan.Add(patientPlan);
                        }
                    }
                }


            }
            else if (rights.PatientEdit == true)
            {
                //  bool AccountExistsUpdate = _context.Patient.Count(p => p.AccountNum == item.AccountNum && item.ID != p.ID ) > 0;

                bool PatientExistsUpdate = _context.Patient.Any(p =>   p.LastName == item.LastName &&  p.FirstName == item.FirstName && (item.DOB == null ? p.DOB == null : p.DOB == item.DOB) && item.ID != p.ID);


                if (PatientExistsUpdate == true)
                {
                    return BadRequest("Patient With Same Information Already Exists.");
                }

                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                item.NewAccountNum = item.AccountNum;
                _context.Patient.Update(item);

                if (item.Note != null)
                {
                    foreach (Notes notes in item.Note)
                    {
                        if (notes.ID <= 0)
                        {
                            notes.PatientID = item.ID;
                            notes.AddedBy = Email;
                            notes.AddedDate = DateTime.Now;
                            notes.NotesDate = DateTime.Now;
                            _context.Notes.Add(notes);
                        }
                        else
                        {

                            notes.PatientID = item.ID;
                            notes.UpdatedBy = Email;
                            notes.UpdatedDate = DateTime.Now;
                            notes.NotesDate = DateTime.Now;
                            _context.Notes.Update(notes);

                        }
                    }
                }


                if (item.PatientPlans != null)
                {
                    var countPrimary = (from pPlan in item.PatientPlans where pPlan.Coverage == "P" && pPlan.IsActive == true select pPlan).ToList().Count();
                    if (countPrimary > 1)
                    {
                        return BadRequest("Can Not Select Multiple Primary  Covrages As Active.");
                    }
                    var countSecondary = (from pPlan in item.PatientPlans where pPlan.Coverage == "S" && pPlan.IsActive == true select pPlan).ToList().Count();
                    if (countSecondary > 1)
                    {
                        return BadRequest("Can Not Select Multiple Secondary  Covrages As Active.");
                    }
                    var countTertiary = (from pPlan in item.PatientPlans where pPlan.Coverage == "T" && pPlan.IsActive == true select pPlan).ToList().Count();
                    if (countTertiary > 1)
                    {
                        return BadRequest("Can Not Select Multiple Tertiary  Covrages As Active.");
                    }
                    foreach (PatientPlan patientPlan in item.PatientPlans)
                    {


                        var temp = patientPlan;
                        if (temp.ID <= 0)
                        {
                            bool PlanExists = _context.PatientPlan.Count(
                           p => p.Coverage == temp.Coverage && p.PatientID == temp.PatientID && temp.IsActive == true && item.ID == 0) > 0;
                            if (PlanExists)
                            {
                                return BadRequest("Patient With  Covrage  -" + temp.Coverage + " Already Exists In PatientPlan ");
                            }
                            patientPlan.PatientID = item.ID;
                            patientPlan.AddedBy = Email;
                            patientPlan.AddedDate = DateTime.Now;
                            _context.PatientPlan.Add(patientPlan);
                        }
                        else
                        {
                            if (temp.IsActive == false)
                            {
                                var plan = (from pPlan in item.PatientPlans where pPlan.ID == temp.ID && pPlan.IsActive == true select pPlan).SingleOrDefault();
                                if (plan != null)
                                {
                                    plan.IsActive = temp.IsActive;
                                    plan.UpdatedBy = Email;
                                    plan.UpdatedDate = DateTime.Now;
                                    _context.PatientPlan.Update(plan);
                                    _context.SaveChanges();

                                }
                            }
                                if (temp.IsActive == true)
                                {
                                    var plantrue = (from pPlan in item.PatientPlans where pPlan.ID == temp.ID && pPlan.IsActive == true select pPlan).SingleOrDefault();
                                    if (plantrue != null)
                                    {
                                        plantrue.IsActive = temp.IsActive;
                                        plantrue.UpdatedBy = Email;
                                        plantrue.UpdatedDate = DateTime.Now;
                                        _context.PatientPlan.Update(plantrue);
                                        _context.SaveChanges();

                                    }
                                }
                            patientPlan.PatientID = item.ID;
                            patientPlan.UpdatedBy = Email;
                            patientPlan.UpdatedDate = DateTime.Now;
                            _context.PatientPlan.Update(patientPlan);

                        }
                    }
                }
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            await _context.SaveChangesAsync();

            if (item.PatientAuthorization != null)
            {
                foreach (PatientAuthorization patientAuthorization in item.PatientAuthorization)
                {
                    var a = patientAuthorization;
                    if (patientAuthorization.ID <= 0)
                    {
                        if (patientAuthorization.PatientID == 0)
                        {
                            PatientPlan patientPlan = item.PatientPlans.Where(p => p.CommonKey == patientAuthorization.CommonKey && p.InsurancePlanID == patientAuthorization.InsurancePlanID).FirstOrDefault();

                            if (patientPlan != null)
                            {
                                patientAuthorization.PatientPlanID = patientPlan.ID;
                                patientAuthorization.PatientID = item.ID;
                                patientAuthorization.AddedBy = Email;
                                patientAuthorization.AddedDate = DateTime.Now;
                                _context.PatientAuthorization.Add(patientAuthorization);
                             
                            }
                        }
                        else
                        {
                            PatientPlan patientPlan = item.PatientPlans.Where(p => p.CommonKey == patientAuthorization.CommonKey && p.InsurancePlanID == patientAuthorization.InsurancePlanID).FirstOrDefault();
                            if (patientPlan != null)
                            {
                                patientAuthorization.PatientPlanID = patientPlan.ID;
                                patientAuthorization.PatientID = item.ID;
                                patientAuthorization.UpdatedBy = Email;
                                patientAuthorization.UpdatedDate = DateTime.Now;
                                _context.PatientAuthorization.Update(patientAuthorization);

                            }
                        }
                    }
                    else
                    {
                        if (patientAuthorization.PatientID == 0)
                        {
                            PatientPlan patientPlan = item.PatientPlans.Where(p => p.CommonKey == patientAuthorization.CommonKey && p.InsurancePlanID == patientAuthorization.InsurancePlanID).FirstOrDefault();

                            if (patientPlan != null)
                            {
                                patientAuthorization.PatientPlanID = patientPlan.ID;
                                patientAuthorization.PatientID = item.ID;
                                patientAuthorization.AddedBy = Email;
                                patientAuthorization.AddedDate = DateTime.Now;
                                _context.PatientAuthorization.Add(patientAuthorization);
                                
                            }
                        }
                        else
                        {

                            PatientPlan patientPlan = item.PatientPlans.Where(p => p.CommonKey == patientAuthorization.CommonKey && p.InsurancePlanID == patientAuthorization.InsurancePlanID).FirstOrDefault();
                            if (patientPlan != null)
                            {
                                patientAuthorization.PatientPlanID = patientPlan.ID;
                                patientAuthorization.PatientID = item.ID;
                                patientAuthorization.UpdatedBy = Email;
                                patientAuthorization.UpdatedDate = DateTime.Now;
                                _context.PatientAuthorization.Update(patientAuthorization);
                               
                            }
                        }
                    }
                }
            }
            // yaha pa kren gay?
            if(item.PatientReferrals != null)
            {
                foreach (PatientReferral referral in item.PatientReferrals)
                {
                    PatientPlan patientPlan = item.PatientPlans.Where(p => p.CommonKey == referral.CommonKey && p.PatientID == item.ID).FirstOrDefault();
                    if (referral.PatientPlanID == 0)
                    {
                        referral.PatientPlanID = patientPlan.ID;
                        referral.PatientID = item.ID;
                        referral.UpdatedBy = Email;
                        referral.UpdatedDate = DateTime.Now;
                        _context.PatientReferral.Update(referral);
                    }

                }

            }

            await _context.SaveChangesAsync();

                return Ok(item);
            }
            catch(Exception ex)
            {
                return BadRequest("Something went wrong. Please contact BellMedEx");
            }
        }


        [Route("DeletePatient/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePatient(long id)
        {
           
            var patient = await _context.Patient.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
          
            return Ok();
        }


        //[Route("GetpatientPlansByPatientID/{PatientId}")]
        //[HttpGet("{PatientId}")]
        //public List<DropDown> GetpatientPlansByPatientID(long PatientId)
        //{
        //    List<DropDown> data = (from pp in _context.PatientPlan
        //                           join ip in _context.InsurancePlan
        //                            on pp.InsurancePlanID equals ip.ID
        //                           where pp.PatientID == PatientId
        //                           orderby pp.Coverage ascending

        //                           select new DropDown()
        //                           {
        //                               ID = pp.ID,
        //                               Description = pp.Coverage,
        //                               Description2 = ip.PlanName,
        //                           }).ToList();
        //    // data.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        //    return data;

        //}


        [Route("FindAudit/{PatientID}")]
        [HttpGet("{PatientID}")]
        public List<PatientAudit> FindAudit(long PatientID)
        {
         
            List<PatientAudit> data = (from pAudit in _context.PatientAudit
                                       where pAudit.PatientID == PatientID
                                       orderby pAudit.AddedDate descending
                                       select new PatientAudit()
                                       {
                                           ID = pAudit.ID,
                                           PatientID = pAudit.PatientID,
                                           TransactionID = pAudit.TransactionID,
                                           ColumnName = pAudit.ColumnName,
                                           CurrentValue = pAudit.CurrentValue,
                                           NewValue = pAudit.NewValue,
                                           CurrentValueID = pAudit.CurrentValueID,
                                           NewValueID = pAudit.NewValueID,
                                           HostName = pAudit.HostName,
                                           AddedBy = pAudit.AddedBy,
                                           AddedDate = pAudit.AddedDate,
                                       }).ToList<PatientAudit>();
   
            return data;
        }



        [Route("GetPatientUsedAuthorizations/{PatientID}")]
        [HttpGet("{PatientID}")]
        public List<GVMPatientUsedAuthorizations> GetPatientUsedAuthorizations(long PatientID)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);

            List<GVMPatientUsedAuthorizations> data = (from pAuth in _context.PatientAuthorization
                                                       join pAuthUse in _context.PatientAuthorizationUsed
                                                       on pAuth.ID equals pAuthUse.PatientAuthID
                                                       join pat in _context.Patient
                                                       on pAuth.PatientID equals pat.ID
                                                       join vi in _context.Visit
                                                       on pat.ID equals vi.PatientID
                                                       join ch in _context.Charge
                                                       on vi.ID equals ch.VisitID
                                                       join cpt in _context.Cpt
                                                       on ch.CPTID equals cpt.ID
                                                       where pAuth.PatientID == PatientID
                                                       && PracticeId == vi.PracticeID
                                                       orderby pAuth.AddedDate descending
                                                       select new GVMPatientUsedAuthorizations()
                                                       {
                                                           AuthorizationNumber = pAuth.AuthorizationNumber,
                                                           VisitID = pAuthUse.VisitID,
                                                           DOS = vi.DateOfServiceFrom,
                                                           BilledAmount = (vi.PrimaryBilledAmount + vi.SecondaryBilledAmount + vi.TertiaryBilledAmount),
                                                           ProviderName = pat.Provider.Name,
                                                           CPT = cpt.Description,
                                                       }).ToList<GVMPatientUsedAuthorizations>();
           
            return data;
        }

        [Route("DeletePatientAuth/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePatientAuth(long id)
        {

            
            var patientAuth = await _context.PatientAuthorization.FindAsync(id);

            if (patientAuth == null)
            {
                return BadRequest("Record Not Found.");
            }
            _context.PatientAuthorization.Remove(patientAuth);
            await _context.SaveChangesAsync();

           
            return Ok();
        }


        [Route("DeletePatientReferral/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePatientReferral(long id)
        {
          

            var patientRef = await _context.PatientReferral.FindAsync(id);

            if (patientRef == null)
            {
                return BadRequest("Record Not Found.");
            }
            _context.PatientReferral.Remove(patientRef);
            await _context.SaveChangesAsync();

          
            return Ok();
        }

        //[Route("GetAuthorizationUsedVisit/{PatientAuthorizationID}")]
        //[HttpGet("{PatientAuthorizationID}")]
        //public List<GVMVisitUsedAuthorizations> GetAuthorizationUsedVisit(long PatientAuthorizationID)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //   User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //   User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

        //    List<GVMVisitUsedAuthorizations> data = (from pAuth in _context.PatientAuthorization
        //                                             join pAuthUse in _context.PatientAuthorizationUsed on pAuth.ID equals pAuthUse.PatientAuthID
        //                                             join iPlan in _context.InsurancePlan on pAuth.InsurancePlanID equals iPlan.ID
        //                                             join pPLan in _context.PatientPlan on pAuth.PatientPlanID equals pPLan.ID
        //                                             join v in _context.Visit on pAuthUse.VisitID equals v.ID
        //                                             join c in _context.Charge on pAuthUse.VisitID equals c.VisitID
        //                                             join cpt in _context.Cpt on pAuth.CPTID equals cpt.ID

        //                                             where pAuth.ID == PatientAuthorizationID
        //                                             && UD.PracticeID == v.PracticeID
        //                                             orderby pAuth.AddedDate descending
        //                                             select new GVMVisitUsedAuthorizations()
        //                                             {
        //                                                 InsurancePlan = iPlan.PlanName,
        //                                                 SubscriberID = pPLan.SubscriberId,
        //                                                 AuthorizationNumber = pAuth.AuthorizationNumber,
        //                                                 VisitID = pAuthUse.VisitID,
        //                                                 ChargeID = c.ID,
        //                                                 DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
        //                                                 CPT = cpt.CPTCode,
        //                                                 BilledAmount = c.PrimaryBilledAmount.Val() + c.SecondaryBilledAmount.Val() + c.TertiaryBilledAmount.Val(),
        //                                                 AllowedAmount = c.PrimaryAllowed.Val() + c.SecondaryAllowed.Val() + c.TertiaryAllowed.Val(),
        //                                                 PaidAmount = c.PrimaryPaid.Val() + c.SecondaryPaid.Val() + c.TertiaryPaid.Val(),
        //                                                 Balance = c.PrimaryBal.Val() + c.SecondaryBal.Val() + c.TertiaryBal.Val()

        //                                             }).ToList<GVMVisitUsedAuthorizations>();
        //    return data;
        //}

        [Route("GetAuthorizationUsedVisit/{PatientAuthorizationID}")]
        [HttpGet("{PatientAuthorizationID}")]

        public async Task<ActionResult<IEnumerable<GVMVisitUsedAuthorizations>>> GetAuthorizationUsedVisit(long PatientAuthorizationID)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return await (from pAuth in _context.PatientAuthorization
                          join pAuthUse in _context.PatientAuthorizationUsed on pAuth.ID equals pAuthUse.PatientAuthID
                          join v in _context.Visit on pAuthUse.VisitID equals v.ID
                          join c in _context.Charge on pAuthUse.VisitID equals c.VisitID
                          join cpt in _context.Cpt on pAuth.CPTID equals cpt.ID
                          join p in _context.Patient on v.PatientID equals p.ID
                          join pplan in _context.PatientPlan on p.ID equals pplan.PatientID
                          join iplan in _context.InsurancePlan on pplan.InsurancePlanID equals iplan.ID

                          where pAuth.ID == PatientAuthorizationID
                          && UD.PracticeID == v.PracticeID
                          && pAuth.CPTID == c.CPTID
                          && pAuth.PatientID == p.ID
                          && pAuthUse.VisitID == v.ID
                          && pAuth.PatientPlanID == iplan.ID
                          && pplan.InsurancePlanID == iplan.ID

                          orderby pAuth.AddedDate descending
                          select new GVMVisitUsedAuthorizations()
                          {
                              InsurancePlan = iplan.PlanName,
                              SubscriberID = pplan.SubscriberId,
                              AuthorizationNumber = pAuth.AuthorizationNumber,
                              VisitID = pAuthUse.VisitID,
                              ChargeID = c.ID,
                              DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                              CPT = cpt.CPTCode,
                              BilledAmount = c.PrimaryBilledAmount.Val() + c.SecondaryBilledAmount.Val() + c.TertiaryBilledAmount.Val(),
                              AllowedAmount = c.PrimaryAllowed.Val() + c.SecondaryAllowed.Val() + c.TertiaryAllowed.Val(),
                              PaidAmount = c.PrimaryPaid.Val() + c.SecondaryPaid.Val() + c.TertiaryPaid.Val(),
                              Balance = c.PrimaryBal.Val() + c.SecondaryBal.Val() + c.TertiaryBal.Val()

                          }).Distinct().ToListAsync();

            //    return data;
        }


        ////FOr testing 
        //[HttpGet]
        //[Route("UpateAccount")]
        //public void  UpateAccount()
        //{
        //    List<Patient> pat = _context.Patient.ToList<Patient>();
        //    foreach(Patient p in pat)
        //    {
        //        p.NewAccountNum = p.AccountNum;
        //        _context.Patient.Update(p);
        //    }
        //    _context.SaveChanges();
        //}
        [HttpGet]
        [Route("PatientAdvInstantSearch")]
        public async Task<ActionResult<IEnumerable<PatientInfoDropDown>>> PatientAdvInstantSearch(string criteria)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);

            if (ExtensionMethods.IsNull(criteria))
            {
                return BadRequest("Please add criteria.");
            }

            var PatientInfo = (from p in _context.Patient
                               join l in _context.Location
                               on p.LocationId equals l.ID
                               where p.PracticeID == PracticeId &&
                               (p.LastName.ToLower().Contains(criteria.ToLower()) ||
                                   p.FirstName.ToLower().Contains(criteria.ToLower()) ||
                                    p.AccountNum.Contains(criteria))
                               select new PatientInfoDropDown()
                               {
                                   PatientID = p.ID.ToString(),
                                   PatientName = p.LastName + ", " + p.FirstName,
                                   AccountNumber = p.AccountNum,
                                   DOB = p.DOB != null ? ((DateTime)(p.DOB)).ToString("MM/dd/yyyy") : "",
                                   Gender = p.Gender,
                                   PracticeID = p.PracticeID,
                                   LocationID = p.LocationId,
                                   POSID = l.POSID,
                                   providerID = p.ProviderID,
                                   RefProviderID = p.RefProviderID,
                                   label = p.LastName + " " + p.FirstName + " " + p.AccountNum,
                                   Value = p.LastName + " " + p.FirstName + " " + p.AccountNum,

                               }).ToList();

            return PatientInfo;
        }

        //[Route("FindPatientOnly/{id}")]
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Patient>> FindPatientOnly(long id)
        //{
        //    var patient = await _context.Patient.FindAsync(id);
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }
        //    return patient;
        //}



        [Route("FindPatientOnly/{id}")]
        [HttpGet("{id}")]

        public List<OnlyPatient> FindPatientOnly(long id)
        {
            List<OnlyPatient> patient = (from pat in _context.Patient
                                         join loc in _context.Location on pat.LocationId equals loc.ID
                                         where pat.ID == id
                                         select new OnlyPatient()
                                         {
                                             ID = pat.ID,
                                             AccountNum = pat.AccountNum,
                                             LastName = pat.LastName,
                                             FirstName = pat.FirstName,
                                             DOB = pat.DOB.Format("MM/dd/yyyy"),
                                             Gender = pat.Gender,
                                             PracticeID = pat.PracticeID,
                                             LocationID = pat.LocationId,
                                             POSID = loc.POSID,
                                             ProviderID = pat.ProviderID,
                                             RefProviderID = pat.RefProviderID
                                         }).ToList();
        

            return patient;
        }





    }
}