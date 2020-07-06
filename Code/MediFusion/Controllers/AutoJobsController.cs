using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static MediFusionPM.ViewModels.VMElectronicSubmission;
using static MediFusionPM.ViewModels.VMPatientAppointment;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class AutoJobsController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public IConfiguration _config;

        public AutoJobsController(ClientDbContext context, MainContext contextMain, IConfiguration _config)
        {
            _context = context;
            _contextMain = contextMain;
            this._config = _config;
        }

        [AllowAnonymous]
        public async Task<ActionResult> DownloadFiles()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoDownloadingJobSetting:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoDownloadingJobSetting:email"];
                loginObj.password = _config["AutoDownloadingJobSetting:password"];

            //var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("email", "jobs@bellmedex.com"),
            //    new KeyValuePair<string, string>("password", "abc@123")
            //});
            string json = JsonConvert.SerializeObject(loginObj);
            string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var GetUserPracticesResponse = await httpClient.GetAsync("Common/GetAutoDownloadingPractices").Result.Content.ReadAsStringAsync();
            IEnumerable<DropDown> dropdown = JsonConvert.DeserializeObject<IEnumerable<DropDown>>(GetUserPracticesResponse);

            Console.WriteLine("total number of practices retrieved : " + dropdown.Count());


            //dropdown = dropdown.Where(dd => dd.ID == 1).ToList();
            int i = 0;
            foreach (DropDown TempDropDown in dropdown)
            {
                try
                {
                    var SwitchResponse = await httpClient.GetAsync("Account/SwitchPractice/" + TempDropDown.ID).Result.Content.ReadAsStringAsync();
                    Console.WriteLine("\n\nPractice id switched to " + TempDropDown.ID + " \n\n");
                    VMUser VMuser = JsonConvert.DeserializeObject<VMUser>(SwitchResponse);
                    token = VMuser.Token.ToString();
                    Console.WriteLine("VmUser obtained : " + VMuser.PracticeID + " - " + VMuser.PracticeName + "  with token : " + token);
                    if (VMuser != null)
                    {
                        if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                        {
                            httpClient.DefaultRequestHeaders.Remove("Authorization");
                        }
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                            var SubmitterResponse = await httpClient.GetAsync("Common/FindSubmitters?PracticeID=" + TempDropDown.ID.GetValueOrDefault()).Result.Content.ReadAsStringAsync();
                        IEnumerable<Submitter> Submitters = JsonConvert.DeserializeObject<IEnumerable<Submitter>>(SubmitterResponse);

                            if (Submitters != null)
                            {

                                foreach (Submitter TempSubmitter in Submitters)
                                {
                                    if (TempSubmitter != null)
                                    {
                                        if (TempSubmitter.AutoDownloading == true)
                                        {
                                            // Download Files Api
                                            var DownloadFilesReponseString = await httpClient.GetAsync("EDI/DownlaodFiles/" + TempSubmitter.ReceiverID).Result.Content.ReadAsStringAsync();
                                            
                                        }
                                    }
                                    else
                                    {
                                        dynamic LogInput = new ExpandoObject();
                                        LogInput.logmessage = "Submitter is not setup!";
                                        LogInput.PracticeID = VMuser.PracticeID;
                                        json = JsonConvert.SerializeObject(LogInput);
                                        var LogResponse = await httpClient.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                                    }
                                }
                            }
                            else
                            {
                                dynamic LogInput = new ExpandoObject();
                                LogInput.logmessage = "Submitter is not setup!";
                                LogInput.PracticeID = VMuser.PracticeID;
                                json = JsonConvert.SerializeObject(LogInput);
                                var LogResponse = await httpClient.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.StackTrace);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoDownloadingJobSetting:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoDownloadingJobSetting:email"];
                loginObj.password = _config["AutoDownloadingJobSetting:password"];

                string json = JsonConvert.SerializeObject(loginObj);
                string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                dynamic LogInput = new ExpandoObject();
                LogInput.Exception = "Something went wrong. \n" + ex.StackTrace;
                json = JsonConvert.SerializeObject(LogInput);
                var LogResponse = await httpClient.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                return BadRequest("Something went wrong");
            }
        }

        //[Route("TestSubmitClaimRoute")]
        //[HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SubmitClaims()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri("https://localhost:44306/api/");//new Uri(_config["AutoDownloadingJobSetting:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoDownloadingJobSetting:email"];
                loginObj.password = _config["AutoDownloadingJobSetting:password"];

                //var content = new FormUrlEncodedContent(new[]
                //{
                //    new KeyValuePair<string, string>("email", "jobs@bellmedex.com"),
                //    new KeyValuePair<string, string>("password", "abc@123")
                //});
                string json = JsonConvert.SerializeObject(loginObj);
                string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var GetUserPracticesResponse = await httpClient.GetAsync("Common/GetAutoClaimSubmissionPractices").Result.Content.ReadAsStringAsync();
                IEnumerable<DropDown> dropdown = JsonConvert.DeserializeObject<IEnumerable<DropDown>>(GetUserPracticesResponse);

                Console.WriteLine("total number of practices retrieved : " + dropdown.Count());

                //dropdown = dropdown.Where(dd => dd.ID == 1).ToList();
                int i = 0;
                foreach (DropDown TempDropDown in dropdown)
                {
                    try
                    {
                        var SwitchResponse = await httpClient.GetAsync("Account/SwitchPractice/" + TempDropDown.ID).Result.Content.ReadAsStringAsync();
                        Console.WriteLine("\n\nPractice id switched to " + TempDropDown.ID + " \n\n");
                        VMUser VMuser = JsonConvert.DeserializeObject<VMUser>(SwitchResponse);
                        token = VMuser.Token.ToString();
                        Console.WriteLine("VmUser obtained : " + VMuser.PracticeID + " - " + VMuser.PracticeName + "  with token : " + token);
                        if (VMuser != null)
                        {
                            if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                            {
                                httpClient.DefaultRequestHeaders.Remove("Authorization");
                            }
                            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                            var SubmitterResponse = await httpClient.GetAsync("Common/FindSubmitters?PracticeID=" + TempDropDown.ID.GetValueOrDefault()).Result.Content.ReadAsStringAsync();
                            IEnumerable<Submitter> Submitters = JsonConvert.DeserializeObject<IEnumerable<Submitter>>(SubmitterResponse);

                            if (Submitters != null)
                            {

                                foreach (Submitter TempSubmitter in Submitters)
                                {
                                    if (TempSubmitter != null)
                                    {
                                        if (TempSubmitter.AutoSubmission == true)
                                        {

                                            // Download Files Api-
                                            dynamic findEVisitParams = new ExpandoObject();
                                            findEVisitParams.ReceiverID = TempSubmitter.ReceiverID;
                                            string findEVisitJson = JsonConvert.SerializeObject(findEVisitParams);
                                            var TempResult = httpClient.PostAsync("ElectronicSubmission/FindVisits", new StringContent(findEVisitJson, Encoding.UTF8, "application/json")).Result;

                                            var GetEVisits = await TempResult.Content.ReadAsStringAsync();
                                            IEnumerable<GElectronicSubmission> Visits = JsonConvert.DeserializeObject<IEnumerable<GElectronicSubmission>>(GetUserPracticesResponse);
                                            List<long> VisitIds = Visits.Select(v => v.VisitID).ToList();

                                            dynamic SubmitVisitsParams = new ExpandoObject();
                                            findEVisitParams.Visits = VisitIds;
                                            string submitEVisitJson = JsonConvert.SerializeObject(findEVisitParams);

                                            var SubmitVisits = await httpClient.PostAsync("ElectronicSubmission/SubmitVisits", new StringContent(submitEVisitJson, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                                        }
                                    }
                                    else
                                    {
                                        dynamic LogInput = new ExpandoObject();
                                        LogInput.logmessage = "Submitter is not setup!";
                                        LogInput.PracticeID = VMuser.PracticeID;
                                        json = JsonConvert.SerializeObject(LogInput);
                                        var LogResponse = await httpClient.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                                    }
                                }
                            }
                            else
                            {
                                dynamic LogInput = new ExpandoObject();
                                LogInput.logmessage = "Submitter is not setup!";
                                LogInput.PracticeID = VMuser.PracticeID;
                                json = JsonConvert.SerializeObject(LogInput);
                                var LogResponse = await httpClient.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.StackTrace);
                        HttpClient httpClientForLog = new HttpClient();
                        httpClientForLog.Timeout = TimeSpan.FromHours(2);
                        httpClientForLog.BaseAddress = new Uri(_config["AutoDownloadingJobSetting:baseUrl"]);

                        dynamic loginObjForLog = new ExpandoObject();
                        loginObjForLog.email = _config["AutoDownloadingJobSetting:email"];
                        loginObjForLog.password = _config["AutoDownloadingJobSetting:password"];

                        string jsonForLog = JsonConvert.SerializeObject(loginObjForLog);
                        string tokenForLog = await httpClient.PostAsync("Account/Login", new StringContent(jsonForLog, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                        if (httpClientForLog.DefaultRequestHeaders.Contains("Authorization"))
                        {
                            httpClientForLog.DefaultRequestHeaders.Remove("Authorization");
                        }
                        httpClientForLog.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenForLog);

                        dynamic LogInput = new ExpandoObject();
                        LogInput.Exception = "Something went wrong. \n" + ex.StackTrace;
                        json = JsonConvert.SerializeObject(LogInput);
                        var LogResponse = await httpClientForLog.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                        return BadRequest("Something went wrong");
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoDownloadingJobSetting:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoDownloadingJobSetting:email"];
                loginObj.password = _config["AutoDownloadingJobSetting:password"];

                string json = JsonConvert.SerializeObject(loginObj);
                string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                dynamic LogInput = new ExpandoObject();
                LogInput.Exception = "Something went wrong. \n" + ex.StackTrace;
                json = JsonConvert.SerializeObject(LogInput);
                var LogResponse = await httpClient.PostAsync("AutoDownloadFile/SaveDownloadLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                return BadRequest("Something went wrong");
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> CreateAutoFollowups()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoFollowupJobSettings:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoFollowupJobSettings:email"];
                loginObj.password = _config["AutoFollowupJobSettings:password"];

            //var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("email", "jobs@bellmedex.com"),
            //    new KeyValuePair<string, string>("password", "abc@123")
            //});
            string json = JsonConvert.SerializeObject(loginObj);
            string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var GetUserPracticesResponse = await httpClient.GetAsync("Common/GetAutoFollowupPractices").Result.Content.ReadAsStringAsync();
            IEnumerable<DropDown> dropdown = JsonConvert.DeserializeObject<IEnumerable<DropDown>>(GetUserPracticesResponse);

            Console.WriteLine("total number of practices retrieved : " + dropdown.Count());
            //dropdown = dropdown.Where(dd => dd.ID == 1).ToList();
            int i = 0;
            foreach (DropDown TempDropDown in dropdown)
            {
                try
                {
                    var SwitchResponse = await httpClient.GetAsync("Account/SwitchPractice/" + TempDropDown.ID).Result.Content.ReadAsStringAsync();
                    Console.WriteLine("\n\nPractice id switched to " + TempDropDown.ID + " \n\n");
                    VMUser VMuser = JsonConvert.DeserializeObject<VMUser>(SwitchResponse);
                    token = VMuser.Token.ToString();
                    Console.WriteLine("VmUser obtained : " + VMuser.PracticeID + " - " + VMuser.PracticeName + "  with token : " + token);
                    if (VMuser != null)
                    {
                        if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                        {
                            httpClient.DefaultRequestHeaders.Remove("Authorization");
                        }
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        var CreateFollowUpResponse = await httpClient.PostAsync("Jobs/CreateFollowUp", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                        VMAutoPlanFollowUp folloupData = JsonConvert.DeserializeObject<VMAutoPlanFollowUp>(CreateFollowUpResponse);

                        dynamic Params = new ExpandoObject();
                        Params.LogMessage = "Stats of Auto Followup Process";
                        if (folloupData != null)
                        {
                            Params.PracticeID = VMuser.PracticeID;
                            Params.ClientID = VMuser.ClientID;
                            Params.TotalRecords = folloupData.TotalRecords;
                            Params.FollowUpCreated = folloupData.InsertedRecords;
                            Params.Addedby = folloupData.AddedBy;
                            Params.updateddate = folloupData.UpdatedDate;
                        }
                        json = JsonConvert.SerializeObject(Params);
                        var LogResponse = await httpClient.PostAsync("AutoPlanFollowUp/SaveFollowUpLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                        Console.WriteLine("Create followup method executed for practice id : " + TempDropDown.ID + " : with response : " + CreateFollowUpResponse + "\n\nIteration: " + i);
                    }
                    else
                    {
                        Console.WriteLine("\nUnable to get a VMUser\n");
                    }
                    Console.WriteLine("\n\n" + i + "   \n\n iteration number");
                }
                catch (Exception ex)
                {
                        dynamic LogInput = new ExpandoObject();
                        LogInput.Exception = "Something went wrong. \n" + ex.StackTrace;
                        json = JsonConvert.SerializeObject(LogInput);
                        var LogResponse = await httpClient.PostAsync("AutoPlanFollowUp/SaveFollowUpLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                        Debug.WriteLine(ex.StackTrace);
                    }
                    finally
                    {
                        i++;
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoFollowupJobSettings:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoFollowupJobSettings:email"];
                loginObj.password = _config["AutoFollowupJobSettings:password"];

                string json = JsonConvert.SerializeObject(loginObj);
                string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                dynamic LogInput = new ExpandoObject();
                LogInput.Exception = "Something went wrong. \n" + ex.StackTrace;
                json = JsonConvert.SerializeObject(LogInput);
                var LogResponse = await httpClient.PostAsync("AutoPlanFollowUp/SaveFollowUpLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                return BadRequest("An error occurred.");
            }
        }

        [Route("SendEmailAppointmentReminders")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendEmailAppointmentReminders()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoEmailAppointmentReminders:baseUrl"]);
                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoEmailAppointmentReminders:email"];
                loginObj.password = _config["AutoEmailAppointmentReminders:password"];
                string json = JsonConvert.SerializeObject(loginObj);
                string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var GetUserPracticesResponse = await httpClient.GetAsync("Common/GetAutoEmailAppointmentReminderPractices").Result.Content.ReadAsStringAsync();
                IEnumerable<DropDown> dropdown = JsonConvert.DeserializeObject<IEnumerable<DropDown>>(GetUserPracticesResponse);

                Console.WriteLine("total number of practices retrieved : " + dropdown.Count());
                //dropdown = dropdown.Where(dd => dd.ID == 1).ToList();
                int i = 0;
                foreach (DropDown TempDropDown in dropdown)
                {
                    try
                    {
                        var SwitchResponse = await httpClient.GetAsync("Account/SwitchPractice/" + TempDropDown.ID).Result.Content.ReadAsStringAsync();
                        Console.WriteLine("\n\nPractice id switched to " + TempDropDown.ID + " \n\n");
                        VMUser VMuser = JsonConvert.DeserializeObject<VMUser>(SwitchResponse);
                        token = VMuser.Token.ToString();
                        Console.WriteLine("VmUser obtained : " + VMuser.PracticeID + " - " + VMuser.PracticeName + "  with token : " + token);
                        if (VMuser != null)
                        {
                            if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                            {
                                httpClient.DefaultRequestHeaders.Remove("Authorization");
                            }
                            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                            dynamic patientObj = new ExpandoObject();
                            patientObj.accountNum = "";
                            patientObj.dob = "";
                            patientObj.firstName = "";
                            patientObj.fromDate = DateTime.Now;
                            patientObj.fromTime = null;
                            patientObj.lastName = "";
                            patientObj.locationID = "";
                            patientObj.providerID = "";
                            patientObj.status = null;
                            patientObj.toDate = null;
                            patientObj.toTime = null;
                            patientObj.visitReasonID = null;

                            json = JsonConvert.SerializeObject(patientObj);
                            var CreateFollowUpResponse = await httpClient.PostAsync("PatientAppointment/FindPatientAppointments", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                            dynamic dynJson = JsonConvert.DeserializeObject(CreateFollowUpResponse);
                            foreach (var item in dynJson)
                            {
                                string eamilid = item.emailID;
                                string patientName = item.patient;
                                string Date = item.start;
                                string interval = item.timeInterval;
                                string provider = item.provider;

                                SendEmail(_config, eamilid , patientName, Date , interval, provider,item.ClientID);

                                //string messagetext = "Dear Mr "+item.patient+ " Your appointment with provider " + item.Provider + " is scheduled at Date " + item.start + "Thanks";
                                SMSSentReceived sms = new SMSSentReceived();
                                sms.patientID = item.patientID;
                                sms.patientAppointmentID = item.id;
                                sms.clientID = item.clientID;
                                sms.practiceID = item.practiceID;
                                sms.sentToNumber = item.PhoneNumber;
                                sms.textSent = "";
                                _context.SMSSentReceived.Add(sms);
                                _context.SaveChanges();
                                string messagetext = "Dear Mr " + item.patient + ", Your appointment with provider " + item.Provider + " is scheduled at Date " + item.start + ". Your Appointment ID is "+sms.ID+". For Confirmation  Please Reply With Appointment ID Space Y. Regards: BellMedEX";
                                json = JsonConvert.SerializeObject(sms);
                                var Ssmsservice = await httpClient.PostAsync("SSMSService/SendAppointmentSMS", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        dynamic LogInput = new ExpandoObject();
                        LogInput.Exception = "Something went wrong. \n" + ex.StackTrace;
                        json = JsonConvert.SerializeObject(LogInput);
                        //var LogResponse = await httpClient.PostAsync("AutoPlanFollowUp/SaveFollowUpLog", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                        Debug.WriteLine(ex.StackTrace);
                    }
                    finally
                    {
                        i++;
                    }
                    //SendEmail(_config);

                }
                //SendEmail(_config);
                return Ok();
            }
            catch (Exception ex)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromHours(2);
                httpClient.BaseAddress = new Uri(_config["AutoEmailAppointmentReminders:baseUrl"]);

                dynamic loginObj = new ExpandoObject();
                loginObj.email = _config["AutoFollowupJobSettings:email"];
                loginObj.password = _config["AutoFollowupJobSettings:password"];

                string json = JsonConvert.SerializeObject(loginObj);
                string token = await httpClient.PostAsync("Account/Login", new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                return BadRequest("An error occurred.");
            }
        }
        public  void SendEmail(IConfiguration config, string ToEmail , string patientName , string Date , string interval , string provider, string ClientID)
        {
            string FromEmail = config.GetSection("AutoEmailAppointmentReminders").GetSection("FromEmail").Value;
            string Host = config.GetSection("AutoEmailAppointmentReminders").GetSection("Host").Value;
            string Pass = config.GetSection("AutoEmailAppointmentReminders").GetSection("FromPassword").Value;
            string CC = config.GetSection("AutoEmailAppointmentReminders").GetSection("CC").Value;

            //string FilePath = config.GetSection("AutoEmailAppointmentReminders").GetSection("FilePath").Value;
            //FilePath = FilePath+ "Resources\\statement\\Email_Template\\reminder.html";
            string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement/Email_Template", "reminder.html"));
            //C:\inetpub\wwwroot\medifusionNew\Resources\statement\Email_Template\reminder.html
            //string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/Email_Template", "reminder.html"));

            //StreamReader str = new StreamReader(statementHTML);
            //string MailText = str.ReadToEnd();
            statementHTML = statementHTML.Replace("[PATIENTNAME]", patientName);
            statementHTML = statementHTML.Replace("[DATE]", Date);
            statementHTML = statementHTML.Replace("[INTERVAL]", interval);
            statementHTML = statementHTML.Replace("[PROVIDERNAME]", provider);
            statementHTML = statementHTML.Replace("[ClientID]", ClientID);

            
            //str.Close();
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmail); //From Email Id  
            mailMessage.Subject = "Appointment Alert"; //Subject of Email  
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = statementHTML; //body or message of Email  

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
            smtp.Send(mailMessage); //sending Email 
        }
    }
}