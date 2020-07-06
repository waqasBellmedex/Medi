using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RingCentral;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FaxController : ControllerBase
    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public IConfiguration Configuration { get; }


        public FaxController(ClientDbContext context, MainContext contextMain, IConfiguration configuration)
        {
            _context = context;
            _contextMain = contextMain;
            Configuration = configuration;
        }


        [Route("SendFax")]
        [HttpPost]
        public async Task<ActionResult> SendFax(FaxInput FaxData)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            // RING CENTRAL KEYS
            string RECIPIENT = FaxData.FaxNumber;
            string RINGCENTRAL_CLIENTID = Configuration["FaxSetting:RINGCENTRAL_CLIENTID"];
            string RINGCENTRAL_CLIENTSECRET = Configuration["FaxSetting:RINGCENTRAL_CLIENTSECRET"];
            string RINGCENTRAL_USERNAME = Configuration["FaxSetting:RINGCENTRAL_USERNAME"];
            string RINGCENTRAL_PASSWORD = Configuration["FaxSetting:RINGCENTRAL_PASSWORD"];
            string RINGCENTRAL_EXTENSION = Configuration["FaxSetting:RINGCENTRAL_EXTENSION"];


            string filename = FaxData.ReferralDocumentFileName;
            //"HIGH_LEVEL.docx";

            string referralFilePath = Path.Combine(_context.env.ContentRootPath, 
                    "Resources", "Physician Referrals", filename);

            Models.Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            if (settings == null)
            {
                return BadRequest("Document Server Settings Not Found");
            }

            string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, UD.ClientID.ToString(), "Fax", "Referrals", FaxData.Provider,
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));
            Directory.CreateDirectory(directoryPath);
            string outputFilePath = Path.Combine(directoryPath, filename);

            System.IO.File.Copy(referralFilePath, outputFilePath);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputFilePath, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex patNameRegex = new Regex("{PATIENT_NAME}");
                docText = patNameRegex.Replace(docText, FaxData.PatientName);

                patNameRegex = new Regex("{PATIENT_DOB}");
                docText = patNameRegex.Replace(docText, FaxData.PatientDOB);

                patNameRegex = new Regex("{SERVICE_NAME}");
                docText = patNameRegex.Replace(docText, FaxData.ServiceName);

                patNameRegex = new Regex("{SIGNATURE_DATE}");
                docText = patNameRegex.Replace(docText, DateTime.Now.Format("MM/dd/yyyy"));

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }

            RestClient rc = new RestClient(RINGCENTRAL_CLIENTID, RINGCENTRAL_CLIENTSECRET, false);
            await rc.Authorize(RINGCENTRAL_USERNAME, RINGCENTRAL_EXTENSION, RINGCENTRAL_PASSWORD);
            
            if (rc.token.access_token.Length > 0)
            {
                var requestParams = new CreateFaxMessageRequest();
                var attachment = new Attachment { fileName = "test.pdf", contentType = "application/pdf",
                    bytes = System.IO.File.ReadAllBytes(Path.Combine(_context.env.ContentRootPath, "Resources", "test.pdf")) };
                var attachments = new Attachment[] { attachment };
                requestParams.attachments = attachments;
                requestParams.to = new MessageStoreCalleeInfoRequest[] { new MessageStoreCalleeInfoRequest { phoneNumber = RECIPIENT } };
                
                requestParams.faxResolution = "High";
                requestParams.coverPageText = "Fax is being send by the BellMedex";
                var resp = await rc.Restapi().Account().Extension().Fax().Post(requestParams);
                Console.WriteLine("Fax sent. Message status: " + resp.messageStatus);
                
            }
            return Ok();
        }
    }
}