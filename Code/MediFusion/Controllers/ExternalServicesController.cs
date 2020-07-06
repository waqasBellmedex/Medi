using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using MediFusionPM.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plivo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    public class ExternalServicesController : Controller
    {


        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public IConfiguration configuration;
        public IHostingEnvironment _env;
        public IConfiguration _config;
        public ExternalServicesController(ClientDbContext context, MainContext contextMain, IHostingEnvironment env, IConfiguration Configuration)
        {
            _context = context;
            _contextMain = contextMain;
            configuration = Configuration;
            this._env = env;

        }
        [Route("SendAppointmentSMS")]
        [HttpPost]
        public ActionResult SendAppointmentSMS(SMSSentReceived sms)
        {

            string text = sms.textSent;
            string src = _config["SendingSMSSettings:SendingNumber"];
               // dst = "+12022214125";// Patient Number

            sms.sentFromNumber = src;
            sms.sentBy = "autoJobs";
            sms.sentDate = DateTime.Now;
            //sms.sentMessageApiReply = result.Message;
            //sms.apiId = result.ApiId;
            //sms.messageUuid = result.MessageUuid[0];
            //_context.SMSSentReceived.Add(sms);
            //_context.SaveChanges();

            var api = new PlivoApi(_config["SendingSMSSettings:AuthID"], _config["SendingSMSSettings:AuthToken"]);
            var result = api.Message.Create(
                src: src,
                dst: new List<String> { sms.sentToNumber },// +12069624548
                text: text
            );

            sms.sentMessageApiReply = result.Message;
            sms.apiId = result.ApiId;
            sms.messageUuid = result.MessageUuid[0];
            _contextMain.SMSSentReceived.Update(sms);

            PatientAppointment patientAppointment = _context.PatientAppointment.Find(sms.patientAppointmentID);
            if (patientAppointment != null && patientAppointment.ID > 0)
            {
                patientAppointment.Status = "8009"; // Reminder sent  
                _context.PatientAppointment.Update(patientAppointment);
            }

            _context.SaveChanges();
            return Ok(result);
        }


        [Route("UpdateAppointmentStatus/{ClientID}")]
        [HttpPost("{ClientID}")]
        public ActionResult UpdateAppointmentStatus(string ClientID, string Status, string AppointmentID)
        {

            ClientDbContext NewContext = CreateClientDbContext(long.Parse(ClientID));

            PatientAppointment patientAppointment = NewContext.PatientAppointment.Find(long.Parse(AppointmentID));
            Patient _Patient = NewContext.Patient.Find(patientAppointment.PatientID);
            Practice _Practice = NewContext.Practice.Find(ClientID);


            if (patientAppointment != null && patientAppointment.ID > 0)
            {
                patientAppointment.Status = Status; // confirmed
                NewContext.PatientAppointment.Update(patientAppointment);
            }

            NewContext.SaveChanges();
            string Action = "";
            string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/Email", "StatusUpdate.html"));
            StreamReader str = new StreamReader(statementHTML);
            string MailText = str.ReadToEnd();
            MailText = MailText.Replace("[PATIENTNAME]", _Patient.FirstName + " " + _Patient.LastName);
            MailText = MailText.Replace("[DATE]", patientAppointment.AppointmentDate.ToString());
            MailText = MailText.Replace("[INTERVAL]", patientAppointment.VisitInterval.ToString());
            MailText = MailText.Replace("[PROVIDERNAME]", _Practice.ProvFirstName + " " + _Practice.ProvLastName);
            if (patientAppointment.Status == "8001")
            {
                Action = "Confirmed";
            }
            if (patientAppointment.Status == "8008")
            {
                Action = "Cancelled";
            }
            if (patientAppointment.Status == "8004")
            {
                Action = "Re-Scheduled";
            }
            if (patientAppointment.Status == "8006")
            {
                Action = "To Be Late";
            }
            MailText = MailText.Replace("[Action]", Action);

            return Json(MailText);               
        }


        [Route("ReceiveAppointmentSMS")]
        [HttpPost]
        public ActionResult ReceiveAppointmentSMS(string response)
        {

            string from_number = Request.Form["From"];
            // Receiver's phone number
            string to_number = Request.Form["To"];
            // The text which was received
            string text = Request.Form["Text"];
            //string[] arrReply = text.Split(' ');
            SMSSentReceived sms = _contextMain.SMSSentReceived.Where(w => w.receivedFromNumber == from_number).OrderByDescending(o => o.sentDate).FirstOrDefault();
            if (sms != null && sms.ID > 0)
            {
                sms.receivedFromNumber = from_number;
                sms.receivedToNumber = to_number;
                sms.textReceived = text;
                sms.ReceivedBy = "autoJobs";
                sms.ReceivedDate = DateTime.Now;
                _contextMain.SMSSentReceived.Update(sms);

                ClientDbContext NewContext = CreateClientDbContext(sms.clientID.Value);
                if (NewContext.Database == null)
                {
                    return NotFound();
                }

                if (text != null && text.ToUpper() == "Y")
                {

                    PatientAppointment patientAppointment = NewContext.PatientAppointment.Find(sms.patientAppointmentID);
                    if (patientAppointment != null && patientAppointment.ID > 0)
                    {
                        patientAppointment.Status = "8001"; // confirmed
                        NewContext.PatientAppointment.Update(patientAppointment);
                    }
                    // NewContext.SaveChanges();
                }
                else if (text != null && text.ToUpper() == "UNSUB")
                {
                    Patient patient = NewContext.Patient.Find(sms.patientID);
                    if (patient != null && patient.ID > 0)
                    {
                        patient.sendAppointmentSMS = false;
                        NewContext.Patient.Update(patient);
                    }
                }
                NewContext.SaveChanges();
            }
            else
            {
                SMSSentReceived smsNew = new SMSSentReceived();
                smsNew.patientID = null;
                smsNew.patientAppointmentID = null;
                smsNew.clientID = null;
                smsNew.practiceID = null;
                smsNew.ReceivedBy = "autoJobs";
                smsNew.ReceivedDate = DateTime.Now;
                smsNew.receivedFromNumber = from_number;
                smsNew.receivedToNumber = to_number;
                smsNew.textReceived = text;
                _contextMain.SMSSentReceived.Add(smsNew);

            }
            _context.SaveChanges();
            return Ok("Message received");

        }

        [Route("CreateAppointmentFromCalendyl")]
        [HttpPost]
        public async Task<ActionResult> CreateAppointmentFromCalendylAsync([FromBody] string content)
        {
            string data = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                data = await reader.ReadToEndAsync();
            }

            long clientId = 1;

            //if (mainClient != null)
            //{ 
            PatientAppointmentsExternal appointment = new PatientAppointmentsExternal();
            ClientDbContext NewContext = CreateClientDbContext(clientId);
            if (NewContext.Database == null)
            {
                return NotFound();
            }
            appointment.addedDate = DateTime.Now;
            appointment.dataReceived = content + "@@" + data;
            NewContext.PatientAppointmentsExternal.Add(appointment);
            NewContext.SaveChanges();
            return Ok(appointment);
            //}
            // return NotFound();
        }

        private ClientDbContext CreateClientDbContext(long clientID)
        {
            var mainClient = (from u in _contextMain.MainClient
                              where u.ID == clientID
                              select u).FirstOrDefault();


            var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
            string server = configuration["DatabaseSetting:server"].ToString();
            string userid = configuration["DatabaseSetting:user id"].ToString();
            string password = configuration["DatabaseSetting:password"].ToString();
            optionsBuilder.UseSqlServer("server = " + server + " ; database = Medifusion" + mainClient.ContextName + "; user id = " + userid + "; password = " + password + " ; ");
            //optionsBuilder.UseSqlServer("server = 96.69.218.154\\SQLEXPRESS; database = Medifusion" + client.ContextName + "; user id = sa; password = Jay321@");
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            //HttpContextAccessor.HttpContext = this.HttpContext;
            ClientDbContext NewContext = new ClientDbContext(optionsBuilder.Options, configuration, httpContextAccessor, _env);
            NewContext.setDatabaseName(mainClient.ContextName);
            return NewContext;
        }
        [Route("AuthenticateGoogleSheet/{ClientID}/{practiceID}")]
        [HttpPost("{ClientID}/{practiceID}")]
        public ActionResult AuthenticateGoogleSheet(long ClientID, long practiceID)
        {
            ClientDbContext NewContext = CreateClientDbContext(ClientID);
            Practice _PracticeModel = NewContext.Practice.Find(practiceID);
         
            if (_PracticeModel.isGoogleSheetEnable == true)
            {
                UserCredential credential; 
                string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
                string ApplicationName = "Google Sheets API .NET Quickstart";
                string clientSecretSheet = string.Empty;
                clientSecretSheet = _PracticeModel.googleSheetSecret.ToString();

                byte[] byteArray = Encoding.ASCII.GetBytes(clientSecretSheet);
                MemoryStream stream = new MemoryStream(byteArray);

                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = _PracticeModel.googleSheetID.ToString();
                String range = "A:N";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(spreadsheetId, range);

                // Prints the names and majors of students in a sample spreadsheet:
                // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
                ValueRange response = request.Execute();

                return Json("Success");
            }
            return Json("Google Sheet is not enabled for this practice");
        }
        [Route("AuthenticateGoogleCalender/{ClientID}/{practiceID}")]
        [HttpPost("{ClientID}/{practiceID}")]
        public ActionResult AuthenticateGoogleCalender(long ClientID, long PracticeID)
        {

            ClientDbContext NewContext = CreateClientDbContext(ClientID);
            Practice _PracticeMoel = NewContext.Practice.Find(PracticeID);
            if (_PracticeMoel.isGoogleCalenderEnable == true)
            {
                string CalenderSecret = string.Empty;
                CalenderSecret = _PracticeMoel.googleCalenderSecret.ToString();

                string[] Scopes = { CalendarService.Scope.CalendarReadonly };
                string ApplicationName = "Google Calendar API .NET Quickstart";
                Google.Apis.Auth.OAuth2.UserCredential credential;
                Stream stream = new MemoryStream();
                byte[] byteArray = Encoding.UTF8.GetBytes(CalenderSecret);
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Position = 0;
                string credPath = PracticeID + "tokenCalender.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                return Json("Success");
            }
            return Json("Google calender is not enabled for this practice");
        }

        
        [Route("DownloadFormText/{ClientID}/{FormID}")]
        [HttpGet("{ClientID}/{FormID}")]
        public async Task<IActionResult> DownloadFormText(long ClientID, long FormID)
        {

            ClientDbContext NewContext = CreateClientDbContext(ClientID);
            if (NewContext.Database == null)
            {
                return NotFound();
            }

            Practice _Practice = NewContext.Practice.Where(s => s.ClientID == ClientID).SingleOrDefault();
            Settings settings = NewContext.Settings.Where(s => s.ClientID == ClientID).SingleOrDefault();
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
            settings.DocumentServerDirectory, _Practice.ID.ToString(), "ClinicalForm");


          

            ClinicalForms clinicalForms = NewContext.ClinicalForms.Find(FormID);
            if (clinicalForms != null)
            {
                string url = (DirectoryPath + "\\" + clinicalForms.url);
                if (!System.IO.File.Exists(url))
                {
                    return NotFound();
                }
                string formText = System.IO.File.ReadAllText(url);

                if (formText == null)
                {
                    return NotFound();

                }
                var ret = new { Name = clinicalForms.Name, Type = clinicalForms.Type, Text = formText };
                return Json(ret);

            }
            return NotFound();
        }

        [Route("SavePatientExternalForm/{ClientID}/{FormID}/{User}")]        
        [HttpPost("{ClientID}/{FormID}/{User}")]
        public IActionResult SavePatientExternalForm(long ClientID, long FormID,string User, PatientForms form)
        {

            ClientDbContext NewContext = CreateClientDbContext(ClientID);
            if (NewContext.Database == null)
            {
                return NotFound();
            }
            Practice _Practice = NewContext.Practice.Where(s => s.ClientID == ClientID).SingleOrDefault();


            form.PracticeID = _Practice.ID;
            form.UpdatedBy = User;
            form.UpdatedDate = DateTime.Now;            
            if (form.ID <= 0)
            {
                form.UpdatedBy = User;
                form.AddedDate = DateTime.Now;                
                _context.PatientForms.Add(form);
            }
            else
            {
                _context.PatientForms.Update(form);
            }
            _context.SaveChanges();

            return Ok(form);

        }



    }
}