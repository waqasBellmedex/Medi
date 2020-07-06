using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.Models.Main;
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Uitilities;
using MediFusionPM.ViewModels;
using MediFusionPM.ViewModels.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMJobs;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ClientDbContext _clientContext;
        private readonly MainContext _mainContext;
        public IConfiguration _config;
        public IHostingEnvironment _env;
        public IConfiguration configuration;
        public JobsController(UserManager<MainAuthIdentityCustom> userManager, SignInManager<MainAuthIdentityCustom> signInManager, ClientDbContext contextClient, MainContext contextMain, IConfiguration config, IHostingEnvironment env, IConfiguration Configuration)
        {
            _clientContext = contextClient;
            _mainContext = contextMain;
            this._config = _config;
            this._env = _env;
            configuration = Configuration;
        }

        //public JobsController(ClientDbContext context)
        //{
        //    _context = context;
        //}

        [Route("CreateFollowUp")]
        [HttpPost()]
        public async Task<ActionResult<VMAutoPlanFollowUp>> CreateFollowUp()
        {

            //UserInfoData UD = VMCommon.GetLoginUserInfo(_mainContext,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.LocationSearch == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;


            List<CreateFollowUpTable> database = (from v in _clientContext.Visit
                                                  join Prac in _clientContext.Practice on v.PracticeID equals Prac.ID
                                                  join pPlan in _clientContext.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                                  join iPlan in _clientContext.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                  where
                                                  Prac.ID == PracticeId &&
                                                  v.IsSubmitted == true &&
                                                 ((DateTime.Now - v.SubmittedDate.Date()).Days >= (iPlan.OutstandingDays.IsNull() ? 3 : 30)) &&
                                                 (v.PrimaryPaid.IsNull() || v.PrimaryPaid.Val() == 0)
                                                  let ces = from Vi in _clientContext.PlanFollowUp
                                                            select Vi.ID
                                                  where (!ces.Contains(v.ID))
                                                  select new CreateFollowUpTable()
                                                  {
                                                      VisitID = v.ID,
                                                      LocationID = v.LocationID,
                                                      ProviderID = v.ID,
                                                      PrimaryPatientPlanID = v.ID,
                                                      PatientID = v.ID,
                                                      SubmittedDateID = v.ID,
                                                      PrimaryPaidID = v.ID,
                                                      PracticeID = v.ID,
                                                      PlanID = pPlan.ID,
                                                      InsuranceplanID = pPlan.ID,
                                                  }).Distinct().ToList();

            //database = (from db in database
            //join v in _context_client.Visit on db.VisitID equals v.ID
            //select db).Distinct().ToList();





            var reason = (from r in _clientContext.Reason
                          where r.Name == "SYSTEM"
                          select r.ID).FirstOrDefault();
            long? action = (from r in _clientContext.Action
                            where r.Name == "SYSTEM"
                            select r.ID).FirstOrDefault();

            long? group = (from r in _clientContext.Group
                           where r.Name == "NEW"
                           select r.ID).FirstOrDefault();
            ////List<JobsInsertdata> lstInsertData= new List<JobsInsertdata>();
            int inserted = 0;
            for (int i = 0; i < database.Count; i++)
            {
                var data = database[i];
                PlanFollowup Obj = new PlanFollowup()
                {
                    ActionID = action,
                    ReasonID = reason,
                    GroupID = group,
                    VisitID = data.VisitID,
                    AddedBy = Email,
                    AddedDate = DateTime.Now,
                    AdjustmentCodeID = null,
                    PaymentVisitID = null,
                    Resolved = false,
                    TickleDate = null
                };

                //data = (from d in database
                //        join c in Obj on d.VisitID equals c.VisitID
                //        join cpt in _context.Cpt on c.CPTID equals cpt.ID
                //        where cpt.CPTCode.Equals(CVisit.CPTCode)
                //        select d).Distinct().ToList();

                var q = (from paf in _clientContext.PlanFollowUp
                         where Obj.VisitID == paf.VisitID
                         select paf).FirstOrDefault();
                if (q == null)
                {
                    _clientContext.PlanFollowUp.Add(Obj);

                    List<Charge> listOfCharges = _clientContext.Charge.Where(c => c.VisitID == data.VisitID && c.PrimaryBal.Val() > 0 && c.IsSubmitted == true).ToList();

                    foreach (Charge charge in listOfCharges)
                    {
                        PlanFollowupCharge followupCharge = new PlanFollowupCharge()
                        {
                            ActionID = action,
                            AddedBy = Email,
                            AddedDate = DateTime.Now,
                            AdjustmentCodeID = null,
                            GroupID = group,
                            PaymentChargeID = null,
                            PlanFollowupID = Obj.ID,
                            ReasonID = reason,
                            RemarkCode1ID = null,
                            RemarkCode2ID = null,
                            RemarkCode4ID = null,
                            RemarkCode3ID = null,
                            TickleDate = null,
                            ChargeID = charge.ID
                        };
                        _clientContext.PlanFollowupCharge.Add(followupCharge);
                    }
                    inserted++;
                }

            }
            _clientContext.SaveChanges();

            VMAutoPlanFollowUp VmAutoPlanFollowUp = new VMAutoPlanFollowUp();
            VmAutoPlanFollowUp.PracticeID = PracticeId;
            VmAutoPlanFollowUp.AddedBy = Email;
            VmAutoPlanFollowUp.AddedDate = DateTime.Now;
            VmAutoPlanFollowUp.UpdatedDate = null;
            VmAutoPlanFollowUp.TotalRecords = database.Count;
            VmAutoPlanFollowUp.InsertedRecords = inserted;
            return VmAutoPlanFollowUp;
        }
        [Route("SaveAutoDownloading")]
        [HttpPost]
        public async Task<ActionResult<AutoDownloadingLog>> SaveAutoDownloading(AutoDownloadingLog AutoDownloading)
        {

            //UserInfoData UD = VMCommon.GetLoginUserInfo(_mainContext,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(AutoDownloading);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (AutoDownloading.ID == 0)
            {
                AutoDownloading.AddedBy = Email;
                AutoDownloading.AddedDate = DateTime.Now;
                //  AutoDownloading.PracticeID = UD.PracticeID;
                _mainContext.Add(AutoDownloading);
                await _mainContext.SaveChangesAsync();
            }
            else
            {
                AutoDownloading.UpdatedBy = Email;
                AutoDownloading.UpdatedDate = DateTime.Now;
                // AutoDownloading.PracticeID = UD.PracticeID;
                _mainContext.Update(AutoDownloading);
                await _mainContext.SaveChangesAsync();
            }
            //List<AutoDownloadingLog> logs = _mainContext.AutoDownloadingLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
            // var table = logs.ToHtmlTable();
            return Ok(AutoDownloading);
        }

        //[Route("AutoDownloadingLog")]
        //[HttpGet()]
        //public string AutoDownloadingLog()
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_mainContext,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

        //    //List<Models.Main.AutoPlanFollowUpLog> logs = _contextMain.AutoPlanFollowUpLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
        //    List<AutoLog> Download = (from Downloadlog in _mainContext.AutoDownloadingLog
        //                              join practice in _mainContext.MainPractice on Downloadlog.PracticeID equals practice.ID into TT
        //                              from TempTable in TT.DefaultIfEmpty()

        //                              where Downloadlog.AddedDate.Date == DateTime.Now.Date
        //                              select new AutoLog()
        //                              {
        //                                  ID = Downloadlog.ID,
        //                                  ClientID = TempTable.ClientID,
        //                                  PracticeID = TempTable.ID,
        //                                  PracticeName = TempTable.Name,
        //                                  ReceiverID = Downloadlog.ReceiverID

        //                              }).ToList();


        //    Download = (from d in Download
        //                join c in _clientContext.Client on d.ClientID equals c.ID
        //                select d).ToList();




        //    // var table = Download.ToHtmlTable();
        //    // return table;
        //}



        [Route("AutoDownloadingLog")]
        [HttpGet()]
        public string AutoDownloadingLog()
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_mainContext,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //List<Models.Main.AutoPlanFollowUpLog> logs = _contextMain.AutoPlanFollowUpLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
            var Download = (from Downloadlog in _mainContext.AutoDownloadingLog
                            join practice in _mainContext.MainPractice on Downloadlog.PracticeID equals practice.ID into TT
                            from TempTable in TT.DefaultIfEmpty()
                                //join Rece in _clientContext.Receiver on Downloadlog.ReceiverID equals Rece.ID into TT2
                                //from TempTable2 in TT2.DefaultIfEmpty()
                            where Downloadlog.AddedDate.Date == DateTime.Now.Date
                            select new
                            {
                                ID = Downloadlog.ID,
                                AddedDate = Downloadlog.AddedDate,
                                // ReceiverName = TT.FirstOrDefault().Name,
                                PracticeID = Downloadlog.PracticeID,
                                PracticeName = TempTable.Name,
                                ReciverID = Downloadlog.ReceiverID,
                                TotalDownloaded = Downloadlog.TotalDownloaded,
                                Files999 = Downloadlog.Files999,
                                Files835 = Downloadlog.Files835,
                                Files227 = Downloadlog.Files277,
                                FilesZip = Downloadlog.FilesZip,
                                FilesInsideZip = Downloadlog.FilesInsideZip,
                                Path = Downloadlog.Path,
                                Files999Procceed = Downloadlog.Files999Processed,
                                Files835Procceed = Downloadlog.Files835Processed,
                                Files227Procceed = Downloadlog.Files277Processed,
                                Addeddate = DateTime.Now.Date,
                                Updateddate = DateTime.Now.Date,
                                Addedby = Email,
                                Updatedby = Email,
                                LogMessage = Downloadlog.LogMessage,
                                Exception = Downloadlog.Exception,
                                SubmitterID = Downloadlog.SubmitterID
                            }).ToList();

            var a = (from d in Download
                     join r in _clientContext.Receiver on d.ReciverID equals r.ID into Table1
                     from rec in Table1.DefaultIfEmpty()
                     select new
                     {
                         ID = d.ID,
                         AddedDate = d.AddedDate,
                         PracticeID = d.PracticeID,
                         PracticeName = d.PracticeName,
                         ReciverID = d.ReciverID,
                         ReceiverName = rec.Name.IsNull() ? "" : rec.Name,
                         TotalDownloaded = d.TotalDownloaded,
                         Files999 = d.Files999,
                         Files835 = d.Files835,
                         Files227 = d.Files227,
                         FilesZip = d.FilesZip,
                         FilesInsideZip = d.FilesInsideZip,
                         Path = d.Path,
                         Files999Procceed = d.Files999Procceed,
                         Files835Procceed = d.Files835Procceed,
                         Files227Procceed = d.Files227Procceed,
                         Addeddate = DateTime.Now.Date,
                         Updateddate = DateTime.Now.Date,
                         Addedby = Email,
                         Updatedby = Email,
                         LogMessage = d.LogMessage,
                         Exception = d.Exception,
                         SubmitterID = d.SubmitterID
                     }
                ).ToList();

            var table = a.ToHtmlTable();
            return table;
        }




        //MEthod for Running Migration in All Clients///Currently  WOrking on it
        [Route("RunMigration")]
        [HttpPost]
        public void RunMigration()
        {
            var mainClient = (from u in _mainContext.MainClient
                              select u).ToList();
            foreach (var client in mainClient)
            {


                if (client.ID != null)
                {
                    // _clientContext.setDatabaseName(client.ContextName);
                    var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
                    string server = configuration["DatabaseSetting:server"].ToString();
                    string userid = configuration["DatabaseSetting:user id"].ToString();
                    string password = configuration["DatabaseSetting:password"].ToString();
                    optionsBuilder.UseSqlServer("server = " + server + " ; database = Medifusion" + client.ContextName + "; user id = " + userid + "; password = " + password + " ; ");
                    //optionsBuilder.UseSqlServer("server = 96.69.218.154\\SQLEXPRESS; database = Medifusion" + client.ContextName + "; user id = sa; password = Jay321@");
                    HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                    //HttpContextAccessor.HttpContext = this.HttpContext;
                    ClientDbContext NewContext = new ClientDbContext(optionsBuilder.Options, _config, httpContextAccessor, _env);
                    if (NewContext.Database == null)
                    {
                        return;
                    }
                    var cli = (from c in NewContext.Client where c.ID == client.ID select c).FirstOrDefault();
                    NewContext.Database.Migrate();

                }

            }
        }


        [HttpGet]
        [Route("InsertCheckInExtraColumn")]
        public void InsertCheckInExtraColumn()
        {
            var mainClient = (from u in _mainContext.MainClient select u).ToList();
            foreach (var client in mainClient)
            {


                if (client.ID != null)
                {
                    // _clientContext.setDatabaseName(client.ContextName);
                    var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
                    string server = configuration["DatabaseSetting:server"].ToString();
                    string userid = configuration["DatabaseSetting:user id"].ToString();
                    string password = configuration["DatabaseSetting:password"].ToString();
                    optionsBuilder.UseSqlServer("server = " + server + " ; database = Medifusion" + client.ContextName + "; user id = " + userid + "; password = " + password + " ; ");
                    //optionsBuilder.UseSqlServer("server = 96.69.218.154\\SQLEXPRESS; database = Medifusion" + client.ContextName + "; user id = sa; password = Jay321@");
                    HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                    //HttpContextAccessor.HttpContext = this.HttpContext;
                    ClientDbContext NewContext = new ClientDbContext(optionsBuilder.Options, _config, httpContextAccessor, _env);
                    if (NewContext.Database == null)
                    {
                        return;
                    }
                    List<PaymentCheck> CheckNum = NewContext.PaymentCheck.ToList();
                    foreach (PaymentCheck p in CheckNum)
                    {
                        p.ExtraColumn = p.CheckNumber;
                        NewContext.PaymentCheck.Update(p);
                    }
                    NewContext.SaveChanges();

                }

            }
        }


        [Route("DeactivateClient")]
        [HttpPost]
        public async Task<ActionResult<MainClient>> DeactivateClient(MainClient item)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_mainContext,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            if (item.ID <= 0 || item.ID.IsNull())
            {
                return BadRequest("Select Client First...");
            }


            else
            {

                item.IsDeactivated = item.IsDeactivated;
                item.DeactivationReason = item.DeactivationReason;
                item.DeactivationDate = DateTime.Now;
                item.DeactivateionAdditionalInfo = item.DeactivateionAdditionalInfo;
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _mainContext.MainClient.Update(item);
                await _mainContext.SaveChangesAsync();



                var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
                string server = configuration["DatabaseSetting:server"];
                string userid = configuration["DatabaseSetting:user id"];
                string password = configuration["DatabaseSetting:password"];
                optionsBuilder.UseSqlServer("server = " + server + " ; database = Medifusion" + item.ContextName + "; user id = " + userid + "; password = " + password + " ; ");
                //optionsBuilder.UseSqlServer("server = 96.69.218.154\\SQLEXPRESS; database = Medifusion" + item.ContextName + "; user id = sa; password = Jay321@");
                HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                ClientDbContext NewContext = new ClientDbContext(optionsBuilder.Options, _config, httpContextAccessor, _env);

                try
                {
                    Client client = new Client();
                    client = NewContext.Client.Where(c => c.ID == item.ID).FirstOrDefault();
                    if (client != null)
                    {
                        client.IsDeactivated = item.IsDeactivated;
                        client.DeactivationReason = item.DeactivationReason;
                        client.DeactivationDate = DateTime.Now;
                        client.DeactivateionAdditionalInfo = item.DeactivateionAdditionalInfo;
                        client.UpdatedBy = Email;
                        client.UpdatedDate = DateTime.Now;
                        _clientContext.Client.Update(client);
                        await NewContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    return Ok("Successfully");
                    // return BadRequest("Can Not Deactive Client..");
                }
            }
            return item;
        }


        ////MEthod for Updating Trigers in All Clients
        [Route("UpdateTriggers")]
        [HttpPost]
        public void UpdateTriggers()
        {


            var mainClient = (from u in _mainContext.MainClient select u).ToList();
            foreach (var client in mainClient)
            {
                if (client.ID != null)
                {
                    string server = configuration["DatabaseSetting:server"];
                    string userid = configuration["DatabaseSetting:user id"];
                    string password = configuration["DatabaseSetting:password"];
                    string sqlConnectionString = ("server = " + server + " ; database = Medifusion" + client.ContextName + "; user id = " + userid + "; password = " + password + " ; ");
                    //string sqlConnectionString = ("server = 96.69.218.154\\SQLEXPRESS; database = Medifusion" + client.ContextName + "; user id = sa; password = Jay321@");
                    string scriptTriger = System.IO.File.ReadAllText(Path.Combine(_clientContext.env.ContentRootPath, "Resources", "TriggerApi.sql"));
                    SqlConnection conn = new SqlConnection(sqlConnectionString);
                    Server sql = new Server(new ServerConnection(conn));
                    sql.ConnectionContext.ExecuteNonQuery(scriptTriger);
                }
            }

        }

        [Route("DuplicateCheckNum")]
        [HttpPost]
        public void CheckNumbers(string CheckNumbers)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            string connectionstring = CommonUtil.GetConnectionString(PracticeId, temp);
            //List<CheckList> chk = new List<CheckList>();
            //foreach (CheckList check in chk)
            //{
                using (SqlConnection myconnection = new SqlConnection(connectionstring))
                {
                    string ostring = "exec [dbo].[DeleteDuplicatePaymentChecks] @CheckNum =  " + CheckNumbers + " ;";
                    SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                    myconnection.Open();
                    using (SqlDataReader oreader = ocmd.ExecuteReader()) 
                    myconnection.Close();
                }
            //}


        }



        public class CheckList
        {
            public List<string> CheckNumbers { get; set; }

        }
    }
}