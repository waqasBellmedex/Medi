using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Plivo.XML;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMEmail;
namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public IConfiguration _config;

        public EmailController(ClientDbContext context, MainContext contextMain, IConfiguration config)
        {
            _context = context;
            _contextMain = contextMain;
            _config = config;

        }


        public string sendEmail(VMEmail vMEmail)
        {
            string FromEmail = _config.GetSection("PracticeEmail").GetSection("FromEmail").Value;
            string Host = _config.GetSection("PracticeEmail").GetSection("Host").Value;
            string Pass = _config.GetSection("PracticeEmail").GetSection("FromPassword").Value;
            string ext = "";

            if (!size(vMEmail.attachmentRecord))
                return ("Attachment  Cannot be greater then 10.00 MB ");
            else
                {

                try
                {
                //if (vMEmail.CC == null || vMEmail.CC.Length == 0)
                //  vMEmail.CC[0] = _config.GetSection("AutoEmailAppointmentReminders").GetSection("CC").Value;




                MailMessage mailMessage = new MailMessage();
                // string test = "data:text/plain;base64,Z2V0dGluZyBEYXRhLi4uLi4=".Split(',').Last();
                // byte[] test1 = System.Convert.FromBase64String(test);
                //Attachment att=new Attachment(new MemoryStream(test1),"test", MediaTypeNames.Text.Plain) ;
                Attachment att = null;// new Attachment("");
                mailMessage.From = new MailAddress(FromEmail); //From Email Id  
                mailMessage.Subject = vMEmail.subject; //Subject of Email  
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = vMEmail.body; //body or message of Email 
                if (vMEmail.attachmentRecord.Count > 0)
                {
                    foreach (var item in vMEmail.attachmentRecord)
                    {if (item.attachment != "" && item.name != "")
                            {
                                int index = item.name.LastIndexOf(".");
                                if (index > 0)
                                    item.name = item.name.Substring(0, index);
                                ext = item.attachment.Split(';')[0].Split('/')[1];
                                string base64String = item.attachment.Split(',').Last();
                                byte[] Stbytes = System.Convert.FromBase64String(base64String);
                                if (ext.ToLower() == "png" || ext.ToLower() == "jpeg" || ext.ToLower() == "jpg")
                                    att = new Attachment(new MemoryStream(Stbytes), item.name + "." + ext, MediaTypeNames.Image.Jpeg);
                                else if (ext.ToLower() == "pdf")
                                    att = new Attachment(new MemoryStream(Stbytes), item.name + "." + ext, MediaTypeNames.Application.Pdf);
                                else if (ext.ToLower() == "zip")
                                    att = new Attachment(new MemoryStream(Stbytes), item.name + "." + ext, MediaTypeNames.Application.Zip);
                                else if (ext.ToLower() == "plain")
                                    att = new Attachment(new MemoryStream(Stbytes), item.name + ".txt", MediaTypeNames.Text.Plain);
                                //handel rar and other cases
                                mailMessage.Attachments.Add(att);
                            }
                    }

                }


                foreach (string ToEMailId in vMEmail.sendTo)
                {
                    mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                }
                if (vMEmail.CC.Length > 0)
                {

                    foreach (string CCEmail in vMEmail.CC)
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
                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);
                 //sending Email }
                        }
                catch(Exception ex)
                { return (ex.ToString()); }


                return "Ok";

            }
        }
        public void savelog(VMEmail vmEmail, string Email, long ClientID, long PracticeID)
        {
            string FromEmail = _config.GetSection("PracticeEmail").GetSection("FromEmail").Value;
            EmailHistory emailHistory = new EmailHistory();






            //Add Email_History data 
            emailHistory.sendfrom = FromEmail;
            emailHistory.practiceID = PracticeID;
            emailHistory.subject = vmEmail.subject;
            emailHistory.body = vmEmail.body;
            emailHistory.addedBy = Email;
            emailHistory.addeddate = DateTime.Now;
            _context.EmailHistory.Add(emailHistory);
            _context.SaveChanges();

            // Add Email_To data in which we place record for those people who have to sent Email
            foreach (var sendToitem in vmEmail.sendTo)
            {
                EmailTo emailTo = new EmailTo();
                emailTo.emailhistoryid = emailHistory.ID;
                emailTo.sendto = sendToitem;
                emailTo.addedby = Email;
                emailTo.addeddate = DateTime.Now;
                _context.EmailTo.Add(emailTo);
            }
            // Add Email_CC in which we place record for those people who have to CC Email

            if (vmEmail.CC != null && vmEmail.CC.Length > 0)
            {
                foreach (var ccitem in vmEmail.CC)
                {
                    EmailCC emailcc = new EmailCC();
                    emailcc.emailhistoryid = emailHistory.ID;
                    emailcc.CC = ccitem;
                    emailcc.addedby = Email;
                    emailcc.addeddate = DateTime.Now;
                    _context.EmailCC.Add(emailcc);
                }
            }


            // Add Email_Attachment in which we place record for those people who have to CC Email

            if (vmEmail.attachmentRecord != null && vmEmail.attachmentRecord.Count > 0)
            {
                foreach (var atttachmentitem in vmEmail.attachmentRecord)
                {if (atttachmentitem.attachment != "" && atttachmentitem.name != "")
                    {
                        EmailAttachments emailAttachment = new EmailAttachments();
                        Models.Settings settings = _context.Settings.Where(s => s.ClientID == ClientID).SingleOrDefault();
                        string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                  settings.DocumentServerDirectory, PracticeID.ToString(), "Email");//settings.DocumentServerURL

                        if (!Directory.Exists(DirectoryPath))
                        {
                            Directory.CreateDirectory(DirectoryPath);
                        }

                        var datetime = DateTime.Now.Year.ToString() + "\\\\" + DateTime.Now.Month.ToString() + "\\\\" + DateTime.Now.Day.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                        var datetimeurl = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                        string attachmenturl = "";

                        var finaldirectory = System.IO.Path.Combine(DirectoryPath, datetime);
                        if (!Directory.Exists(finaldirectory))
                        {
                            Directory.CreateDirectory(finaldirectory);
                        }
                        int index = atttachmentitem.name.LastIndexOf(".");
                        if (index > 0)
                            atttachmentitem.name = atttachmentitem.name.Substring(0, index);
                        string base64String = atttachmentitem.attachment.Split(',').Last();
                        string ext = atttachmentitem.attachment.Split(';')[0].Split('/')[1];
                        if (ext.ToLower() == "plain".ToLower())
                            ext = "txt";
                        attachmenturl = System.IO.Path.Combine(finaldirectory, datetimeurl + "_" + atttachmentitem.name + "." + ext).Replace(" ", "");





                        byte[] Stbytes = System.Convert.FromBase64String(base64String);
                        System.IO.File.WriteAllBytes(attachmenturl, Stbytes);
                        emailAttachment.attachmenturl = System.IO.Path.Combine(datetime, datetimeurl + "_" + atttachmentitem.name + "." + ext).Replace(" ", ""); ;
                        emailAttachment.emailhistoryid = emailHistory.ID;
                        emailAttachment.addedby = Email;
                        emailAttachment.addeddate = DateTime.Now;
                        emailAttachment.attachmentname = atttachmentitem.name = atttachmentitem.name + "." + ext;
                        _context.EmailAttachments.Add(emailAttachment);
                    }
                }
            }

            _context.SaveChanges();


        }


        public bool size(List<AttachmentRecord> attachmentRecord)
        {
            string base64String = "";
            byte[] Stbytes;
            long length = 0;
            foreach (var item in attachmentRecord)
            {
                base64String = item.attachment.Split(',').Last();
                Stbytes = System.Convert.FromBase64String(base64String);
                length = +Stbytes.Length;
            }
            if (length <= (1000000 * 10))
                return true;
            else
                return false;
        }

        [HttpPost]
        [Route("FindEmail")]



        public  ActionResult FindEmail(SEmail sEmail)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
     User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
     User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var data = (from eh in _context.EmailHistory
                            //join et in _context.EmailTo on eh.ID equals et.emailhistoryid into etTable
                            //from etT in etTable.DefaultIfEmpty()
                            //join eA in _context.EmailAttachments on eh.ID equals eA.emailhistoryid into eATable
                            //from eAT in eATable.DefaultIfEmpty()
                            //join eC in _context.EmailCC on eh.ID equals eC.emailhistoryid into eCTable
                            //from eCT in eCTable.DefaultIfEmpty()
                        where
                        eh.practiceID == UD.PracticeID &&
                        (sEmail.sendFrom.IsNull() || sEmail.sendFrom.Length == 0 ? true : sEmail.sendFrom.Contains(eh.sendfrom)) &&
                         (sEmail.subject.IsNull() || sEmail.subject.Length == 0 ? true : sEmail.subject.Contains(eh.subject)) &&
                          (sEmail.body.IsNull() || sEmail.body.Length == 0 ? true : sEmail.body.Contains(eh.body)) 
                           //(sEmail.sendTo.IsNull() || sEmail.sendTo.Length == 0 ? true : sEmail.body.Contains(etT.sendto)) &&
                           // (sEmail.CC.IsNull() || sEmail.CC.Length == 0 ? true : sEmail.body.Contains(eCT.CC)) &&
                           // (sEmail.attachment.IsNull() || sEmail.attachment.Length == 0 ? true : sEmail.body.Contains(eAT.attachmentname))
                        select new
                        {eh.ID,
                            eh.sendfrom,
                            eh.subject,
                            eh.addeddate,
                            eh.body,
             

                        }).ToList();
            var emailData = data.Select(
                                 e =>
                                   new
                                   {
                                       sendfrom = e.sendfrom,
                                       subject = e.subject,
                                       addeddate = e.addeddate,
                                       body = e.body,
                                       sendTo = (e.ID > 0 ? getSendTo(e.ID, sEmail.sendTo) : null),
                                       CC = (e.ID > 0 ? getCC(e.ID, sEmail.CC) : null),
                                       attachment = (e.ID > 0 ? getAttachments(e.ID, sEmail.attachment) : null)


                                   }
                                 );
            if (emailData == null)
            {
                return NotFound();
            }
            var lst = emailData.ToList();
            return Ok(lst);
           



        }
        private List<string> getSendTo(long emailHistoryid,string sendTo )
        {
          return  (from et in _context.EmailTo
                                
                        where
                        et.emailhistoryid == emailHistoryid
                        &&
                        (sendTo.IsNull() || sendTo.Length == 0 ? true : sendTo.Contains(et.sendto))
                                select et ).Select(S=>S.sendto ).ToList();
             
        }
        private List<string> getCC(long emailHistoryid, string CC)
        {
            return (from ec in _context.EmailCC

                    where
                    ec.emailhistoryid == emailHistoryid
                    &&
                    (CC.IsNull() || CC.Length == 0 ? true : CC.Contains(ec.CC))
                    select ec).Select(S => S.CC).ToList();

        }

        private List<VMAttachments> getAttachments(long emailHistoryid, string attachmemt)
        {
            var data = (from eA in _context.EmailAttachments

                        where
                    eA.emailhistoryid == emailHistoryid
                    &&
                    (attachmemt.IsNull() || attachmemt.Length == 0 ? true : attachmemt.Contains(eA.attachmentname))
                        select new VMAttachments
                        {
                            attachment = eA.attachmentname,
                            ID = eA.ID,
                            attachmenturl=eA.attachmenturl
                        }).ToList();
            return data;




        }
        [HttpPost]
        [Route("DownloadAttachment")]

        public ActionResult DownloadAttachment(DdocumentAll document)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            CommonController obj = new CommonController(_context, _contextMain);

            var stream = obj.DownloadAllDocument(UD, "Email", document.document_address);
            return stream;



        }





    }
}
