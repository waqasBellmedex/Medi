using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MediFusionPM.Models;
using MediFusionPM.ViewModel;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMUser;
using static MediFusionPM.ViewModels.VMRoleManager;
using static MediFusionPM.ViewModels.VMCommon;
using Microsoft.AspNetCore.Http;
using MediFusionPM.Models.Main;
using MediFusionPM.ViewModels.Main;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Net.Mail;
using System.Net;


//using Microsoft.IdentityModel.JsonWebTokens;
//using Microsoft.IdentityModel.Tokens;

namespace MediFusionPM.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<MainAuthIdentityCustom> _userManager;
        private readonly SignInManager<MainAuthIdentityCustom> _signInManager;
        private readonly IConfiguration _configuration;

        private readonly ClientDbContext _context_client;
        private readonly MainContext _context_main;
        private long PracticeId = 0;

        public AccountController(UserManager<MainAuthIdentityCustom> userManager, SignInManager<MainAuthIdentityCustom> signInManager, IConfiguration configuration, ClientDbContext contextClient, MainContext contextMain)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context_client = contextClient;
            _context_main = contextMain;

        }

        [Route("GetProfiles")]
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<VMUser>> GetProfiles()
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var CurrentLoginUser = (from u in _context_main.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();
            string UserId = CurrentLoginUser.Id;
            var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
            VMUser obj = new VMUser();
            obj.GetProfile(_context_main, UserId, RoleClaim.Value);
            return obj;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<Object> Login(MainAccountVM model)
                {
            MainAuthIdentityCustom us = new MainAuthIdentityCustom();
            us.Email = model.Email;

            if (!ModelState.IsValid)
            {
                return BadRequest("User Name or Password is invalid");
            }
            var q = (from u in _context_main.Users
                     where u.Email == model.Email
                     select u
                     ).FirstOrDefault();
            if (q.IsUserBlockByAdmin == false)
            {
                if (q.IsUserBlock == false)
                {
                    if (q != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            if (q.IsUserLogin == false)
                            {
                                var appUser = _userManager.Users.SingleOrDefault(m => m.Email == model.Email);
                                us.Id = appUser.Id;
                                var appUserRole = _userManager.GetRolesAsync(us).Result.SingleOrDefault() ?? "None";
                                q.LogInAttempts = 0;
                                // q.IsUserLogin = true;
                                q.BlockNote = "";
                                _context_main.Entry(q).State = EntityState.Modified;
                                _context_main.SaveChanges();

                                MainClient client = _context_main.MainClient.Find(appUser.ClientID);
                                MainPractice practice = _context_main.MainPractice.Find(appUser.PracticeID);

                                if (appUserRole == "SuperAdmin")
                                {

                                    var checkrights = (from rig in _context_main.MainRights
                                                     where rig.Id == appUser.Id
                                                     select rig
                                                    ).FirstOrDefault();
                                    if (checkrights == null)
                                    {
                                        MainRights chkRights = new MainRights();


                                        //Scheduler Rights false;
                                        chkRights.SchedulerCreate = true;
                                        chkRights.SchedulerEdit = true;
                                        chkRights.SchedulerDelete = true;
                                        chkRights.SchedulerSearch = true;
                                        chkRights.SchedulerImport = true;
                                        chkRights.SchedulerExport = true;

                                        //Patient Rights
                                        chkRights.PatientCreate = true;
                                        chkRights.PatientEdit = true;
                                        chkRights.PatientDelete = true;
                                        chkRights.PatientSearch = true;
                                        chkRights.PatientImport = true;
                                        chkRights.PatientExport = true;

                                        //Charges Rights
                                        chkRights.ChargesCreate = true;
                                        chkRights.ChargesEdit = true;
                                        chkRights.ChargesDelete = true;
                                        chkRights.ChargesSearch = true;
                                        chkRights.ChargesImport = true;
                                        chkRights.ChargesExport = true;

                                        //Documents Rights
                                        chkRights.DocumentsCreate = true;
                                        chkRights.DocumentsEdit = true;
                                        chkRights.DocumentsDelete = true;
                                        chkRights.DocumentsSearch = true;
                                        chkRights.DocumentsImport = true;
                                        chkRights.DocumentsExport = true;

                                        //Submissions Rights
                                        chkRights.SubmissionsCreate = true;
                                        chkRights.SubmissionsEdit = true;
                                        chkRights.SubmissionsDelete = true;
                                        chkRights.SubmissionsSearch = true;
                                        chkRights.SubmissionsImport = true;
                                        chkRights.SubmissionsExport = true;

                                        //Payments Rights
                                        chkRights.PaymentsCreate = true;
                                        chkRights.PaymentsEdit = true;
                                        chkRights.PaymentsDelete = true;
                                        chkRights.PaymentsSearch = true;
                                        chkRights.PaymentsImport = true;
                                        chkRights.PaymentsExport = true;

                                        //Followup Rights
                                        chkRights.FollowupCreate = true;
                                        chkRights.FollowupEdit = true;
                                        chkRights.FollowupDelete = true;
                                        chkRights.FollowupSearch = true;
                                        chkRights.FollowupImport = true;
                                        chkRights.FollowupExport = true;

                                        //Reports Rights
                                        chkRights.ReportsCreate = true;
                                        chkRights.ReportsEdit = true;
                                        chkRights.ReportsDelete = true;
                                        chkRights.ReportsSearch = true;
                                        chkRights.ReportsImport = true;
                                        chkRights.ReportsExport = true;


                                        ////SetUp Client 
                                        //Client Rights
                                        chkRights.ClientCreate = true;
                                        chkRights.ClientEdit = true;
                                        chkRights.ClientDelete = true;
                                        chkRights.ClientSearch = true;
                                        chkRights.ClientImport = true;
                                        chkRights.ClientExport = true;

                                        chkRights.UserCreate = true;
                                        chkRights.UserEdit = true;
                                        chkRights.UserDelete = true;
                                        chkRights.UserSearch = true;
                                        chkRights.UserImport = true;
                                        chkRights.UserExport = true;

                                        ////SetUp Admin 
                                        //Practice
                                        chkRights.PracticeCreate = true;
                                        chkRights.PracticeEdit = true;
                                        chkRights.PracticeDelete = true;
                                        chkRights.PracticeSearch = true;
                                        chkRights.PracticeImport = true;
                                        chkRights.PracticeExport = true;

                                        //Location
                                        chkRights.LocationCreate = true;
                                        chkRights.LocationEdit = true;
                                        chkRights.LocationDelete = true;
                                        chkRights.LocationSearch = true;
                                        chkRights.LocationImport = true;
                                        chkRights.LocationExport = true;

                                        //Provider
                                        chkRights.ProviderCreate = true;
                                        chkRights.ProviderEdit = true;
                                        chkRights.ProviderDelete = true;
                                        chkRights.ProviderSearch = true;
                                        chkRights.ProviderImport = true;
                                        chkRights.ProviderExport = true;


                                        //Referring Provider
                                        chkRights.ReferringProviderCreate = true;
                                        chkRights.ReferringProviderEdit = true;
                                        chkRights.ReferringProviderDelete = true;
                                        chkRights.ReferringProviderSearch = true;
                                        chkRights.ReferringProviderImport = true;
                                        chkRights.ReferringProviderExport = true;

                                        //Setup Insurance
                                        //Insurance

                                        chkRights.InsuranceCreate = true;
                                        chkRights.InsuranceEdit = true;
                                        chkRights.InsuranceDelete = true;
                                        chkRights.InsuranceSearch = true;
                                        chkRights.InsuranceImport = true;
                                        chkRights.InsuranceExport = true;

                                        //Insurance Plan 
                                        chkRights.InsurancePlanCreate = true;
                                        chkRights.InsurancePlanEdit = true;
                                        chkRights.InsurancePlanDelete = true;
                                        chkRights.InsurancePlanSearch = true;
                                        chkRights.InsurancePlanImport = true;
                                        chkRights.InsurancePlanExport = true;

                                        //Insurance Plan Address 
                                        chkRights.InsurancePlanAddressCreate = true;
                                        chkRights.InsurancePlanAddressEdit = true;
                                        chkRights.InsurancePlanAddressDelete = true;
                                        chkRights.InsurancePlanAddressSearch = true;
                                        chkRights.InsurancePlanAddressImport = true;
                                        chkRights.InsurancePlanAddressExport = true;

                                        //EDI Submit
                                        chkRights.EDISubmitCreate = true;
                                        chkRights.EDISubmitEdit = true;
                                        chkRights.EDISubmitDelete = true;
                                        chkRights.EDISubmitSearch = true;
                                        chkRights.EDISubmitImport = true;
                                        chkRights.EDISubmitExport = true;

                                        //EDI EligiBility
                                        chkRights.EDIEligiBilityCreate = true;
                                        chkRights.EDIEligiBilityEdit = true;
                                        chkRights.EDIEligiBilityDelete = true;
                                        chkRights.EDIEligiBilitySearch = true;
                                        chkRights.EDIEligiBilityImport = true;
                                        chkRights.EDIEligiBilityExport = true;

                                        //EDI Status
                                        chkRights.EDIStatusCreate = true;
                                        chkRights.EDIStatusEdit = true;
                                        chkRights.EDIStatusDelete = true;
                                        chkRights.EDIStatusSearch = true;
                                        chkRights.EDIStatusImport = true;
                                        chkRights.EDIStatusExport = true;

                                        //Coding
                                        //ICD
                                        chkRights.ICDCreate = true;
                                        chkRights.ICDEdit = true;
                                        chkRights.ICDDelete = true;
                                        chkRights.ICDSearch = true;
                                        chkRights.ICDImport = true;
                                        chkRights.ICDExport = true;

                                        //CPT
                                        chkRights.CPTCreate = true;
                                        chkRights.CPTEdit = true;
                                        chkRights.CPTDelete = true;
                                        chkRights.CPTSearch = true;
                                        chkRights.CPTImport = true;
                                        chkRights.CPTExport = true;

                                        //Modifiers
                                        chkRights.ModifiersCreate = true;
                                        chkRights.ModifiersEdit = true;
                                        chkRights.ModifiersDelete = true;
                                        chkRights.ModifiersSearch = true;
                                        chkRights.ModifiersImport = true;
                                        chkRights.ModifiersExport = true;

                                        //POS
                                        chkRights.POSCreate = true;
                                        chkRights.POSEdit = true;
                                        chkRights.POSDelete = true;
                                        chkRights.POSSearch = true;
                                        chkRights.POSImport = true;
                                        chkRights.POSExport = true;

                                        //EDI Codes
                                        //Claim Status Category Codes
                                        chkRights.ClaimStatusCategoryCodesCreate = true;
                                        chkRights.ClaimStatusCategoryCodesEdit = true;
                                        chkRights.ClaimStatusCategoryCodesDelete = true;
                                        chkRights.ClaimStatusCategoryCodesSearch = true;
                                        chkRights.ClaimStatusCategoryCodesImport = true;
                                        chkRights.ClaimStatusCategoryCodesExport = true;

                                        //Claim Status Codes
                                        chkRights.ClaimStatusCodesCreate = true;
                                        chkRights.ClaimStatusCodesEdit = true;
                                        chkRights.ClaimStatusCodesDelete = true;
                                        chkRights.ClaimStatusCodesSearch = true;
                                        chkRights.ClaimStatusCodesImport = true;
                                        chkRights.ClaimStatusCodesExport = true;

                                        //Adjustment Codes
                                        chkRights.AdjustmentCodesCreate = true;
                                        chkRights.AdjustmentCodesEdit = true;
                                        chkRights.AdjustmentCodesDelete = true;
                                        chkRights.AdjustmentCodesSearch = true;
                                        chkRights.AdjustmentCodesImport = true;
                                        chkRights.AdjustmentCodesExport = true;

                                        //Remark Codes
                                        chkRights.RemarkCodesCreate = true;
                                        chkRights.RemarkCodesEdit = true;
                                        chkRights.RemarkCodesDelete = true;
                                        chkRights.RemarkCodesSearch = true;
                                        chkRights.RemarkCodesImport = true;
                                        chkRights.RemarkCodesExport = true;

                                        //Team Rights
                                        chkRights.teamCreate = true;
                                        chkRights.teamupdate = true;
                                        chkRights.teamDelete = true;
                                        chkRights.teamSearch = true;
                                        chkRights.teamExport = true;
                                        chkRights.teamImport = true;

                                        //Receiver Rights
                                        chkRights.receiverCreate = true;
                                        chkRights.receiverupdate = true;
                                        chkRights.receiverDelete = true;
                                        chkRights.receiverSearch = true;
                                        chkRights.receiverExport = true;
                                        chkRights.receiverImport = true;

                                        //Submitter Rights
                                        chkRights.submitterCreate = true;
                                        chkRights.submitterUpdate = true;
                                        chkRights.submitterDelete = true;
                                        chkRights.submitterSearch = true;
                                        chkRights.submitterExport = true;
                                        chkRights.submitterImport = true;

                                        //PatientPlan Rights
                                        chkRights.patientPlanCreate = true;
                                        chkRights.patientPlanUpdate = true;
                                        chkRights.patientPlanDelete = true;
                                        chkRights.patientPlanSearch = true;
                                        chkRights.patientPlanExport = true;
                                        chkRights.patientPlanImport = true;
                                        chkRights.performEligibility = true;

                                        //PatientPayment Rights
                                        chkRights.patientPaymentCreate = true;
                                        chkRights.patientPaymentUpdate = true;
                                        chkRights.patientPaymentDelete = true;
                                        chkRights.patientPaymentSearch = true;
                                        chkRights.patientPaymentExport = true;
                                        chkRights.patientPaymentImport = true;

                                        // Charges Rights
                                        chkRights.resubmitCharges = true;
                                        // BatchDocument Rights
                                        chkRights.batchdocumentCreate = true;
                                        chkRights.batchdocumentUpdate = true;
                                        chkRights.batchdocumentDelete = true;
                                        chkRights.batchdocumentSearch = true;
                                        chkRights.batchdocumentExport = true;
                                        chkRights.batchdocumentImport = true;

                                        // ElectronicSubmission Rights
                                        chkRights.electronicsSubmissionSearch = true;
                                        chkRights.electronicsSubmissionSubmit = true;
                                        chkRights.electronicsSubmissionResubmit = true;

                                        // PaperSubmission Rights
                                        chkRights.paperSubmissionSearch = true;
                                        chkRights.paperSubmissionSubmit = true;
                                        chkRights.paperSubmissionResubmit = true;
                                        // SubmissionLog Rights	
                                        chkRights.submissionLogSearch = true;
                                        // PlanFollowup Rights	
                                        chkRights.planFollowupSearch = true;
                                        chkRights.planFollowupCreate = true;
                                        chkRights.planFollowupDelete = true;
                                        chkRights.planFollowupUpdate = true;
                                        chkRights.planFollowupImport = true;
                                        chkRights.planFollowupExport = true;

                                        // PatientFollowup Rights	
                                        chkRights.patientFollowupSearch = true;
                                        chkRights.patientFollowupCreate = true;
                                        chkRights.patientFollowupDelete = true;
                                        chkRights.patientFollowupUpdate = true;
                                        chkRights.patientFollowupImport = true;
                                        chkRights.patientFollowupExport = true;

                                        // Group Rights	
                                        chkRights.groupSearch = true;
                                        chkRights.groupCreate = true;
                                        chkRights.groupUpdate = true;
                                        chkRights.groupDelete = true;
                                        chkRights.groupExport = true;
                                        chkRights.groupImport = true;
                                        // Reason Rights	
                                        chkRights.reasonSearch = true;
                                        chkRights.reasonCreate = true;
                                        chkRights.reasonUpdate = true;
                                        chkRights.reasonDelete = true;
                                        chkRights.reasonExport = true;
                                        chkRights.reasonImport = true;

                                        chkRights.addPaymentVisit = true;
                                        chkRights.DeleteCheck = true;
                                        chkRights.ManualPosting = true;
                                        chkRights.Postcheck = true;
                                        chkRights.PostExport = true;
                                        chkRights.PostImport = true;

                                        chkRights.ManualPostingAdd = true;
                                        chkRights.ManualPostingUpdate = true;
                                        chkRights.PostCheckSearch = true;
                                        chkRights.DeletePaymentVisit = true;

                                        chkRights.Id = appUser.Id;
                                        chkRights.AddedBy = model.Email;
                                        _context_main.MainRights.Add(chkRights);
                                        _context_main.SaveChanges();
                                    }
                                }


                                //check Client Id exist or not
                                var cli = (from u in _context_main.MainClient
                                           where u.ID == appUser.ClientID
                                           select u).FirstOrDefault();

                                if (cli != null)
                                {
                                    _context_client.setDatabaseName(cli.ContextName);
                                    _context_client.Database.Migrate();



                                    string ALLNewInserts = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "ALLNewInserts.sql"));
                                    string ALLNewTrigger = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "ALLNewTriggers.sql"));
                                    string connectionString = _configuration.GetConnectionString("MedifusionLocal");
                                    string[] splitString = connectionString.Split(';');
                                    splitString[1] = splitString[1];
                                    connectionString = splitString[0] + "; " + splitString[1] + cli.ContextName + "; " + splitString[2] + "; " + splitString[3];
                                    SqlConnection conn = new SqlConnection(connectionString);
                                    Server server = new Server(new ServerConnection(conn));

                                    DateTime LastModifiedInsertDate = System.IO.File.GetLastWriteTimeUtc(Path.Combine(_context_client.env.ContentRootPath, "Resources", "ALLNewInserts.sql"));
                                    DateTime LastModifiedTriggerDate = System.IO.File.GetLastWriteTimeUtc(Path.Combine(_context_client.env.ContentRootPath, "Resources", "ALLNewTriggers.sql"));

                                    if (!(cli.LastNewInsertsModifiedDate == null || cli.LastNewInsertsModifiedDate == LastModifiedInsertDate))
                                    {
                                        server.ConnectionContext.ExecuteNonQuery(ALLNewInserts);
                                    }
                                    if (!(cli.LastNewTrigerModifiedDate == null || cli.LastNewTrigerModifiedDate == LastModifiedTriggerDate))
                                    {
                                        server.ConnectionContext.ExecuteNonQuery(ALLNewTrigger);
                                    }


                                    cli.LastNewInsertsModifiedDate = LastModifiedInsertDate;
                                    cli.LastNewTrigerModifiedDate = LastModifiedTriggerDate;
                                    _context_main.MainClient.Update(cli);
                                    _context_main.SaveChanges();

                                    //check Client Id exist or not
                                    var ClientExists = (from u in _context_client.Client where u.ID == appUser.ClientID select u).FirstOrDefault();
                                    ClientExists.Name = cli.Name;
                                    ClientExists.OrganizationName = cli.OrganizationName;
                                    ClientExists.TaxID = cli.TaxID;
                                    ClientExists.ServiceLocation = cli.ServiceLocation;
                                    ClientExists.Address = cli.Address;
                                    ClientExists.State = cli.State;
                                    ClientExists.City = cli.City;
                                    ClientExists.ZipCode = cli.ZipCode;
                                    ClientExists.OfficeHour = cli.OfficeHour;
                                    ClientExists.FaxNo = cli.FaxNo;
                                    ClientExists.OfficePhoneNo = cli.OfficePhoneNo;
                                    ClientExists.OfficeEmail = cli.OfficeEmail;
                                    ClientExists.ContactPerson = cli.ContactPerson;
                                    ClientExists.ContactNo = cli.ContactNo;
                                    ClientExists.UpdatedBy = cli.UpdatedBy;
                                    ClientExists.UpdatedDate = cli.UpdatedDate;
                                    ClientExists.LastNewInsertsModifiedDate = cli.LastNewInsertsModifiedDate;
                                    ClientExists.LastNewTrigerModifiedDate = cli.LastNewTrigerModifiedDate;
                                    _context_client.Entry(ClientExists).State = EntityState.Modified;
                                    await _context_client.SaveChangesAsync();
                                }


                                //return GenerateJwtToken(model.Email, appUser, appUserRole, client?.ContextName);

                                MainUserLoginHistory LH = new MainUserLoginHistory();
                                LH.UserId = appUser.Id;
                                LH.TokenId = "";
                                LH.AddedDate = DateTime.Now;
                                LH.LastActivityTime = DateTime.Now;
                                LH.Status = true;
                                _context_main.MainUserLoginHistory.Add(LH);
                                _context_main.SaveChanges();
                                var LogHis = (from u in _context_main.MainUserLoginHistory where u.Id == LH.Id select u).FirstOrDefault();
                                var token = GenerateJwtToken(LH.Id, model.Email, appUser, appUserRole, client?.ContextName, practice?.ID);
                                LogHis.TokenId = token.ToString();
                                _context_main.MainUserLoginHistory.Update(LogHis);
                                _context_main.SaveChanges();
                                return token;
                            }
                            else
                            {
                                q.LogInAttempts = 0;
                                q.BlockNote = "";
                                _context_main.Entry(q).State = EntityState.Modified;
                                _context_main.SaveChanges();
                                return BadRequest("This Account has been already running on other Browser / Location.");
                            }

                        }
                        else
                        {

                            int count = 2 - q.LogInAttempts;
                            if (count >= 0)
                            {
                                q.LogInAttempts = q.LogInAttempts + 1;
                                q.BlockNote = "Due to Invalid Password. You have " + (count + 1) + " Attempts Left.";
                                _context_main.Entry(q).State = EntityState.Modified;
                                _context_main.SaveChanges();
                                return BadRequest("Due to Invalid Password. You have " + (count + 1) + " Attempts Left.");
                            }
                            else
                            {
                                q.LogInAttempts = q.LogInAttempts + 1;
                                q.IsUserBlock = true;
                                q.BlockNote = "Due to Invalid Password. Account Blocked.";
                                _context_main.Entry(q).State = EntityState.Modified;
                                _context_main.SaveChanges();
                                return BadRequest("Due to Invalid Password. Account Blocked.");
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid Email Address.");
                    }
                }
                else
                {
                    return BadRequest(q.BlockNote);
                }
            }
            else
            {
                return BadRequest("Temporary Message  " + q.BlockNote);
            }
        }


        [HttpPost]
        [Route("CreateAccount")]
        [Authorize]
        public async Task<ActionResult<MainAccountVM>> CreateAccount(MainAccountVM model)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (ModelState.IsValid)
            {
                //Validation
                string ErrorMsg = string.Empty;
                if (model.Email.IsNull()) ErrorMsg = "Email is required";
                else if (model.ClientID.IsNull()) ErrorMsg = "Client is requird";
                else if (model.PracticeID.IsNull()) ErrorMsg = "Practice is required";
                else if (model.UserRole.IsNull()) ErrorMsg = "User Role is required";

                if (!ErrorMsg.IsNull())
                    return BadRequest(ErrorMsg);
                //get Login User Id
                var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));

                MainUserPractices mainUserPractices = new MainUserPractices();
                UserPractices userPractices = new UserPractices();
                //____________________________________________________________________
                //check Client Id exist or not ..... for dbName set in Connection String
                var q = (from u in _context_main.MainClient
                         where u.ID == model.ClientID
                         select u).FirstOrDefault();
                if (q == null) return BadRequest("Client not found");
                _context_client.setDatabaseName(q.ContextName);
                //____________________________________________________________________

                //Check this Email already Exist or not
                var UserProfile = (from u in _context_main.Users
                                   where u.Email == model.Email
                                   select u).FirstOrDefault();
                //check Reporting To Email is Exist or not
                var ReportingToId = (from u in _context_main.Users
                                     where u.Email == model.ReportingTo
                                     select u).FirstOrDefault();
                if (model.Piccontent != null && model.Piccontent != "" )
                {
                    Models.Settings settings = _context_client.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
                    string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
              settings.DocumentServerDirectory, UD.PracticeID.ToString(), "Signature");//settings.DocumentServerURL
                    string picextension;
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }

                    var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                    string HTMLfileName = "";
                    if (model.Piccontent.Contains("Jpeg"))
                        picextension = ".Jpeg";
                    else if (model.Piccontent.Contains("png"))
                        picextension = ".PNG";
                    else
                        picextension = ".Jpg";

                    HTMLfileName = System.IO.Path.Combine(DirectoryPath, model.Name + "_" + datetime + picextension).Replace(" ", "");


                    //string StatmentHtml = Encoding.UTF8.GetString(Convert.FromBase64String((model.Piccontent)));//.Remove(0, 22)
                    //System.IO.File.WriteAllText(HTMLfileName, StatmentHtml);
                    string base64String = model.Piccontent.Split(',').Last();
                    System.IO.File.WriteAllBytes(HTMLfileName, Convert.FromBase64String(base64String));
                    model.signatureURL = (model.Name + "_" + datetime + picextension).Replace(" ", "");
                }
                //If User Email not Exist Than Create
                if (UserProfile == null)
                {
                    var user = new MainAuthIdentityCustom();
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.ClientID = model.ClientID;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Name = model.Name;
                    user.PracticeID = model.PracticeID;
                    user.LogInAttempts = 0;
                    user.IsUserLogin = false;
                    user.IsUserBlockByAdmin = false;
                    user.TeamID = model.TeamID;
                    user.DesignationID = model.DesignationID;

                    user.ReportingTo = model?.ReportingTo;
                //    user.RefUserID = UserId.Value;

                    var createUser = await _userManager.CreateAsync(user, model.Password);
                    if (createUser.Succeeded)
                    {
                        //Check this user Exist in Client Table or not
                        var clientUser = (from u in _context_client.Users
                                          where u.Email == model.Email
                                          select u
                        ).FirstOrDefault();

                        // get User Email Data from Main Database for Client user Table
                        var mUser = (from u in _context_main.Users
                                     where u.Email == model.Email
                                     select u
                                   ).FirstOrDefault();

                        if (clientUser == null)
                        {
                            if (!model.TeamID.IsNull())
                            {
                                var CheckClientTeam = (from ct in _context_client.Team
                                                       where ct.ID == model.TeamID
                                                       select ct
                                                  ).FirstOrDefault();
                                if (CheckClientTeam == null)
                                {
                                    var CheckMainTeam = (from ct in _context_main.MainTeam
                                                         where ct.ID == model.TeamID
                                                         select ct
                                                       ).FirstOrDefault();
                                    Team t = new Team();
                                    t.ID = CheckMainTeam.ID;
                                    t.Name = CheckMainTeam.Name;
                                    t.Details = CheckMainTeam.Details;
                                    t.AddedBy = CheckMainTeam.AddedBy;
                                    t.AddedDate = CheckMainTeam.AddedDate;
                                    t.UpdatedBy = CheckMainTeam.UpdatedBy;
                                    t.UpdatedDate = CheckMainTeam.UpdatedDate;
                                    _context_client.Team.Add(t);
                                    await _context_client.SaveChangesAsync();
                                }
                            }
                            if (!model.DesignationID.IsNull())
                            {
                                var CheckClientDesignation = (from ct in _context_client.Designations
                                                              where ct.ID == model.DesignationID
                                                              select ct
                                                  ).FirstOrDefault();
                                if (CheckClientDesignation == null)
                                {
                                    var CheckMainDesignation = (from ct in _context_main.MainDesignations
                                                                where ct.ID == model.DesignationID
                                                                select ct
                                                       ).FirstOrDefault();
                                    Designations des = new Designations();
                                    des.ID = CheckMainDesignation.ID;
                                    des.Name = CheckMainDesignation.Name;
                                    des.AddedBy = CheckMainDesignation.AddedBy;
                                    des.AddedDate = CheckMainDesignation.AddedDate;
                                    des.UpdatedBy = CheckMainDesignation.UpdatedBy;
                                    des.UpdatedDate = CheckMainDesignation.UpdatedDate;
                                    _context_client.Designations.Add(des);
                                    await _context_client.SaveChangesAsync();
                                }
                            }

                            AuthIdentityCustom usr = new AuthIdentityCustom();
                            usr.Id = mUser.Id;
                            usr.UserName = mUser.UserName;
                            usr.NormalizedUserName = mUser.NormalizedUserName;
                            usr.Email = mUser.Email;
                            usr.NormalizedEmail = mUser.NormalizedEmail;
                            usr.EmailConfirmed = mUser.EmailConfirmed;
                            usr.PasswordHash = mUser.PasswordHash;
                            usr.SecurityStamp = mUser.SecurityStamp;
                            usr.ConcurrencyStamp = mUser.ConcurrencyStamp;
                            usr.PhoneNumber = mUser.PhoneNumber;
                            usr.PhoneNumberConfirmed = mUser.PhoneNumberConfirmed;
                            usr.TwoFactorEnabled = mUser.TwoFactorEnabled;
                            usr.LockoutEnd = mUser.LockoutEnd;
                            usr.LockoutEnabled = mUser.LockoutEnabled;
                            usr.AccessFailedCount = mUser.AccessFailedCount;
                            usr.FirstName = mUser.FirstName;
                            usr.LastName = mUser.LastName;
                            usr.Name = mUser.Name;
                            usr.IsUserLogin = mUser.IsUserLogin;
                            usr.LogInAttempts = mUser.LogInAttempts;
                            usr.IsUserBlock = mUser.IsUserBlock;
                            usr.IsUserBlockByAdmin = mUser.IsUserBlockByAdmin;
                            usr.BlockNote = mUser.BlockNote;
                            usr.ClientID = mUser.ClientID;
                            usr.PracticeID = mUser.PracticeID;
                            usr.TeamID = mUser.TeamID;
                            usr.DesignationID = mUser.DesignationID;
                            usr.ReportingTo = mUser.ReportingTo;
                          //  usr.RefUserID = mUser.RefUserID;
                            _context_client.Users.Add(usr);
                            await _context_client.SaveChangesAsync();
                        }



                        var addToRole = await _userManager.AddToRoleAsync(user, model.UserRole);

                        if (addToRole.Succeeded)
                        {
                            var clientUserRoles = (from urol in _context_client.UserRoles
                                                   where urol.UserId == mUser.Id
                                                   select urol).FirstOrDefault();
                            if (clientUserRoles == null)
                            {
                                var UserRoles = (from urol in _context_main.UserRoles
                                                 where urol.UserId == user.Id
                                                 select urol).FirstOrDefault();
                                _context_client.UserRoles.Add(UserRoles);
                                await _context_client.SaveChangesAsync();
                            }
                            if (model.UserRole == "SuperAdmin" || model.UserRole == "SuperUser")
                            {
                                //Verify this code start
                                var PracticeList = (from pra in _context_main.MainPractice select pra).ToList();
                                foreach (var item in PracticeList)
                                {
                                    mainUserPractices.UserID = user.Id;
                                    mainUserPractices.AssignedByUserId = UserId.Value.ToString();
                                    mainUserPractices.PracticeID = item.ID;
                                    mainUserPractices.Status = true;
                                    _context_main.MainUserPractices.Add(mainUserPractices);
                                    _context_main.SaveChanges();
                                }


                                var ClientPracticeList = (from pra in _context_client.Practice select pra).ToList();
                                foreach (var item in ClientPracticeList)
                                {
                                    userPractices.UserID = user.Id;
                                    userPractices.AssignedByUserId = UserId.Value.ToString();
                                    userPractices.PracticeID = item.ID;
                                    userPractices.Status = true;
                                    _context_client.UserPractices.Add(userPractices);
                                    _context_client.SaveChanges();
                                }
                                //Verify this code End

                            }
                            else
                            {
                                mainUserPractices.UserID = user.Id;
                                mainUserPractices.AssignedByUserId = UserId.Value.ToString();
                                mainUserPractices.PracticeID = model.PracticeID;
                                mainUserPractices.Status = true;
                                _context_main.MainUserPractices.Add(mainUserPractices);
                                _context_main.SaveChanges();

                                userPractices.UserID = user.Id;
                                userPractices.AssignedByUserId = UserId.Value.ToString();
                                userPractices.PracticeID = model.PracticeID;
                                userPractices.Status = true;
                                _context_client.UserPractices.Add(userPractices);
                                _context_client.SaveChanges();
                            }


                            var rig = MainCreateRights(user.Id, model.UserRole);

                            var CliRights = (from rights in _context_client.Rights where rights.Id == user.Id select rights).FirstOrDefault();
                            if (CliRights == null)
                            {
                                var Rights = ClientCreateRights(user.Id);
                            }

                            //if (rig.ToString()!="OK") return Ok("User Created Successfully. Butt Something Wrong with Rights.");
                            return Ok("User Created Successfully");
                        }
                        else
                        {
                            return BadRequest("User role not assigned. Contact Bell Medex.");
                        }
                    }
                    else
                    {
                        return BadRequest("User not created. Contact Bell Medex.");
                    }
                }
                else
                {
                    //Check this Email already Exist or not
                    var ClientUserProfile = (from u in _context_client.Users
                                             where u.Email == model.Email
                                             select u).FirstOrDefault();

                    var OldRoleId = (from u in _context_main.Users
                                     join v in _context_main.UserRoles
                                     on u.Id equals v.UserId
                                     join w in _context_main.Roles
                                     on v.RoleId equals w.Id
                                     where u.Id == UserProfile.Id
                                     select new
                                     {
                                         w.Id,
                                         w.Name
                                     }).FirstOrDefault();
                    if (OldRoleId !=null && OldRoleId.Name != model.UserRole)
                    {
                        var upRole = (from u in _context_main.UserRoles
                                      where u.RoleId == OldRoleId.Id && u.UserId == UserProfile.Id
                                      select u
                                     ).FirstOrDefault();
                        _context_main.UserRoles.Remove(upRole);
                        await _context_main.SaveChangesAsync();
                        _context_client.UserRoles.Remove(upRole);
                        await _context_client.SaveChangesAsync();
                        var newRoleId = (from v in _context_main.Roles
                                         where v.Name == model.UserRole
                                         select new
                                         {
                                             v.Id
                                         }).FirstOrDefault();
                        upRole.RoleId = newRoleId.Id;
                        upRole.UserId = UserProfile.Id;
                        _context_main.UserRoles.Add(upRole);
                        await _context_main.SaveChangesAsync();
                        _context_client.UserRoles.Add(upRole);
                        await _context_client.SaveChangesAsync();


                         
                        if (model.UserRole == "Manager")
                        {
                            UserProfile.FirstName = model.FirstName;
                            UserProfile.LastName = model.LastName;
                            UserProfile.TeamID = model.TeamID;
                            UserProfile.DesignationID = model.DesignationID;
                            UserProfile.ReportingTo = "";
                           // UserProfile.RefUserID = UserProfile.Id;
                            _context_main.Entry(UserProfile).State = EntityState.Modified;
                            await _context_main.SaveChangesAsync();

                            ClientUserProfile.FirstName = model.FirstName;
                            ClientUserProfile.LastName = model.LastName;
                            ClientUserProfile.TeamID = model.TeamID;
                            ClientUserProfile.DesignationID = model.DesignationID;
                            ClientUserProfile.ReportingTo = "";
                            _context_client.Entry(ClientUserProfile).State = EntityState.Modified;
                            await _context_client.SaveChangesAsync();
                        }
                        else
                        {
                            UserProfile.FirstName = model.FirstName;
                            UserProfile.LastName = model.LastName;
                            UserProfile.TeamID = model.TeamID;
                            UserProfile.DesignationID = model.DesignationID;
                            UserProfile.ReportingTo = model.ReportingTo;
                       //     UserProfile.RefUserID = UserProfile.Id;
                            _context_main.Entry(UserProfile).State = EntityState.Modified;
                            await _context_main.SaveChangesAsync();


                            ClientUserProfile.FirstName = model.FirstName;
                            ClientUserProfile.LastName = model.LastName;
                            ClientUserProfile.TeamID = model.TeamID;
                            ClientUserProfile.DesignationID = model.DesignationID;
                            ClientUserProfile.ReportingTo = model.ReportingTo;
                            _context_client.Entry(ClientUserProfile).State = EntityState.Modified;
                            await _context_client.SaveChangesAsync();

                        }

                    }
                    else
                    {
                        UserProfile.FirstName = model.FirstName;
                        UserProfile.LastName = model.LastName;
                        UserProfile.TeamID = model.TeamID;
                        UserProfile.DesignationID = model.DesignationID;
                        UserProfile.ReportingTo = model.ReportingTo;
                      //  UserProfile.RefUserID = UserProfile.Id;
                        _context_main.Entry(UserProfile).State = EntityState.Modified;
                            await _context_main.SaveChangesAsync();
                        ClientUserProfile.FirstName = model.FirstName;
                        ClientUserProfile.LastName = model.LastName;
                        ClientUserProfile.TeamID = model.TeamID;
                        ClientUserProfile.DesignationID = model.DesignationID;
                        ClientUserProfile.ReportingTo = model.ReportingTo;
                        _context_client.Entry(ClientUserProfile).State = EntityState.Modified;
                        await _context_client.SaveChangesAsync();
                    }
                    return Ok(model);
                    // return  OK (model);
                  //  return Ok("Updated Successfully");
                }

            }
            return BadRequest("Unexpected Error Occurred. Contact Bell Medex.");
        }


        private object GenerateJwtToken(int Id,string email, MainAuthIdentityCustom appUser, string appUserRole, string contextName, long? PracticeID)
        {
            List<Claim> claims=null;
            if (contextName.IsNull())
            {
                claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Jti, appUser.Id),
                new Claim("Role",appUserRole),
                new Claim("UserName",appUser.UserName),
                new Claim("LH",Id.ToString()),
                new Claim("RandomKEY", PracticeID.ToString())
                };
            }
            else
            {
                claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Jti, appUser.Id),
                new Claim("Role",appUserRole),
                new Claim("UserName",appUser.UserName),
                new Claim("TEMP", contextName),
                new Claim("LH",Id.ToString()),
                new Claim("RandomKEY", PracticeID.ToString())

            };
            }
          


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT-Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT-Expiry"]));


            var token = new JwtSecurityToken(
                issuer: _configuration["JWT-Issuer"],
                audience: _configuration["JWT-Issuer"],
                claims: claims,
                expires: expire,
                signingCredentials: cred
                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }



        [HttpPost]
        [Route("FindUsers")]
        public async Task<ActionResult<IEnumerable<GUser>>> FindUsers(CUser CUser)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );



            List<GUser> gus = new List<GUser>();

            var List = (from cli in _context_main.MainClient
                        join pra in _context_main.MainPractice on cli.ID equals pra.ClientID
                        join up in _context_main.MainUserPractices on pra.ID equals up.PracticeID
                        join usr in _context_main.Users on up.UserID equals usr.Id
                        join w in _context_main.UserRoles on usr.Id equals w.UserId
                        join x in _context_main.Roles on w.RoleId equals x.Id
                        join te in _context_main.MainTeam on usr.TeamID equals te.ID into Table1 from t1 in Table1.DefaultIfEmpty()
                        join des in _context_main.MainDesignations on usr.DesignationID equals des.ID into Table2 from t2 in Table2.DefaultIfEmpty()
                        where //up.Status == true &&
                        (CUser.LastName.IsNull() ? true : usr.LastName.Contains(CUser.LastName))
                      &&(CUser.FirstName.IsNull() ? true : usr.FirstName.Contains(CUser.FirstName))
                      &&(CUser.Email.IsNull() ? true : usr.Email.Equals(CUser.Email)) 
                      &&(CUser.ClientID.IsNull() ? true : cli.ID.Equals(CUser.ClientID)) 
                      &&(CUser.TeamID.IsNull() ? true : t1.ID.Equals(CUser.TeamID))
                        select new GUser()
                        {
                            UserId = usr.Id,
                            Name = usr.LastName + ", " + usr.FirstName,
                            Email = usr.Email,
                            Role = x.Name,
                            TeamName = t1.Name,
                            DesignationName = t2.Name,
                            ReportingTo = usr.ReportingTo,
                            //  ClientId = cli.ID
                        }).Distinct();
            var a = List.ToList();
            if (UD.Role == "SuperAdmin")
            {
                 a = List.ToList();
                List = List.Where(m => m.Email != UD.Email);
                 a = List.ToList();
            }
            else if (UD.Role == "SuperUser")
            {
                List = List.Where(m => m.Role != "SuperAdmin");
                List = List.Where(m => m.Email != UD.Email);
            }
            else if (UD.Role == "Manager")
            {
                List = List.Where(m => m.Role != "SuperAdmin" && m.Role != "SuperUser");
                List = List.Where(m => m.ReportingTo == UD.Email || m.Email == UD.Email);
                a = List.ToList();
                foreach (var item in List.ToList())
                {
                    gus.Add(item);
                    var List2 = (from cli in _context_main.MainClient
                                 join pra in _context_main.MainPractice
                                 on cli.ID equals pra.ClientID
                                 join up in _context_main.MainUserPractices
                                 on pra.ID equals up.PracticeID
                                 join usr in _context_main.Users
                                 on up.UserID equals usr.Id

                                 join w in _context_main.UserRoles
                                 on usr.Id equals w.UserId
                                 join x in _context_main.Roles
                                 on w.RoleId equals x.Id

                                 join te in _context_main.MainTeam
                                 on usr.TeamID equals te.ID into Table1
                                 from t1 in Table1.DefaultIfEmpty()

                                 join des in _context_main.MainDesignations
                                 on usr.DesignationID equals des.ID into Table2
                                 from t2 in Table2.DefaultIfEmpty()

                                 where up.Status == true &&
                                   (CUser.LastName.IsNull() ? true : usr.LastName.Contains(CUser.LastName)) &&
                                   (CUser.FirstName.IsNull() ? true : usr.FirstName.Contains(CUser.FirstName)) &&
                                   (CUser.Email.IsNull() ? true : usr.Email.Equals(CUser.Email)) &&
                                   (CUser.ClientID.IsNull() ? true : cli.ID.Equals(CUser.ClientID))
                                 select new GUser()
                                 {
                                     UserId = usr.Id,
                                     Name = usr.LastName + ", " + usr.FirstName,
                                     Email = usr.Email,
                                     Role = x.Name,
                                     TeamName = t1.Name,
                                     DesignationName = t2.Name,
                                     ReportingTo = usr.ReportingTo,
                                     //ClientId = cli.ID
                                 }).Distinct();
                    List2 = List2.Where(m => m.ReportingTo == item.Email);
                    foreach (var item2 in List2.ToList())
                    {
                        gus.Add(item2);
                    }
                }

                return gus.ToList();
            }
            else if (UD.Role == "TeamLead")
            {
                List = List.Where(m => m.ReportingTo == UD.Email || m.Email == UD.Email);
            }
            else if (UD.Role == "ClientManager")
            {
                List = List.Where(m => m.ReportingTo == UD.Email || m.Email == UD.Email);
            }
            else
            {
                List = List.Where(m =>
                                m.Role != "SuperAdmin" && m.Role != "SuperUser" && m.Role != "Manager" &&
                                m.Role != "ClientManager" && m.Role != "ClientUser" && m.Role != "TeamLead"
                                && m.Role != "Biller");
            }
            return await List.ToListAsync();
        }


        [Route("GetClientPractices/{id}")]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DropDown>> GetClientPractices(long id)
        {
            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Praclist = (from pr in _context_main.MainPractice
                            join uPr in _context_main.MainUserPractices
                            on pr.ID equals uPr.PracticeID

                            where pr.ClientID == id
                            && uPr.UserID == UserId
                            select new DropDown()
                            {
                                ID = pr.ID,
                                Description = pr.Name
                            }).ToList();

            Praclist.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            if (Praclist == null)
            {
                return BadRequest("Not Found");
            }

            return Ok(Praclist);
        }

        [Route("GetUserInfo")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<VMUser>> GetUserInfo()
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var CurrentLoginUser = (from u in _context_main.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();
            string UserId = CurrentLoginUser.Id;

            var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));

            VMUser obj = new VMUser();
            obj.GetUserInfo(_context_client, _context_main, UserId, RoleClaim.Value);

            if (obj.PracticeID > 0)
                obj.Token = GenerateJwtToken(_context_main, RoleClaim.Value, UserId);

            //obj.GetUserInfo(_context_client, UserId, RoleClaim.Value);
            return obj;
        }


        private object GenerateJwtToken(MainContext contextMain, string Role, string UserId)
        {
            var Clicntontext = (from u in contextMain.Users
                                join c in contextMain.MainClient
                                on u.ClientID equals c.ID
                                join p in contextMain.MainPractice
                                on u.PracticeID equals p.ID
                                where u.Id == UserId
                                select new
                                {
                                    c.ContextName,
                                    u.Email,
                                    p.ID
                                }).FirstOrDefault();
            List<Claim> claims = null;
            claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email,Clicntontext.Email),
                new Claim(JwtRegisteredClaimNames.Jti, UserId),
                new Claim("Role",Role),
                new Claim("TEMP", Clicntontext.ContextName),
                new Claim("RandomKEY", Clicntontext.ID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT-Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT-Expiry"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT-Issuer"],
                audience: _configuration["JWT-Issuer"],
                claims: claims,
                expires: expire,
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }



        [Route("test")]
        [HttpGet()]
        [Authorize]
        public string test()
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var CurrentLoginUser = (from u in _context_main.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();
            string UserId = CurrentLoginUser.Id;

            string Role = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value;


            var user = (from u in _context_main.Users
                        where u.Id == UserId
                        select u
                       ).First();
            string Name = user.FirstName + ", " + user.LastName;
            Email = user.Email;

            long? PracticeID = user.PracticeID;
            long? ClientID = user.ClientID;


            List<DropDown> Clients = null;


            if (Role == "SuperAdmin" || Role == "SuperUser")
            {
                Clients = (from u in _context_main.MainClient
                           select new DropDown()
                           {
                               ID = u.ID,
                               Description = u.Name//+ " - " + p.Coverage, 
                           }).ToList();
            }
            else
            {

                Clients = (from cli in _context_main.MainClient
                           join pra in _context_main.MainPractice
                           on cli.ID equals pra.ClientID
                           join up in _context_main.MainUserPractices
                           on pra.ID equals up.PracticeID
                           join usr in _context_main.Users
                           on up.UserID equals usr.Id
                           where usr.Id == UserId && up.Status == true
                           select new DropDown()
                           {
                               ID = cli.ID,
                               Description = cli.Name
                           }).Distinct().ToList();
            }

            // Fetching Practices From Main DB
            List<DropDown>  UserPractices = (from u in _context_main.MainUserPractices
                             join p in _context_main.MainPractice
                             on u.PracticeID equals p.ID
                             join w in _context_main.Users
                             on u.UserID equals w.Id
                             where u.UserID == UserId && u.Status == true
                             select new DropDown()
                             {
                                 ID = u.PracticeID,
                                 Description = p.Name
                             }).ToList();
            UserPractices.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            try
            {
                if (PracticeID > 0)
                {
                    string contextName = _context_main.MainClient.Find(ClientID)?.ContextName;
                    _context_client.setDatabaseName(contextName);

                    List<DropDown> UserLocations = (from u in _context_client.UserPractices
                                     join p in _context_client.Practice
                                     on u.PracticeID equals p.ID
                                     join w in _context_client.Users
                                     on u.UserID equals w.Id
                                     join loc in _context_client.Location
                                     on p.ID equals loc.PracticeID
                                     where u.UserID == UserId && p.ID == PracticeID && u.Status == true
                                     select new DropDown()
                                     {
                                         ID = loc.ID,
                                         ID2 = p.ID,
                                         Description = loc.Name
                                     }).ToList();
                    UserLocations.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



                    List<DropDown> UserProviders = (from u in _context_client.UserPractices
                                     join p in _context_client.Practice
                                     on u.PracticeID equals p.ID
                                     join w in _context_client.Users
                                     on u.UserID equals w.Id
                                     join pro in _context_client.Provider
                                     on p.ID equals pro.PracticeID
                                     where u.UserID == UserId && p.ID == PracticeID && u.Status == true
                                     select new DropDown()
                                     {
                                         ID = pro.ID,
                                         ID2 = p.ID,
                                         Description = pro.Name
                                     }).ToList();
                    UserProviders.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


                    List<DropDown> UserRefProviders = (from u in _context_client.UserPractices
                                        join p in _context_client.Practice
                                        on u.PracticeID equals p.ID
                                        join w in _context_client.Users
                                        on u.UserID equals w.Id
                                        join rPro in _context_client.RefProvider
                                        on p.ID equals rPro.PracticeID
                                        where u.UserID == UserId && p.ID == PracticeID && u.Status == true
                                        select new DropDown()
                                        {
                                            ID = rPro.ID,
                                            ID2 = p.ID,
                                            Description = rPro.Name
                                        }).ToList();
                    UserRefProviders.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
                }

            }
            catch (Exception)
            {
                if (PracticeID > 0)
                {
                    throw;
                }
            }


            List<DropDown> Teams = (from u in _context_main.MainTeam
                     select new DropDown()
                     {
                         ID = u.ID,
                         Description = u.Name, //+ " - " + p.Coverage, 
                     }).ToList();
            Teams.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            List<DropDown> Designations = (from u in _context_main.MainDesignations
                            select new DropDown()
                            {
                                ID = u.ID,
                                Description = u.Name, //+ " - " + p.Coverage, 
                            }).ToList();
            Designations.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            MainRights Rights = (from u in _context_main.MainRights
                      where u.Id == UserId
                      select u
                      ).FirstOrDefault();


            return Role;
        }


        [Route("FindUser/{Email}")]
        [HttpGet("{Email}")]
        [Authorize]
        public async Task<ActionResult<VMUser>> FindUser(string Email)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            Models.Settings settings = _context_client.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
      settings.DocumentServerDirectory, UD.PracticeID.ToString(), "Signature");
            VMUser obj = new VMUser();
            obj.FindUser(_context_main, Email, DirectoryPath);

            return obj;
        }
        [Route("SwitchPractice/{Id}")]
        [HttpGet("{Id}")]
        [Authorize]
        public async Task<ActionResult> SwitchPractice(long Id)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var CurrentLoginUser = (from u in _context_main.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();
            string LoginUserId = CurrentLoginUser.Id;
            var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));

            var checkPracticeAssigned = (from w in _context_main.MainUserPractices
                                         where w.UserID == LoginUserId && w.PracticeID == Id
                                         select w
                           ).FirstOrDefault();



            if (checkPracticeAssigned != null)
            {
                var Client = (from w in _context_main.MainPractice
                              where w.ID == Id
                              select w
                        ).FirstOrDefault();

                CurrentLoginUser.PracticeID = Id;
                CurrentLoginUser.ClientID = Client.ClientID;
                _context_main.Entry(CurrentLoginUser).State = EntityState.Modified;
                await _context_main.SaveChangesAsync();
                _context_client.Database.Migrate();

                //VMUser obj = new VMUser(_configuration);
                //obj.GetProfile(_context_main, LoginUserId, RoleClaim.Value);
                VMUser obj = new VMUser();
                obj.GetUserInfo(_context_client,_context_main, LoginUserId, RoleClaim.Value);
                obj.Token = GenerateJwtToken(_context_main, RoleClaim.Value, LoginUserId);
                return Ok(obj);
            }
            else
            {
                return BadRequest("Unexpected Error Occurred. Contact Bell Medex.");
            }

        }




        [Route("GetRoleManager")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GRoleManager>>> GetRoleManager(CRoleManager cRoleManager)
        {
            List<GRoleManager> a = new List<GRoleManager>();

            // Get Desgination query
            var Desgination = (from desig in _context_main.MainDesignations
                               where desig.ID == cRoleManager.DesignationID
                               select new
                               {
                                   desig.Name
                               }).FirstOrDefault();

            if (cRoleManager.Role == "Biller")
            {

                List<GRoleManager> RoleManagers = (from usr in _context_main.Users
                                                   join tm in _context_main.MainTeam
                                                   on usr.TeamID equals tm.ID
                                                   join uRol in _context_main.UserRoles
                                                   on usr.Id equals uRol.UserId
                                                   join rol in _context_main.Roles
                                                   on uRol.RoleId equals rol.Id
                                                   where
                                                   (rol.Name == "TeamLead") &&
                                                   (usr.TeamID.IsNull() ? true : usr.TeamID == cRoleManager.TeamID)
                                                   // commented by aziz
                                                   // (usr.DesignationID.IsNull() ? true : usr.DesignationID == cRoleManager.DesignationID)
                                                   select new GRoleManager()
                                                   {

                                                       Name = usr.LastName + ", " + usr.FirstName,
                                                       Email = usr.Email
                                                       //Role = rol.Name,
                                                       //TeamName = tm.Name
                                                   }).ToList();
                RoleManagers.Insert(0, new GRoleManager() { Email = null, Name = "Please Select" });
                return RoleManagers;


            }
            else if (cRoleManager.Role == "TeamLead")
            {
                List<GRoleManager> RoleManagers = (from usr in _context_main.Users
                                                   join tm in _context_main.MainTeam
                                                   on usr.TeamID equals tm.ID
                                                   join uRol in _context_main.UserRoles
                                                   on usr.Id equals uRol.UserId
                                                   join rol in _context_main.Roles
                                                   on uRol.RoleId equals rol.Id
                                                   where
                                                   (rol.Name == "Manager") &&
                                                   (usr.TeamID.IsNull() ? true : usr.TeamID == cRoleManager.TeamID)
                                                   // commented by aziz
                                                   // (usr.DesignationID.IsNull() ? true : usr.DesignationID == cRoleManager.DesignationID)
                                                   select new GRoleManager()
                                                   {

                                                       Name = usr.LastName + ", " + usr.FirstName,
                                                       Email = usr.Email
                                                       //Role = rol.Name,
                                                       //TeamName = tm.Name
                                                   }).ToList();
                RoleManagers.Insert(0, new GRoleManager() { Email = null, Name = "Please Select" });
                return RoleManagers;
            }
            else
            {
                a.Insert(0, new GRoleManager() { Email = null, Name = "Please Select" });
                return a;
            }
            return BadRequest("Not Found");
        }

        public ActionResult MainCreateRights(string id, string Role)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
                 User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
                 User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
                 );
            var checkrole = (from rol in _context_main.Roles
                             where rol.Name == Role
                             select rol
                            ).FirstOrDefault();
            if (checkrole == null)
                return BadRequest("No Role Found");

            MainRights chkRights = new MainRights();

            if (Role == "SuperAdmin")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;

            }
            else if (Role == "SuperUser")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "Manager")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "TeamLead")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "Biller")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;


                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "ClientManager")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "ClientUser")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "SupportAdmin")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;


            }
            else if (Role == "SupportEditor")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }
            else if (Role == "SupportUser")
            {
                //Scheduler Rights true;
                chkRights.SchedulerCreate = true;
                chkRights.SchedulerEdit = true;
                chkRights.SchedulerDelete = true;
                chkRights.SchedulerSearch = true;
                chkRights.SchedulerImport = true;
                chkRights.SchedulerExport = true;



                //Patient Rights
                chkRights.PatientCreate = true;
                chkRights.PatientEdit = true;
                chkRights.PatientDelete = true;
                chkRights.PatientSearch = true;
                chkRights.PatientImport = true;
                chkRights.PatientExport = true;

                //Charges Rights
                chkRights.ChargesCreate = true;
                chkRights.ChargesEdit = true;
                chkRights.ChargesDelete = true;
                chkRights.ChargesSearch = true;
                chkRights.ChargesImport = true;
                chkRights.ChargesExport = true;

                //Documents Rights
                chkRights.DocumentsCreate = true;
                chkRights.DocumentsEdit = true;
                chkRights.DocumentsDelete = true;
                chkRights.DocumentsSearch = true;
                chkRights.DocumentsImport = true;
                chkRights.DocumentsExport = true;

                //Submissions Rights
                chkRights.SubmissionsCreate = true;
                chkRights.SubmissionsEdit = true;
                chkRights.SubmissionsDelete = true;
                chkRights.SubmissionsSearch = true;
                chkRights.SubmissionsImport = true;
                chkRights.SubmissionsExport = true;

                //Payments Rights
                chkRights.PaymentsCreate = true;
                chkRights.PaymentsEdit = true;
                chkRights.PaymentsDelete = true;
                chkRights.PaymentsSearch = true;
                chkRights.PaymentsImport = true;
                chkRights.PaymentsExport = true;

                //Followup Rights
                chkRights.FollowupCreate = true;
                chkRights.FollowupEdit = true;
                chkRights.FollowupDelete = true;
                chkRights.FollowupSearch = true;
                chkRights.FollowupImport = true;
                chkRights.FollowupExport = true;

                //Reports Rights
                chkRights.ReportsCreate = true;
                chkRights.ReportsEdit = true;
                chkRights.ReportsDelete = true;
                chkRights.ReportsSearch = true;
                chkRights.ReportsImport = true;
                chkRights.ReportsExport = true;


                ////SetUp Client 
                //Client Rights
                chkRights.ClientCreate = true;
                chkRights.ClientEdit = true;
                chkRights.ClientDelete = true;
                chkRights.ClientSearch = true;
                chkRights.ClientImport = true;
                chkRights.ClientExport = true;

                chkRights.UserCreate = true;
                chkRights.UserEdit = true;
                chkRights.UserDelete = true;
                chkRights.UserSearch = true;
                chkRights.UserImport = true;
                chkRights.UserExport = true;

                ////SetUp Admin 
                //Practice
                chkRights.PracticeCreate = true;
                chkRights.PracticeEdit = true;
                chkRights.PracticeDelete = true;
                chkRights.PracticeSearch = true;
                chkRights.PracticeImport = true;
                chkRights.PracticeExport = true;

                //Location
                chkRights.LocationCreate = true;
                chkRights.LocationEdit = true;
                chkRights.LocationDelete = true;
                chkRights.LocationSearch = true;
                chkRights.LocationImport = true;
                chkRights.LocationExport = true;

                //Provider
                chkRights.ProviderCreate = true;
                chkRights.ProviderEdit = true;
                chkRights.ProviderDelete = true;
                chkRights.ProviderSearch = true;
                chkRights.ProviderImport = true;
                chkRights.ProviderExport = true;


                //Referring Provider
                chkRights.ReferringProviderCreate = true;
                chkRights.ReferringProviderEdit = true;
                chkRights.ReferringProviderDelete = true;
                chkRights.ReferringProviderSearch = true;
                chkRights.ReferringProviderImport = true;
                chkRights.ReferringProviderExport = true;

                //Setup Insurance
                //Insurance

                chkRights.InsuranceCreate = true;
                chkRights.InsuranceEdit = true;
                chkRights.InsuranceDelete = true;
                chkRights.InsuranceSearch = true;
                chkRights.InsuranceImport = true;
                chkRights.InsuranceExport = true;

                //Insurance Plan 
                chkRights.InsurancePlanCreate = true;
                chkRights.InsurancePlanEdit = true;
                chkRights.InsurancePlanDelete = true;
                chkRights.InsurancePlanSearch = true;
                chkRights.InsurancePlanImport = true;
                chkRights.InsurancePlanExport = true;

                //Insurance Plan Address 
                chkRights.InsurancePlanAddressCreate = true;
                chkRights.InsurancePlanAddressEdit = true;
                chkRights.InsurancePlanAddressDelete = true;
                chkRights.InsurancePlanAddressSearch = true;
                chkRights.InsurancePlanAddressImport = true;
                chkRights.InsurancePlanAddressExport = true;

                //EDI Submit
                chkRights.EDISubmitCreate = true;
                chkRights.EDISubmitEdit = true;
                chkRights.EDISubmitDelete = true;
                chkRights.EDISubmitSearch = true;
                chkRights.EDISubmitImport = true;
                chkRights.EDISubmitExport = true;

                //EDI EligiBility
                chkRights.EDIEligiBilityCreate = true;
                chkRights.EDIEligiBilityEdit = true;
                chkRights.EDIEligiBilityDelete = true;
                chkRights.EDIEligiBilitySearch = true;
                chkRights.EDIEligiBilityImport = true;
                chkRights.EDIEligiBilityExport = true;

                //EDI Status
                chkRights.EDIStatusCreate = true;
                chkRights.EDIStatusEdit = true;
                chkRights.EDIStatusDelete = true;
                chkRights.EDIStatusSearch = true;
                chkRights.EDIStatusImport = true;
                chkRights.EDIStatusExport = true;

                //Coding
                //ICD
                chkRights.ICDCreate = true;
                chkRights.ICDEdit = true;
                chkRights.ICDDelete = true;
                chkRights.ICDSearch = true;
                chkRights.ICDImport = true;
                chkRights.ICDExport = true;

                //CPT
                chkRights.CPTCreate = true;
                chkRights.CPTEdit = true;
                chkRights.CPTDelete = true;
                chkRights.CPTSearch = true;
                chkRights.CPTImport = true;
                chkRights.CPTExport = true;

                //Modifiers
                chkRights.ModifiersCreate = true;
                chkRights.ModifiersEdit = true;
                chkRights.ModifiersDelete = true;
                chkRights.ModifiersSearch = true;
                chkRights.ModifiersImport = true;
                chkRights.ModifiersExport = true;

                //POS
                chkRights.POSCreate = true;
                chkRights.POSEdit = true;
                chkRights.POSDelete = true;
                chkRights.POSSearch = true;
                chkRights.POSImport = true;
                chkRights.POSExport = true;

                //EDI Codes
                //Claim Status Category Codes
                chkRights.ClaimStatusCategoryCodesCreate = true;
                chkRights.ClaimStatusCategoryCodesEdit = true;
                chkRights.ClaimStatusCategoryCodesDelete = true;
                chkRights.ClaimStatusCategoryCodesSearch = true;
                chkRights.ClaimStatusCategoryCodesImport = true;
                chkRights.ClaimStatusCategoryCodesExport = true;

                //Claim Status Codes
                chkRights.ClaimStatusCodesCreate = true;
                chkRights.ClaimStatusCodesEdit = true;
                chkRights.ClaimStatusCodesDelete = true;
                chkRights.ClaimStatusCodesSearch = true;
                chkRights.ClaimStatusCodesImport = true;
                chkRights.ClaimStatusCodesExport = true;

                //Adjustment Codes
                chkRights.AdjustmentCodesCreate = true;
                chkRights.AdjustmentCodesEdit = true;
                chkRights.AdjustmentCodesDelete = true;
                chkRights.AdjustmentCodesSearch = true;
                chkRights.AdjustmentCodesImport = true;
                chkRights.AdjustmentCodesExport = true;

                //Remark Codes
                chkRights.RemarkCodesCreate = true;
                chkRights.RemarkCodesEdit = true;
                chkRights.RemarkCodesDelete = true;
                chkRights.RemarkCodesSearch = true;
                chkRights.RemarkCodesImport = true;
                chkRights.RemarkCodesExport = true;

                //Team Rights
                chkRights.teamCreate = true;
                chkRights.teamupdate = true;
                chkRights.teamDelete = true;
                chkRights.teamSearch = true;
                chkRights.teamExport = true;
                chkRights.teamImport = true;

                //Receiver Rights
                chkRights.receiverCreate = true;
                chkRights.receiverupdate = true;
                chkRights.receiverDelete = true;
                chkRights.receiverSearch = true;
                chkRights.receiverExport = true;
                chkRights.receiverImport = true;

                //Submitter Rights
                chkRights.submitterCreate = true;
                chkRights.submitterUpdate = true;
                chkRights.submitterDelete = true;
                chkRights.submitterSearch = true;
                chkRights.submitterExport = true;
                chkRights.submitterImport = true;




                //PatientPlan Rights
                chkRights.patientPlanCreate = true;
                chkRights.patientPlanUpdate = true;
                chkRights.patientPlanDelete = true;
                chkRights.patientPlanSearch = true;
                chkRights.patientPlanExport = true;
                chkRights.patientPlanImport = true;
                chkRights.performEligibility = true;


                //PatientPayment Rights
                chkRights.patientPaymentCreate = true;
                chkRights.patientPaymentUpdate = true;
                chkRights.patientPaymentDelete = true;
                chkRights.patientPaymentSearch = true;
                chkRights.patientPaymentExport = true;
                chkRights.patientPaymentImport = true;

                // Charges Rights
                chkRights.resubmitCharges = true;
                // BatchDocument Rights
                chkRights.batchdocumentCreate = true;
                chkRights.batchdocumentUpdate = true;
                chkRights.batchdocumentDelete = true;
                chkRights.batchdocumentSearch = true;
                chkRights.batchdocumentExport = true;
                chkRights.batchdocumentImport = true;





                // ElectronicSubmission Rights
                chkRights.electronicsSubmissionSearch = true;
                chkRights.electronicsSubmissionSubmit = true;
                chkRights.electronicsSubmissionResubmit = true;

                // PaperSubmission Rights
                chkRights.paperSubmissionSearch = true;
                chkRights.paperSubmissionSubmit = true;
                chkRights.paperSubmissionResubmit = true;
                // SubmissionLog Rights	
                chkRights.submissionLogSearch = true;
                // PlanFollowup Rights	
                chkRights.planFollowupSearch = true;
                chkRights.planFollowupCreate = true;
                chkRights.planFollowupDelete = true;
                chkRights.planFollowupUpdate = true;
                chkRights.planFollowupImport = true;
                chkRights.planFollowupExport = true;

                // PatientFollowup Rights	
                chkRights.patientFollowupSearch = true;
                chkRights.patientFollowupCreate = true;
                chkRights.patientFollowupDelete = true;
                chkRights.patientFollowupUpdate = true;
                chkRights.patientFollowupImport = true;
                chkRights.patientFollowupExport = true;

                // Group Rights	
                chkRights.groupSearch = true;
                chkRights.groupCreate = true;
                chkRights.groupUpdate = true;
                chkRights.groupDelete = true;
                chkRights.groupExport = true;
                chkRights.groupImport = true;
                // Reason Rights	
                chkRights.reasonSearch = true;
                chkRights.reasonCreate = true;
                chkRights.reasonUpdate = true;
                chkRights.reasonDelete = true;
                chkRights.reasonExport = true;
                chkRights.reasonImport = true;

                chkRights.addPaymentVisit = true;
                chkRights.DeleteCheck = true;
                chkRights.ManualPosting = true;
                chkRights.Postcheck = true;
                chkRights.PostExport = true;
                chkRights.PostImport = true;

                chkRights.ManualPostingAdd = true;
                chkRights.ManualPostingUpdate = true;
                chkRights.PostCheckSearch = true;
                chkRights.DeletePaymentVisit = true;
            }

            chkRights.Id = id;
            chkRights.AddedBy = UD.Email;
            _context_main.MainRights.Add(chkRights);
            _context_main.SaveChanges();

            return Ok("OK");

        }

        public ActionResult ClientCreateRights(string id)
        {

            var Rights = (from rights in _context_main.MainRights where rights.Id == id select rights).FirstOrDefault();



            Rights chkRights = new Rights();
            chkRights.Id = Rights.Id;

            //Scheduler Rights
            chkRights.SchedulerCreate = Rights.SchedulerCreate;
            chkRights.SchedulerEdit = Rights.SchedulerEdit;
            chkRights.SchedulerDelete = Rights.SchedulerDelete;
            chkRights.SchedulerSearch = Rights.SchedulerSearch;
            chkRights.SchedulerImport = Rights.SchedulerImport;
            chkRights.SchedulerExport = Rights.SchedulerExport;



            //Patient Rights
            chkRights.PatientCreate = Rights.PatientCreate;
            chkRights.PatientEdit = Rights.PatientEdit;
            chkRights.PatientDelete = Rights.PatientDelete;
            chkRights.PatientSearch = Rights.PatientSearch;
            chkRights.PatientImport = Rights.PatientImport;
            chkRights.PatientExport = Rights.PatientExport;

            //Charges Rights
            chkRights.ChargesCreate = Rights.ChargesCreate;
            chkRights.ChargesEdit = Rights.ChargesEdit;
            chkRights.ChargesDelete = Rights.ChargesDelete;
            chkRights.ChargesSearch = Rights.ChargesSearch;
            chkRights.ChargesImport = Rights.ChargesImport;
            chkRights.ChargesExport = Rights.ChargesExport;

            //Documents Rights
            chkRights.DocumentsCreate = Rights.DocumentsCreate;
            chkRights.DocumentsEdit = Rights.DocumentsEdit;
            chkRights.DocumentsDelete = Rights.DocumentsDelete;
            chkRights.DocumentsSearch = Rights.DocumentsSearch;
            chkRights.DocumentsImport = Rights.DocumentsImport;
            chkRights.DocumentsExport = Rights.DocumentsExport;

            //Submissions Rights
            chkRights.SubmissionsCreate = Rights.SubmissionsCreate;
            chkRights.SubmissionsEdit = Rights.SubmissionsEdit;
            chkRights.SubmissionsDelete = Rights.SubmissionsDelete;
            chkRights.SubmissionsSearch = Rights.SubmissionsSearch;
            chkRights.SubmissionsImport = Rights.SubmissionsImport;
            chkRights.SubmissionsExport = Rights.SubmissionsExport;

            //Payments Rights
            chkRights.PaymentsCreate = Rights.PaymentsCreate;
            chkRights.PaymentsEdit = Rights.PaymentsEdit;
            chkRights.PaymentsDelete = Rights.PaymentsDelete;
            chkRights.PaymentsSearch = Rights.PaymentsSearch;
            chkRights.PaymentsImport = Rights.PaymentsImport;
            chkRights.PaymentsExport = Rights.PaymentsExport;

            //Followup Rights
            chkRights.FollowupCreate = Rights.FollowupCreate;
            chkRights.FollowupEdit = Rights.FollowupEdit;
            chkRights.FollowupDelete = Rights.FollowupDelete;
            chkRights.FollowupSearch = Rights.FollowupSearch;
            chkRights.FollowupImport = Rights.FollowupImport;
            chkRights.FollowupExport = Rights.FollowupExport;

            //Reports Rights
            chkRights.ReportsCreate = Rights.ReportsCreate;
            chkRights.ReportsEdit = Rights.ReportsEdit;
            chkRights.ReportsDelete = Rights.ReportsDelete;
            chkRights.ReportsSearch = Rights.ReportsSearch;
            chkRights.ReportsImport = Rights.ReportsImport;
            chkRights.ReportsExport = Rights.ReportsExport;


            ////SetUp Client 
            //Client Rights
            chkRights.ClientCreate = Rights.ClientCreate;
            chkRights.ClientEdit = Rights.ClientEdit;
            chkRights.ClientDelete = Rights.ClientDelete;
            chkRights.ClientSearch = Rights.ClientSearch;
            chkRights.ClientImport = Rights.ClientImport;
            chkRights.ClientExport = Rights.ClientExport;

            chkRights.UserCreate = Rights.UserCreate;
            chkRights.UserEdit = Rights.UserEdit;
            chkRights.UserDelete = Rights.UserDelete;
            chkRights.UserSearch = Rights.UserSearch;
            chkRights.UserImport = Rights.UserImport;
            chkRights.UserExport = Rights.UserExport;

            ////SetUp Admin 
            //Practice
            chkRights.PracticeCreate = Rights.PracticeCreate;
            chkRights.PracticeEdit = Rights.PracticeEdit;
            chkRights.PracticeDelete = Rights.PracticeDelete;
            chkRights.PracticeSearch = Rights.PracticeSearch;
            chkRights.PracticeImport = Rights.PracticeImport;
            chkRights.PracticeExport = Rights.PracticeExport;

            //Location
            chkRights.LocationCreate = Rights.LocationCreate;
            chkRights.LocationEdit = Rights.LocationEdit;
            chkRights.LocationDelete = Rights.LocationDelete;
            chkRights.LocationSearch = Rights.LocationSearch;
            chkRights.LocationImport = Rights.LocationImport;
            chkRights.LocationExport = Rights.LocationExport;

            //Provider
            chkRights.ProviderCreate = Rights.ProviderCreate;
            chkRights.ProviderEdit = Rights.ProviderEdit;
            chkRights.ProviderDelete = Rights.ProviderDelete;
            chkRights.ProviderSearch = Rights.ProviderSearch;
            chkRights.ProviderImport = Rights.ProviderImport;
            chkRights.ProviderExport = Rights.ProviderExport;


            //Referring Provider
            chkRights.ReferringProviderCreate = Rights.ReferringProviderCreate;
            chkRights.ReferringProviderEdit = Rights.ReferringProviderEdit;
            chkRights.ReferringProviderDelete = Rights.ReferringProviderDelete;
            chkRights.ReferringProviderSearch = Rights.ReferringProviderSearch;
            chkRights.ReferringProviderImport = Rights.ReferringProviderImport;
            chkRights.ReferringProviderExport = Rights.ReferringProviderExport;

            //Setup Insurance
            //Insurance

            chkRights.InsuranceCreate = Rights.InsuranceCreate;
            chkRights.InsuranceEdit = Rights.InsuranceEdit;
            chkRights.InsuranceDelete = Rights.InsuranceDelete;
            chkRights.InsuranceSearch = Rights.InsuranceSearch;
            chkRights.InsuranceImport = Rights.InsuranceImport;
            chkRights.InsuranceExport = Rights.InsuranceExport;

            //Insurance Plan 
            chkRights.InsurancePlanCreate = Rights.InsurancePlanCreate;
            chkRights.InsurancePlanEdit = Rights.InsurancePlanEdit;
            chkRights.InsurancePlanDelete = Rights.InsurancePlanDelete;
            chkRights.InsurancePlanSearch = Rights.InsurancePlanSearch;
            chkRights.InsurancePlanImport = Rights.InsurancePlanImport;
            chkRights.InsurancePlanExport = Rights.InsurancePlanExport;

            //Insurance Plan Address 
            chkRights.InsurancePlanAddressCreate = Rights.InsurancePlanAddressCreate;
            chkRights.InsurancePlanAddressEdit = Rights.InsurancePlanAddressEdit;
            chkRights.InsurancePlanAddressDelete = Rights.InsurancePlanAddressDelete;
            chkRights.InsurancePlanAddressSearch = Rights.InsurancePlanAddressSearch;
            chkRights.InsurancePlanAddressImport = Rights.InsurancePlanAddressImport;
            chkRights.InsurancePlanAddressExport = Rights.InsurancePlanAddressExport;

            //EDI Submit
            chkRights.EDISubmitCreate = Rights.EDISubmitCreate;
            chkRights.EDISubmitEdit = Rights.EDISubmitEdit;
            chkRights.EDISubmitDelete = Rights.EDISubmitDelete;
            chkRights.EDISubmitSearch = Rights.EDISubmitSearch;
            chkRights.EDISubmitImport = Rights.EDISubmitImport;
            chkRights.EDISubmitExport = Rights.EDISubmitExport;

            //EDI EligiBility
            chkRights.EDIEligiBilityCreate = Rights.EDIEligiBilityCreate;
            chkRights.EDIEligiBilityEdit = Rights.EDIEligiBilityEdit;
            chkRights.EDIEligiBilityDelete = Rights.EDIEligiBilityDelete;
            chkRights.EDIEligiBilitySearch = Rights.EDIEligiBilitySearch;
            chkRights.EDIEligiBilityImport = Rights.EDIEligiBilityImport;
            chkRights.EDIEligiBilityExport = Rights.EDIEligiBilityExport;

            //EDI Status
            chkRights.EDIStatusCreate = Rights.EDIStatusCreate;
            chkRights.EDIStatusEdit = Rights.EDIStatusEdit;
            chkRights.EDIStatusDelete = Rights.EDIStatusDelete;
            chkRights.EDIStatusSearch = Rights.EDIStatusSearch;
            chkRights.EDIStatusImport = Rights.EDIStatusImport;
            chkRights.EDIStatusExport = Rights.EDIStatusExport;

            //Coding
            //ICD
            chkRights.ICDCreate = Rights.ICDCreate;
            chkRights.ICDEdit = Rights.ICDEdit;
            chkRights.ICDDelete = Rights.ICDDelete;
            chkRights.ICDSearch = Rights.ICDSearch;
            chkRights.ICDImport = Rights.ICDImport;
            chkRights.ICDExport = Rights.ICDExport;

            //CPT
            chkRights.CPTCreate = Rights.CPTCreate;
            chkRights.CPTEdit = Rights.CPTEdit;
            chkRights.CPTDelete = Rights.CPTDelete;
            chkRights.CPTSearch = Rights.CPTSearch;
            chkRights.CPTImport = Rights.CPTImport;
            chkRights.CPTExport = Rights.CPTExport;

            //Modifiers
            chkRights.ModifiersCreate = Rights.ModifiersCreate;
            chkRights.ModifiersEdit = Rights.ModifiersEdit;
            chkRights.ModifiersDelete = Rights.ModifiersDelete;
            chkRights.ModifiersSearch = Rights.ModifiersSearch;
            chkRights.ModifiersImport = Rights.ModifiersImport;
            chkRights.ModifiersExport = Rights.ModifiersExport;

            //POS
            chkRights.POSCreate = Rights.POSCreate;
            chkRights.POSEdit = Rights.POSEdit;
            chkRights.POSDelete = Rights.POSDelete;
            chkRights.POSSearch = Rights.POSSearch;
            chkRights.POSImport = Rights.POSImport;
            chkRights.POSExport = Rights.POSExport;

            //EDI Codes
            //Claim Status Category Codes
            chkRights.ClaimStatusCategoryCodesCreate = Rights.ClaimStatusCategoryCodesCreate;
            chkRights.ClaimStatusCategoryCodesEdit = Rights.ClaimStatusCategoryCodesEdit;
            chkRights.ClaimStatusCategoryCodesDelete = Rights.ClaimStatusCategoryCodesDelete;
            chkRights.ClaimStatusCategoryCodesSearch = Rights.ClaimStatusCategoryCodesSearch;
            chkRights.ClaimStatusCategoryCodesImport = Rights.ClaimStatusCategoryCodesImport;
            chkRights.ClaimStatusCategoryCodesExport = Rights.ClaimStatusCategoryCodesExport;

            //Claim Status Codes
            chkRights.ClaimStatusCodesCreate = Rights.ClaimStatusCodesCreate;
            chkRights.ClaimStatusCodesEdit = Rights.ClaimStatusCodesEdit;
            chkRights.ClaimStatusCodesDelete = Rights.ClaimStatusCodesDelete;
            chkRights.ClaimStatusCodesSearch = Rights.ClaimStatusCodesSearch;
            chkRights.ClaimStatusCodesImport = Rights.ClaimStatusCodesImport;
            chkRights.ClaimStatusCodesExport = Rights.ClaimStatusCodesExport;

            //Adjustment Codes
            chkRights.AdjustmentCodesCreate = Rights.AdjustmentCodesCreate;
            chkRights.AdjustmentCodesEdit = Rights.AdjustmentCodesEdit;
            chkRights.AdjustmentCodesDelete = Rights.AdjustmentCodesDelete;
            chkRights.AdjustmentCodesSearch = Rights.AdjustmentCodesSearch;
            chkRights.AdjustmentCodesImport = Rights.AdjustmentCodesImport;
            chkRights.AdjustmentCodesExport = Rights.AdjustmentCodesExport;

            //Remark Codes
            chkRights.RemarkCodesCreate = Rights.RemarkCodesCreate;
            chkRights.RemarkCodesEdit = Rights.RemarkCodesEdit;
            chkRights.RemarkCodesDelete = Rights.RemarkCodesDelete;
            chkRights.RemarkCodesSearch = Rights.RemarkCodesSearch;
            chkRights.RemarkCodesImport = Rights.RemarkCodesImport;
            chkRights.RemarkCodesExport = Rights.RemarkCodesExport;



            //Team Rights
            chkRights.teamCreate = Rights.teamCreate;
            chkRights.teamupdate = Rights.teamupdate;
            chkRights.teamDelete = Rights.teamDelete;
            chkRights.teamSearch = Rights.teamSearch;
            chkRights.teamExport = Rights.teamExport;
            chkRights.teamImport = Rights.teamImport;

            //Receiver Rights
            chkRights.receiverCreate = Rights.receiverCreate;
            chkRights.receiverupdate = Rights.receiverupdate;
            chkRights.receiverDelete = Rights.receiverDelete;
            chkRights.receiverSearch = Rights.receiverSearch;
            chkRights.receiverExport = Rights.receiverExport;
            chkRights.receiverImport = Rights.receiverImport;

            //Submitter Rights
            chkRights.submitterCreate = Rights.submitterCreate;
            chkRights.submitterUpdate = Rights.submitterUpdate;
            chkRights.submitterDelete = Rights.submitterDelete;
            chkRights.submitterSearch = Rights.submitterSearch;
            chkRights.submitterExport = Rights.submitterExport;
            chkRights.submitterImport = Rights.submitterImport;




            //PatientPlan Rights
            chkRights.patientPlanCreate = Rights.patientPlanCreate;
            chkRights.patientPlanUpdate = Rights.patientPlanUpdate;
            chkRights.patientPlanDelete = Rights.patientPlanDelete;
            chkRights.patientPlanSearch = Rights.patientPlanSearch;
            chkRights.patientPlanExport = Rights.patientPlanExport;
            chkRights.patientPlanImport = Rights.patientPlanImport;
            chkRights.performEligibility = Rights.performEligibility;


            //PatientPayment Rights
            chkRights.patientPaymentCreate = Rights.patientPaymentCreate;
            chkRights.patientPaymentUpdate = Rights.patientPaymentUpdate;
            chkRights.patientPaymentDelete = Rights.patientPaymentDelete;
            chkRights.patientPaymentSearch = Rights.patientPaymentSearch;
            chkRights.patientPaymentExport = Rights.patientPaymentExport;
            chkRights.patientPaymentImport = Rights.patientPaymentImport;

            // Charges Rights
            chkRights.resubmitCharges = Rights.resubmitCharges;
            // BatchDocument Rights
            chkRights.batchdocumentCreate = Rights.batchdocumentCreate;
            chkRights.batchdocumentUpdate = Rights.batchdocumentUpdate;
            chkRights.batchdocumentDelete = Rights.batchdocumentDelete;
            chkRights.batchdocumentSearch = Rights.batchdocumentSearch;
            chkRights.batchdocumentExport = Rights.batchdocumentExport;
            chkRights.batchdocumentImport = Rights.batchdocumentImport;





            // ElectronicSubmission Rights
            chkRights.electronicsSubmissionSearch = Rights.electronicsSubmissionSearch;
            chkRights.electronicsSubmissionSubmit = Rights.electronicsSubmissionSubmit;
            chkRights.electronicsSubmissionResubmit = Rights.electronicsSubmissionResubmit;

            // PaperSubmission Rights
            chkRights.paperSubmissionSearch = Rights.paperSubmissionSearch;
            chkRights.paperSubmissionSubmit = Rights.paperSubmissionSubmit;
            chkRights.paperSubmissionResubmit = Rights.paperSubmissionResubmit;
            // SubmissionLog Rights	
            chkRights.submissionLogSearch = Rights.submissionLogSearch;
            // PlanFollowup Rights	
            chkRights.planFollowupSearch = Rights.planFollowupSearch;
            chkRights.planFollowupCreate = Rights.planFollowupCreate;
            chkRights.planFollowupDelete = Rights.planFollowupDelete;
            chkRights.planFollowupUpdate = Rights.planFollowupUpdate;
            chkRights.planFollowupImport = Rights.planFollowupImport;
            chkRights.planFollowupExport = Rights.planFollowupExport;

            // PatientFollowup Rights	
            chkRights.patientFollowupSearch = Rights.patientFollowupSearch;
            chkRights.patientFollowupCreate = Rights.patientFollowupCreate;
            chkRights.patientFollowupDelete = Rights.patientFollowupDelete;
            chkRights.patientFollowupUpdate = Rights.patientFollowupUpdate;
            chkRights.patientFollowupImport = Rights.patientFollowupImport;
            chkRights.patientFollowupExport = Rights.patientFollowupExport;

            // Group Rights	
            chkRights.groupSearch = Rights.groupSearch;
            chkRights.groupCreate = Rights.groupCreate;
            chkRights.groupUpdate = Rights.groupUpdate;
            chkRights.groupDelete = Rights.groupDelete;
            chkRights.groupExport = Rights.groupExport;
            chkRights.groupImport = Rights.groupImport;
            // Reason Rights	
            chkRights.reasonSearch = Rights.reasonSearch;
            chkRights.reasonCreate = Rights.reasonCreate;
            chkRights.reasonUpdate = Rights.reasonUpdate;
            chkRights.reasonDelete = Rights.reasonDelete;
            chkRights.reasonExport = Rights.reasonExport;
            chkRights.reasonImport = Rights.reasonImport;

            chkRights.addPaymentVisit = Rights.addPaymentVisit;
            chkRights.DeleteCheck = Rights.DeleteCheck;
            chkRights.ManualPosting = Rights.ManualPosting;
            chkRights.Postcheck = Rights.Postcheck;
            chkRights.PostExport = Rights.PostExport;
            chkRights.PostImport = Rights.PostImport;
            chkRights.ManualPostingAdd = Rights.ManualPostingAdd;
            chkRights.ManualPostingUpdate = Rights.ManualPostingUpdate;
            chkRights.PostCheckSearch = Rights.PostCheckSearch;
            chkRights.DeletePaymentVisit = Rights.DeletePaymentVisit;


            chkRights.UpdatedBy = Rights.UpdatedBy;

            _context_client.Rights.Add(chkRights);
            _context_client.SaveChanges();

            return Ok("OK");

        }

        [Authorize]
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check this Email already Exist or not
            var UserProfile = (from u in _context_main.Users
                               where u.Email == model.Email
                               select u).FirstOrDefault();
            if (UserProfile != null)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var removepass = _userManager.RemovePasswordAsync(user);
                if (removepass.Result.Succeeded)
                {
                    var resulwt = await _userManager.AddPasswordAsync(user, model.Password);
                    if (resulwt.Succeeded)
                    {
                        UserProfile.IsUserBlock = false;
                        UserProfile.LogInAttempts = 0;
                        UserProfile.BlockNote ="";
                        _context_main.Entry(UserProfile).State = EntityState.Modified;
                        await _context_main.SaveChangesAsync();
                        //ClientUserProfile.FirstName = model.FirstName;
                        //ClientUserProfile.LastName = model.LastName;
                        //ClientUserProfile.TeamID = model.TeamID;
                        //ClientUserProfile.DesignationID = model.DesignationID;
                        //ClientUserProfile.ReportingTo = model.ReportingTo;
                        //_context_client.Entry(ClientUserProfile).State = EntityState.Modified;
                        //await _context_client.SaveChangesAsync();

                        return Ok("Password Reset Done.");
                    }
                    return Ok("Password Removed Done.");
                }
                return Ok("Password Reset Failed.");
            }
            else { return Ok("User not Found."); }
        }

        [Authorize]
        [HttpPost]
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
            //   User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //   User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            //   );
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(Email);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword,
                model.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Password Changed Done");
            }

            return Ok("Password Changed Failed");
        }

        //Code By Waseem Sabir 03312020
       // [Authorize]
        [HttpGet("{emailTo}")]
        [AllowAnonymous]
        [Route("SendPasswordResetLink/{emailTo}")]
        public IActionResult SendPasswordResetLink(string emailTo)
        {

            MainAuthIdentityCustom us = new MainAuthIdentityCustom();
            us.Email = emailTo;
            var appUser = _userManager.Users.SingleOrDefault(m => m.Email == emailTo);
            us.Id = appUser.Id;
            var appUserRole = _userManager.GetRolesAsync(us).Result.SingleOrDefault() ?? "None";



            if (appUser == null)
            {
                return BadRequest();
            }


            VMUser token = new VMUser();
            token.GetUserInfo(_context_client, _context_main, appUser.Id, appUserRole);
            token.Token = GenerateJwtToken(_context_main, appUserRole, appUser.Id);

            var resetLink = Url.Action("ResetPassword",
                            "Account", new { token = token.Token },
                             protocol: HttpContext.Request.Scheme);

            // code to email the above link



            var lnkHref = "<a href='" + Url.Action("ResetPassword", "Account", new { token = token.Token }, "http") + "'>Reset Password</a>";


            //HTML Template for Send email  

            string subject = "Your changed password";

            string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;



            //Call send email methods.  


            string UserName = _configuration["EmailSettings:UserName"];
            string Password = _configuration["EmailSettings:Password"];
            string Host = _configuration["EmailSettings:SmtpHost"];
            string Port = _configuration["EmailSettings:SmtpPort"];
            string From = _configuration["EmailSettings:From"];

            SendEmail(subject, body, emailTo, From, Host, Port, UserName, Password);


            // see the earlier article


            return Ok(body);

        }
        private static void SendEmail(string subject, string body, string emailTo, string From, string Host, string Port, string UserName, string Password)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(emailTo);
            mail.From = new MailAddress(From);
            mail.Subject = subject;
            mail.Body = body;


            SmtpClient smtp = new SmtpClient();
            smtp.Host = Host;
            smtp.Port = Convert.ToInt16(Port);
            smtp.Credentials = new NetworkCredential(UserName, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);

        }


    }

}