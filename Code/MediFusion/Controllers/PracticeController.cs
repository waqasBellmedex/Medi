    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using MediFusionPM.Models;
    using MediFusionPM.Models.TodoApi.Models;
    using static MediFusionPM.ViewModels.VMPractice;
    using System.IO;
    using Microsoft.AspNetCore.Authorization;
    using System.IdentityModel.Tokens.Jwt;
    using MediFusionPM.ViewModels;
    using static MediFusionPM.ViewModels.VMCommon;
    using Newtonsoft.Json.Linq;
    using System.Diagnostics;
    using MediFusionPM.Models.Main;
    using MediFusionPM.Models.Audit;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediFusionPM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PracticeController : ControllerBase
    {
        private readonly ClientDbContext _context_client;
        private readonly MainContext _context_main;
        public PracticeController(ClientDbContext contextClient, MainContext contextMain)
        {
            _context_client = contextClient;
            _context_main = contextMain;

            //// Only For Testing
            //if (_context_client.Practice.Count() == 0)
            //{
            //  //  _context_client.Facilities.Add(new Facility { Name = "ABC Facility", OrganizationName = "" });
            //   // _context_client.SaveChanges();
            //}
        }
        [Route("GetProfiles/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VMPractice>> GetProfiles(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            ViewModels.VMPractice obj = new ViewModels.VMPractice();
            obj.GetProfiles(_context_client, id);

            return obj;
        }

        [HttpGet]
        [Route("GetPractices")]
        public async Task<ActionResult<IEnumerable<MainPractice>>> GetPractices()
        {
            try
            {
                // object o = null;
                //string oo = o.ToString();
                return await _context_main.MainPractice.ToListAsync();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context_main.env.ContentRootPath, "Logs", "Practice.txt"), ex.ToString());
                throw ex;
            }
        }

        [Route("FindPractice/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MainPractice>> FindPractice(long id)
        {
            var practice = await _context_main.MainPractice.FindAsync(id);

            if (practice == null)
            {
                return NotFound();
            }

            return practice;
        }

        [Route("FindAudit/{PracticeID}")]
        [HttpGet("{PracticeID}")]
        public  List<PracticeAudit> FindAudit(long PracticeID)
        {
            List<PracticeAudit> data = (from pAudit in _context_client.PracticeAudit
                                        where pAudit.PracticeID == PracticeID
                                        orderby pAudit.AddedDate descending
                                        select new PracticeAudit()
                                    {
                                       ID = pAudit.ID,
                                       PracticeID = pAudit.PracticeID,
                                       TransactionID = pAudit.TransactionID,
                                        ColumnName = pAudit.ColumnName,
                                        CurrentValue = pAudit.CurrentValue,
                                        NewValue = pAudit.NewValue,
                                        CurrentValueID = pAudit.CurrentValueID,
                                        NewValueID = pAudit.NewValueID,
                                        HostName = pAudit.HostName,
                                        AddedBy = pAudit.AddedBy,
                                        AddedDate = pAudit.AddedDate,
                                    }).ToList<PracticeAudit>();
            return data;
        }

        [HttpPost]
        [Route("FindPractices")]
        public ActionResult<IEnumerable<GPractice>> FindPractices(CPractice CPractice)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Valuei);
            //if (UD == null || UD.Rights == null || UD.Rights.PracticeSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;
            return FindPractices(CPractice, PracticeId,Email,UserId);
        }


        private List<GPractice> FindPractices(CPractice CPractice, long PracticeId,string Email,String UserId)
        {
            List<GPractice> data = (from p in _context_main.MainPractice
                                    //join up in _context_main.MainUserPractices on p.ID equals up.PracticeID
                                    //join u in _context_main.Users on up.UserID equals u.Id
                                    where p.ID == PracticeId // && u.Id.ToString() == UserId && u.IsUserBlock == false
                                    &&(CPractice.Name.IsNull() ? true : p.Name.Contains(CPractice.Name)) 
                                    &&(CPractice.OrganizationName.IsNull() ? true : p.OrganizationName.Contains(CPractice.OrganizationName)) 
                                    &&(CPractice.NPI.IsNull() ? true : p.NPI == (CPractice.NPI))
                                    &&(CPractice.TaxID.IsNull() ? true : p.TaxID == (CPractice.TaxID)) 
                                    &&(CPractice.PhoneNumber.IsNull() ? true : p.OfficePhoneNum == (CPractice.PhoneNumber)) 
                                    &&(CPractice.Address.IsNull() ? true : p.Address1.Contains(CPractice.Address))
                                    select new GPractice()
                                    {
                                        ID = p.ID,
                                        Name = p.Name,
                                        NPI = p.NPI,
                                        OfficePhoneNum = p.OfficePhoneNum,
                                        OrganizationName = p.OrganizationName,
                                        PayToAddress = p.PayToAddress1,
                                        Address = p.Address1 + ", " + p.City + ", " + p.State + ", " + p.ZipCode,
                                        TaxID = p.TaxID,
                                    }).ToList();

            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPractice CPractice)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;
            List<GPractice> data = FindPractices(CPractice, PracticeId, Email, UserId);
            ExportController controller = new ExportController(_context_client);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CPractice, "Practice Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPractice CPractice)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;
            List<GPractice> data = FindPractices(CPractice, PracticeId, Email, UserId);
            ExportController controller = new ExportController(_context_client);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SavePractice")]
        [HttpPost]
        public async Task<ActionResult<MainPractice>> SavePractice(MainPractice item)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            var CurrentLoginUser = (from u in _context_main.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();

            string LoginUserId = CurrentLoginUser.Id;


            if (item.ZipCode.IsNull()) item.ZipCode = null;

            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }


            //check Client Id exist or not
            var cli = (from u in _context_main.MainClient
                       where u.ID == item.ClientID
                       select u).FirstOrDefault();

            if (cli != null) { _context_client.setDatabaseName(cli.ContextName); }
            bool PracticeExists = _context_client.Practice.Count(p => p.Name == item.Name && item.ID == 0) > 0;
            if (PracticeExists)
            {
                return BadRequest("Practice With Same Name Already Exists");
            }

            //Practice Id Came or not from if not came than Create Practice else Update Practice
            if (item.ID == 0)
            {
                //Create Practice
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context_main.MainPractice.Add(item);
                await _context_main.SaveChangesAsync();

                Practice pra = new Practice();
                pra.ID = item.ID;
                pra.Name = item.Name;
                pra.ClientID = item.ClientID;
                pra.OrganizationName = item.OrganizationName;
                pra.TaxID = item.TaxID;
                pra.CLIANumber = item.CLIANumber;
                pra.NPI = item.NPI;
                pra.SSN = item.SSN;
                pra.Type = item.Type;
                pra.TaxonomyCode = item.TaxonomyCode;
                pra.Address1 = item.Address1;
                pra.Address2 = item.Address2;
                pra.City = item.City;
                pra.State = item.State;
                pra.ZipCode = item.ZipCode;
                pra.OfficePhoneNum = item.OfficePhoneNum;
                pra.FaxNumber = item.FaxNumber;
                pra.Email = item.Email;
                pra.Website = item.Website;
                pra.PayToAddress1 = item.PayToAddress1;
                pra.PayToAddress2 = item.PayToAddress2;
                pra.PayToCity = item.PayToCity;
                pra.PayToState = item.PayToState;
                pra.PayToZipCode = item.PayToZipCode;
                pra.DefaultLocationID = item.DefaultLocationID;
                pra.WorkingHours = item.WorkingHours;
                pra.Notes = item.Notes;
                pra.IsActive = item.IsActive;
                pra.IsDeleted = item.IsDeleted;
                pra.AddedBy = item.AddedBy;
                pra.AddedDate = item.AddedDate;
                pra.UpdatedBy = item.UpdatedBy;
                pra.UpdatedDate = item.UpdatedDate;
                pra.ProvFirstName = item.ProvFirstName;
                pra.ProvLastName = item.ProvLastName;
                pra.ProvMiddleInitial = item.ProvMiddleInitial;
                pra.StatementExportType = item.StatementExportType;
                pra.StatementMessage = item.StatementMessage;
                pra.StatementAgingDays = item.StatementAgingDays;
                pra.StatementMaxCount = item.StatementMaxCount;
                //New Fields Are Added
                pra.CellNumber = item.CellNumber;
                pra.ContactPersonName = item.ContactPersonName;
                pra.InvoicePercentage = item.InvoicePercentage;
                pra.MinimumMonthlyAmount = item.MinimumMonthlyAmount;
                pra.NumberOfFullTimeEmployees = item.NumberOfFullTimeEmployees;
                pra.FTEPerDayRate = item.FTEPerDayRate;
                pra.FTEPerWeekRate = item.FTEPerWeekRate;
                pra.FTEPerMonthRate = item.FTEPerMonthRate;
                pra.IncludePatientCollection = item.IncludePatientCollection;
                pra.ClientCategory = item.ClientCategory;
                pra.RefferedBy = item.RefferedBy;
                pra.PMSoftwareName = item.PMSoftwareName;
                pra.EHRSoftwareName = item.EHRSoftwareName;
                pra.StatementPhoneNumber = item.StatementPhoneNumber;
                pra.StatementPhoneNumberExt = item.StatementPhoneNumberExt;
                pra.AppointmentPhoneNumber = item.AppointmentPhoneNumber;
                pra.AppointmentPhoneNumberExt = item.AppointmentPhoneNumberExt;
                pra.StatementFaxNumber = item.StatementFaxNumber;
                pra.IsAutoFollowup = item.IsAutoFollowup;
                pra.IsAutoDownloading = item.IsAutoDownloading;
                pra.IsAutoSubmission = item.IsAutoSubmission;
                pra.PLDDirectory = item.PLDDirectory;
                pra.IsEmailAppointmentReminder = item.IsEmailAppointmentReminder;
                pra.IsSMSAppointmentReminder = item.IsSMSAppointmentReminder;
                pra.IsEmailAppointmentReminder = item.IsEmailAppointmentReminder;
                pra.IsSMSAppointmentReminder = item.IsSMSAppointmentReminder;
                pra.isGoogleCalenderEnable = item.isGoogleCalenderEnable;
                pra.googleCalenderSecret = item.googleCalenderSecret;
                pra.googleSheetID = item.googleSheetID;
                pra.googleSheetSecret = item.googleSheetSecret;
                pra.CalenderID = item.CalenderID;
                pra.isGoogleSheetEnable = item.isGoogleSheetEnable;
                pra.GoogleSheetRows = item.GoogleSheetRows;
                _context_client.Practice.Add(pra);

                await _context_client.SaveChangesAsync();

                ////Code Comment In Case OF Solo Provider
                if (pra.Type.Equals("SP"))
                {
                    Location loc = new Location();
                    POS p = _context_client.POS.Where(v => v.Name.Equals("Office")).FirstOrDefault();

                    loc.Name = pra.Name;
                    loc.OrganizationName = pra.OrganizationName;
                    loc.PracticeID = pra.ID;
                    loc.NPI = pra.NPI;
                    loc.Address1 = pra.Address1;
                    loc.Address2 = pra.Address2;
                    loc.City = pra.City;
                    loc.State = pra.State;
                    loc.ZipCode = pra.ZipCode;
                    loc.CLIANumber = pra.CLIANumber;
                    loc.Fax = pra.FaxNumber;
                    loc.POSID = p.ID;   // Entering POS ID from value comming from quering POS
                    _context_client.Location.Add(loc);
                    Provider pro = new Provider();
                    pro.Name = pra.ProvLastName + ' ' + pra.ProvFirstName;
                    pro.LastName = pra.ProvLastName;
                    pro.FirstName = pra.ProvFirstName;
                    pro.MiddleInitial = pra.ProvMiddleInitial;
                    pro.NPI = pra.NPI;
                    pro.SSN = pra.SSN;
                    pro.TaxonomyCode = pra.TaxonomyCode;
                    pro.Address1 = pra.Address1;
                    pro.Address2 = pra.Address2;
                    pro.City = pra.City;
                    pro.State = pra.State;
                    pro.ZipCode = pra.ZipCode;
                    pro.OfficePhoneNum = pra.OfficePhoneNum;
                    pro.FaxNumber = pra.FaxNumber;
                    pro.PracticeID = pra.ID;
                    pro.Email = pra.Email;
                    _context_client.Provider.Add(pro);
                    await _context_client.SaveChangesAsync();
                }



                //Get Roles and UserId's  for Assign Practice to SuperAdmin & SuperUser
                var RolesUserId = (from rol in _context_main.Roles
                                   join urol in _context_main.UserRoles
                                   on rol.Id equals urol.RoleId
                                   //join usr in _context_client.Users
                                   //on urol.UserId equals usr.Id
                                   where rol.Name == "SuperAdmin" || rol.Name == "SuperUser"
                                   select new
                                   {
                                       RoleId = rol.Id,
                                       RoleName = rol.Name,
                                       UserId = urol.UserId
                                   }).ToList();

                //For Assign
                foreach (var items in RolesUserId)
                {
                    MainUserPractices userPractices = new MainUserPractices();
                    userPractices.UserID = items.UserId;
                    userPractices.AssignedByUserId = LoginUserId;
                    userPractices.PracticeID = item.ID;
                    userPractices.Status = true;
                    _context_main.MainUserPractices.Add(userPractices);
                    await _context_main.SaveChangesAsync();

                    // If First Practice Not Set
                    var AssUser = (from u in _context_main.Users
                                   where u.Id == items.UserId
                                   select u
                   ).FirstOrDefault();
                    //check Practice set or not
                    if (AssUser.PracticeID.IsNull())
                    {
                        AssUser.PracticeID = item.ID;
                        AssUser.ClientID = item.ClientID;
                        _context_main.Entry(AssUser).State = EntityState.Modified;
                        await _context_main.SaveChangesAsync();
                    }

                }



                //Get Roles and UserId's  for Assign Practice to SuperAdmin & SuperUser
                var clientRolesUserId = (from rol in _context_client.Roles
                                         join urol in _context_client.UserRoles
                                         on rol.Id equals urol.RoleId
                                         //join usr in _context_client.Users
                                         //on urol.UserId equals usr.Id
                                         where rol.Name == "SuperAdmin" || rol.Name == "SuperUser"
                                         select new
                                         {
                                             RoleId = rol.Id,
                                             RoleName = rol.Name,
                                             UserId = urol.UserId
                                         }).ToList();

                //For Assign
                foreach (var items in clientRolesUserId)
                {
                    UserPractices cliuserPractices = new UserPractices();
                    cliuserPractices.UserID = items.UserId;
                    cliuserPractices.AssignedByUserId = LoginUserId;
                    cliuserPractices.PracticeID = item.ID;
                    cliuserPractices.Status = true;
                    _context_client.UserPractices.Add(cliuserPractices);
                    await _context_client.SaveChangesAsync();

                    // If First Practice Not Set
                    var AssUser = (from u in _context_client.Users
                                   where u.Id == items.UserId
                                   select u
                   ).FirstOrDefault();
                    //check Practice set or not
                    if (AssUser.PracticeID.IsNull())
                    {
                        AssUser.PracticeID = item.ID;
                        AssUser.ClientID = item.ClientID;
                        _context_client.Entry(AssUser).State = EntityState.Modified;
                        await _context_client.SaveChangesAsync();
                    }
                }



            }
            else
            {
                bool UpdateProviderExistsUpdate = _context_client.Practice.Any(p => p.Name == item.Name && item.ID != p.ID);

                if (UpdateProviderExistsUpdate == true)
                {
                    return BadRequest("Practice With Same Name Already Exists");
                }
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context_main.MainPractice.Update(item);
                await _context_main.SaveChangesAsync();

                var pra = (from u in _context_client.Practice where u.ID == item.ID select u).FirstOrDefault();
                pra.ID = item.ID;
                pra.Name = item.Name;
                pra.ClientID = item.ClientID;
                pra.OrganizationName = item.OrganizationName;
                pra.TaxID = item.TaxID;
                pra.CLIANumber = item.CLIANumber;
                pra.NPI = item.NPI;
                pra.SSN = item.SSN;
                pra.Type = item.Type;
                pra.TaxonomyCode = item.TaxonomyCode;
                pra.Address1 = item.Address1;
                pra.Address2 = item.Address2;
                pra.City = item.City;
                pra.State = item.State;
                pra.ZipCode = item.ZipCode;
                pra.OfficePhoneNum = item.OfficePhoneNum;
                pra.FaxNumber = item.FaxNumber;
                pra.Email = item.Email;
                pra.Website = item.Website;
                pra.PayToAddress1 = item.PayToAddress1;
                pra.PayToAddress2 = item.PayToAddress2;
                pra.PayToCity = item.PayToCity;
                pra.PayToState = item.PayToState;
                pra.PayToZipCode = item.PayToZipCode;
                pra.DefaultLocationID = item.DefaultLocationID;
                pra.WorkingHours = item.WorkingHours;
                pra.Notes = item.Notes;
                pra.IsActive = item.IsActive;
                pra.IsDeleted = item.IsDeleted;
                pra.AddedBy = item.AddedBy;
                pra.AddedDate = item.AddedDate;
                pra.UpdatedBy = item.UpdatedBy;
                pra.UpdatedDate = item.UpdatedDate;
                pra.StatementExportType = item.StatementExportType;
                pra.StatementMessage = item.StatementMessage;
                pra.StatementAgingDays = item.StatementAgingDays;
                pra.StatementMaxCount = item.StatementMaxCount;
                //New Fields Are Added
                pra.CellNumber = item.CellNumber;
                pra.ContactPersonName = item.ContactPersonName;
                pra.InvoicePercentage = item.InvoicePercentage;
                pra.MinimumMonthlyAmount = item.MinimumMonthlyAmount;
                pra.NumberOfFullTimeEmployees = item.NumberOfFullTimeEmployees;
                pra.FTEPerDayRate = item.FTEPerDayRate;
                pra.FTEPerWeekRate = item.FTEPerWeekRate;
                pra.FTEPerMonthRate = item.FTEPerMonthRate;
                pra.IncludePatientCollection = item.IncludePatientCollection;
                pra.ClientCategory = item.ClientCategory;
                pra.RefferedBy = item.RefferedBy;
                pra.PMSoftwareName = item.PMSoftwareName;
                pra.EHRSoftwareName = item.EHRSoftwareName;
                pra.StatementPhoneNumber = item.StatementPhoneNumber;
                pra.StatementPhoneNumberExt = item.StatementPhoneNumberExt;
                pra.AppointmentPhoneNumber = item.AppointmentPhoneNumber;
                pra.AppointmentPhoneNumberExt = item.AppointmentPhoneNumberExt;
                pra.StatementFaxNumber = item.StatementFaxNumber;
                pra.IsAutoFollowup = item.IsAutoFollowup;
                pra.IsAutoDownloading = item.IsAutoDownloading;
                pra.IsAutoSubmission = item.IsAutoSubmission;
                pra.PLDDirectory = item.PLDDirectory;
                pra.IsEmailAppointmentReminder = item.IsEmailAppointmentReminder;
                pra.IsSMSAppointmentReminder = item.IsSMSAppointmentReminder;
                pra.isGoogleCalenderEnable = item.isGoogleCalenderEnable;
                pra.googleCalenderSecret = item.googleCalenderSecret;
                pra.googleSheetID = item.googleSheetID;
                pra.googleSheetSecret = item.googleSheetSecret;
                pra.CalenderID = item.CalenderID;
                pra.isGoogleSheetEnable = item.isGoogleSheetEnable;
                pra.GoogleSheetRows = item.GoogleSheetRows;

                _context_client.Practice.Update(pra);
                await _context_client.SaveChangesAsync();
            }
            return Ok(item);
        }


        [Route("DeletePractice/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePractice(long id)
        {
            var Practice = await _context_main.MainPractice.FindAsync(id);

            if (Practice == null)
            {
                return NotFound();
            }

            _context_main.MainPractice.Remove(Practice);
            await _context_main.SaveChangesAsync();

            return Ok();
        }


    }
}
