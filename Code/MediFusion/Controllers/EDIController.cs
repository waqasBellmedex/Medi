using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.BusinessLogic;
using MediFusionPM.BusinessLogic._277CA;
using MediFusionPM.BusinessLogic._999;
using MediFusionPM.BusinessLogic.ClaimGeneration;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMAction;
using static MediFusionPM.ViewModels.VMCommon;
using Action = MediFusionPM.Models.Action;
using MediFusionPM.BusinessLogic.EraParsing;
using MediFusionPM.Models.TodoApi;


namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class EDIController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public EDIController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        [Route("DownlaodFiles/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AutoDownloadingLog>> DownlaodFiles(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);
            AutoDownloadingLog data = new AutoDownloadingLog();

            Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
            if (settings == null)
            {
                return BadRequest("Document Server Settings Not Found");
            }

            Submitter submitter = _context.Submitter.Where(m => m.ReceiverID == id && m.ClientID == UD.ClientID).SingleOrDefault();
            if (submitter == null)
            {
                return BadRequest("Submitter is not Setup");
            }

            var headerData = await
                    (from rec in _context.Receiver
                     join sub in _context.Submitter
                     on rec.ID equals sub.ReceiverID
                     join cl in _context.Client
                     on sub.ClientID equals cl.ID
                     where
                     cl.ID == UD.ClientID &&
                     rec.ID == id
                     select new
                     {
                         SubmitterID = sub.ID,
                         ClientID = sub.ClientID,
                         ReceiverOrgName = rec.X12_837_NM1_40_ReceiverName,
                         SFTPModel = new SFTPModel()
                         {
                             FTPHost = rec.SubmissionURL,
                             FTPPort = rec.SubmissionPort,
                             FTPUserName = sub.SubmissionUserName,
                             FTPPassword = sub.SubmissionPassword,
                             SubmitToFTP = !sub.ManualSubmission,
                             ConnectivityType = rec.SubmissionMethod,
                             SubmitDirectory = rec.SubmissionDirectory,
                             //FileName = string.Format(sub.FileName, DateTime.Now.ToString("hhmmss"))
                             FileName = sub.FileName,
                             DownloadDirectory = rec.ReportsDirectory
                         },
                         SubmitterOrgName = sub.X12_837_NM1_41_SubmitterName
                     }).SingleOrDefaultAsync();

            SFTPModel sftp = headerData.SFTPModel;

            if (sftp == null || sftp.SubmitToFTP == false)
            {
                return BadRequest("SFTP Setup is not complete.");
            }

            if (sftp.FTPHost.IsNull2() || sftp.FTPPassword.IsNull2()
                    || sftp.FTPPort.IsNull2() || sftp.FTPUserName.IsNull2() ||
                   sftp.DownloadDirectory.IsNull2())
            {
                return BadRequest("FTP Host, User Name, Password, Port can not be Empty.");
            }


            SFTPSubmission obj = new SFTPSubmission(sftp.FTPHost, sftp.FTPUserName, sftp.FTPPassword,
                int.Parse(sftp.FTPPort), sftp.ConnectivityType);

            string DownloadDirectory = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory,
                        headerData.ReceiverOrgName, headerData.SubmitterOrgName,
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            string AllDownloads = Path.Combine(DownloadDirectory, "DOWNLOADS");
            Directory.CreateDirectory(AllDownloads);
            obj.DownloadFiles(sftp.DownloadDirectory, AllDownloads);

            string zipFilePath = Path.Combine(DownloadDirectory, "alldownloads.zip");
            ZipFile.CreateFromDirectory(AllDownloads, zipFilePath);

            // Extracting the zip files
            long totalFilesBeforeUnZip = Directory.GetFiles(AllDownloads).Length;
            long zipFiles = 0;
            foreach (string f in Directory.GetFiles(AllDownloads))
            {
                if (f.Contains("alldownloads.zip")) continue;
                string extentsion = Path.GetExtension(f).Replace(".", "");
                if (extentsion == "zip" || extentsion == "rar")
                {
                    ZipFile.ExtractToDirectory(f, AllDownloads, true);
                    zipFiles += 1;
                }
            }
            long totalFilesAfterZip = Directory.GetFiles(AllDownloads).Length;

            int filesCount = 0;
            foreach (string f in Directory.GetFiles(AllDownloads))
            {
                string extentsion = Path.GetExtension(f).Replace(".", "");
                if (extentsion == "zip" || extentsion == "rar")
                    continue;

                filesCount++;
            }

            data.ReceiverID = id;
            data.SubmitterID = submitter.ID;
            data.PracticeID = UD.PracticeID;
            data.Path = AllDownloads;
            data.LogMessage = "Submitter Id Not Setup !";
            data.TotalDownloaded = totalFilesBeforeUnZip;
            data.FilesZip = zipFiles;
            data.FilesInsideZip = totalFilesAfterZip - totalFilesBeforeUnZip;


            ReportsLog log = new ReportsLog()
            {
                AddedBy = UD.Email,
                AddedDate = DateTime.Now,
                ClientID = UD.ClientID,
                ReceiverID = id,
                Processed = false,
                UserResolved = false,
                SubmitterID = headerData.SubmitterID,
                ZipFilePath = zipFilePath,
                FilesCount = filesCount
            };
            _context.ReportsLog.Add(log);
            _context.SaveChanges();
            _contextMain.AutoDownloadingLog.Add(data);
            _contextMain.SaveChanges();

            foreach (string file in Directory.GetFiles(AllDownloads))
            {
                string extentsion = Path.GetExtension(file).Replace(".", "");
                if (extentsion == "zip" || extentsion == "rar")
                    continue;

                string fileType = Utilities.GetFileType(file);
                if (fileType == "999") data.Files999 += 1;
                else if (fileType == "277CA") data.Files277 += 1;
                else if(fileType == "277") data.Files277 += 1;
                else if (fileType == "835") data.Files835 += 1;
                else if (fileType == "HR") data.FilesOther += 1;
                if (!fileType.IsNull2())
                {
                    string directory = Path.Combine(DownloadDirectory, fileType);
                    Directory.CreateDirectory(directory);
                    string sourceFile = Path.Combine(directory, Path.GetFileName(file));
                    System.IO.File.Copy(file, sourceFile);

                    DownloadedFile downloadedFile = new DownloadedFile()
                    {
                        AddedBy = UD.Email,
                        AddedDate = DateTime.Now,
                        FileType = fileType,
                        FilePath = sourceFile,
                        Processed = false,
                        ReportsLogID = log.ID
                    };
                    _context.DownloadedFile.Add(downloadedFile);
                    _context.SaveChanges();
                }
            }

            obj.DeleteDownloadFiles(sftp.DownloadDirectory, "", AllDownloads);

            //Process999(UD.Email);
            //Process277CA(UD.Email);
            //Process835(UD.Email);

            string FilePath = string.Empty;
            try
            {
                List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "999" && d.Processed == false).ToList();
                //if (files != null || files.Count > 0) 

                foreach (DownloadedFile file in files)
                {
                    try
                    {
                        FilePath = file.FilePath;

                        _999Parser parser = new _999Parser();
                        List<_999Header> header = parser.Parse999File(FilePath);

                        foreach (_999Header H in header)
                        {
                            foreach (_999TransactionSet ST in H.ListOfTransactions)
                            {
                                SubmissionLog submitLog = _context.SubmissionLog.Where(m => m.ISAControlNumber == ST.AK1_02_GroupControlNum).SingleOrDefault();
                                if (submitLog != null)
                                {
                                    submitLog.AK9_Status = H.AK9_01_BatchAcknowledgmentCode;
                                    submitLog.NoOfTotalST = H.AK9_02_NoOfTotalST.Val();
                                    submitLog.NoOfAcceptedST = H.AK9_04_NoOfAcceptedST.Val();
                                    submitLog.NoOfReceivedST = H.AK9_03_NoOfReceivedST.Val();
                                    submitLog.IK5_Status = ST.IK5_01_ST_AcknowledgmentCode;
                                    submitLog.AK9_ErrorCode = "";
                                    submitLog.Transaction999Path = FilePath;
                                    submitLog.DownloadedFileID = file.ID;
                                    submitLog.UpdatedBy = UD.Email;
                                    submitLog.UpdatedDate = DateTime.Now;
                                    _context.SubmissionLog.Update(submitLog);

                                    string status = string.Empty;
                                   
                                    if (submitLog.IK5_Status == "A" && submitLog.AK9_Status == "A") status = "K";
                                    else if (submitLog.IK5_Status == "R" && submitLog.AK9_Status == "R") status = "D";
                                    else if (submitLog.IK5_Status == "E" && submitLog.AK9_Status == "E") status = "E";

                                    List<Visit> visits = _context.Visit.Where(m => m.SubmissionLogID == submitLog.ID).ToList();
                                    foreach (Visit v in visits)
                                    {
                                        if (v.PrimaryStatus == "S")
                                        {
                                            v.PrimaryStatus = status;
                                            _context.Visit.Update(v);
                                        }
                                    }
                                }
                            }
                        }

                        file.Processed = true;
                        data.Files999Processed += 1;
                    }
                    catch (Exception ex)
                    {
                        //file.Exception = ex.ToString();
                    }
                    finally
                    {
                        _context.DownloadedFile.Update(file);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(AllDownloads, "Exception999.txt"), ex.ToString());
            }

            try
            {
                List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "277CA" && d.Processed == false).ToList();
                //if (files == null || files.Count == 0) return;

                //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                //{
                foreach (DownloadedFile file in files)
                {
                    try
                    {
                        FilePath = file.FilePath;
                        if (FilePath.Contains("Exception.txt")) continue;
                        ClaimAckParser parser = new ClaimAckParser();
                        List<ClaimAckHeader> header = parser.Parse277CAFile(FilePath);

                        _context.VisitStatusLog.Add(new VisitStatusLog()
                        {
                            AddedBy = UD.Email,
                            AddedDate = DateTime.Now,
                            DownloadedFileID = file.ID,
                            Transaction276Path = null,
                            Transaction277CAPath = FilePath,
                            Transaction277Path = null,
                            Transaction999Path = null
                        });

                        foreach (ClaimAckHeader H in header)
                        {
                            foreach (ClaimAckVisit V in H.ClaimAckVisits)
                            {
                                VisitStatus visitStatus = new VisitStatus()
                                {
                                    AddedBy = UD.Email,
                                    AddedDate = DateTime.Now,
                                    SubmitterTRN = H.SubmitterTRN,
                                    ResponseEntity = H.PayerEntity,
                                    BilledAmount = V.VisitClaimAmt1,
                                    CategoryCode1 = V.VisitCategoryCode1,
                                    CategoryCodeDesc1 = V.VisitCategoryCodeDesc1,
                                    CategoryCode2 = V.VisitCategoryCode2,
                                    CategoryCodeDesc2 = V.VisitCategoryCodeDesc2,
                                    CategoryCode3 = V.VisitCategoryCode3,
                                    CategoryCodeDesc3 = V.VisitCategoryCodeDesc3,
                                    CheckDate = null,
                                    CheckNumber = null,
                                    DOS = null,
                                    StatusCode1 = V.VisitStatusCode1,
                                    StatusCodeDesc1 = V.VisitStatusCodeDesc1,
                                    StatusCode2 = V.VisitStatusCode2,
                                    StatusCodeDesc2 = V.VisitStatusCodeDesc2,
                                    StatusCode3 = V.VisitStatusCode3,
                                    StatusCodeDesc3 = V.VisitStatusCodeDesc3,
                                    EntityCode1 = V.VisitEntityCode1,
                                    EntityCodeDesc1 = V.VisitEntityCodeDesc1,
                                    EntityCode2 = V.VisitEntityCode2,
                                    EntityCodeDesc2 = V.VisitEntityCodeDesc2,
                                    EntityCode3 = V.VisitEntityCode3,
                                    EntityCodeDesc3 = V.VisitEntityCodeDesc3,
                                    ErrorMessage = "",
                                    PatientLN = V.PatientLastName,
                                    PatientFN = V.PatientFirstName,
                                    SubscriberID = V.PatientSubscriberID,
                                    SubscriberFN = V.PatientFirstName,
                                    SubscriberLN = V.PatientLastName,
                                    TRNNumber = V.VisitTRN,
                                    ActionCode = V.VisitActionCode1,
                                    Status = V.Status,
                                    StatusDate = V.VisitStatusDate1,
                                    PayerID = H.PayerID,
                                    PayerName = H.PayerOrgName,
                                    PayerControlNumber = V.PayerClaimControlNum,
                                    FreeText1 = V.VisitRejection1,
                                    FreeText2 = V.VisitRejection2,
                                    FreeText3 = V.VisitRejection3,
                                    RejectionReason1 = V.VisitActionCode1 == "U" ?
                                                        (V.VisitStatusCodeDesc1.Contains("Entity") && !V.VisitEntityCodeDesc1.IsNull2() ?
                                                        V.VisitStatusCodeDesc1 + " (" + V.VisitEntityCodeDesc1 + ")" : V.VisitStatusCodeDesc1) : V.VisitStatusCodeDesc1,
                                    RejectionReason2 = V.VisitActionCode2 == "U" ?
                                                        (V.VisitStatusCodeDesc2.Contains("Entity") && !V.VisitEntityCodeDesc2.IsNull2() ?
                                                        V.VisitStatusCodeDesc2 + " (" + V.VisitEntityCodeDesc2 + ")" : V.VisitStatusCodeDesc2) : V.VisitStatusCodeDesc2,
                                    RejectionReason3 = V.VisitActionCode3 == "U" ?
                                                        (V.VisitStatusCodeDesc3.Contains("Entity") && !V.VisitEntityCodeDesc3.IsNull2() ?
                                                        V.VisitStatusCodeDesc3 + " (" + V.VisitEntityCodeDesc3 + ")" : V.VisitStatusCodeDesc3) : V.VisitStatusCodeDesc3,
                                    VisitAmount = V.VisitClaimAmt1,
                                    ProviderFN = V.BillProvFirstName,
                                    ProviderLN = V.BillProvOrgName,
                                    ProviderNPI = V.BillProvNPI
                                };

                                //if (visitStatus.ActionCode == "U" && !visitStatus.RejectionReason1.IsNull2())
                                //{
                                //    visitStatus.RejectionReason1 = V.VisitStatusCodeDesc1;
                                //}

                                Visit visit = null;

                                string[] temp = null;
                                if (V.VisitTRN.Contains(" "))
                                    temp = V.VisitTRN.Split(' ');
                                else
                                    temp = V.VisitTRN.Split('_');

                                if (temp != null && temp.Length >= 2)
                                {
                                    visit = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).SingleOrDefault();
                                    if (visit != null)
                                    {
                                        visitStatus.VisitID = visit.ID;
                                        visitStatus.PracticeID = visit.PracticeID;
                                        visitStatus.ProviderID = visit.ProviderID;
                                        visitStatus.LocationID = visit.LocationID;
                                        visitStatus.PatientPlanID = visit.PrimaryPatientPlanID;

                                        string Status = string.Empty, rejectionReason1 = string.Empty, freeText = string.Empty;
                                        if (V.VisitActionCode1 == "U")
                                        {
                                            Status = "R";

                                            if (!visitStatus.FreeText1.IsNull2()) freeText = visitStatus.FreeText1;
                                            if (!visitStatus.FreeText2.IsNull2()) freeText += Environment.NewLine + visitStatus.FreeText2;
                                            if (!visitStatus.FreeText3.IsNull2()) freeText += Environment.NewLine + visitStatus.FreeText3;

                                            if (!visitStatus.RejectionReason1.IsNull2()) rejectionReason1 = visitStatus.RejectionReason1;
                                            if (!visitStatus.RejectionReason2.IsNull2()) rejectionReason1 += Environment.NewLine + visitStatus.RejectionReason2;
                                            if (!visitStatus.RejectionReason3.IsNull2()) rejectionReason1 += Environment.NewLine + visitStatus.RejectionReason3;
                                        }
                                        else if (V.VisitActionCode1 == "WQ")
                                        {
                                            Status = V.VisitCategoryCode1 + H.PayerEntity;
                                        }

                                        //if (!Status.IsNull2() && ( visit.PrimaryStatus == "S" || visit.PrimaryStatus == "K" || visit.PrimaryStatus == "E" ||
                                        //    visit.PrimaryStatus == "D"))
                                        //{
                                            visit.PrimaryStatus = Status;
                                        //}

                                        //if (!rejectionReason1.IsNull2())
                                        //{
                                        //    visit.RejectionReason = rejectionReason1;
                                        //}

                                        if (!freeText.IsNull2())
                                        {
                                            visit.RejectionReason = freeText;
                                        }

                                        if (!V.PayerClaimControlNum.IsNull2())
                                            visit.PayerClaimControlNum = V.PayerClaimControlNum;

                                        _context.Visit.Update(visit);
                                    }
                                }

                                if (!H.SubmitterTRN.IsNull2())
                                {
                                    SubmissionLog submitLog = _context.SubmissionLog.Where(m => m.ISAControlNumber == H.SubmitterTRN).SingleOrDefault();
                                    if (submitLog != null)
                                    {
                                        submitLog.Trasaction277CAPath = FilePath;
                                        submitLog.UpdatedBy = UD.Email;
                                        submitLog.UpdatedDate = DateTime.Now;
                                        _context.SubmissionLog.Update(submitLog);
                                    }
                                }

                                _context.VisitStatus.Add(visitStatus);

                                foreach (ClaimAckCharge C in V.ClaimAckCharges)
                                {
                                    ChargeStatus chargeStatus = new ChargeStatus()
                                    {
                                        VisitStatusID = visitStatus.ID,
                                        AddedBy = UD.Email,
                                        AddedDate = DateTime.Now,
                                        BilledAmount = C.ChargeAmt,
                                        CategoryCode1 = C.ChargeCategoryCode1,
                                        CategoryCode2 = C.ChargeCategoryCode2,
                                        CategoryCode3 = C.ChargeCategoryCode3,
                                        EntityCode1 = C.ChargeEntityCode1,
                                        EntityCode2 = C.ChargeEntityCode2,
                                        EntityCode3 = C.ChargeEntityCode3,
                                        StatusCode1 = C.ChargeStatusCode1,
                                        StatusCode2 = C.ChargeStatusCode2,
                                        StatusCode3 = C.ChargeStatusCode3,
                                        DOS = C.ServiceDateFrom,
                                        CPT = C.CPT,
                                        Modifier1 = C.Modifier1,
                                        Modifier2 = C.Modifier2,
                                        Modifier3 = C.Modifier3,
                                        Modifier4 = C.Modifier4,
                                        RejectionReason1 = C.ChargeRejection1,
                                        RejectionReason2 = C.ChargeRejection2,
                                        RejectionReason3 = C.ChargeRejection3
                                    };
                                    _context.ChargeStatus.Add(chargeStatus);

                                    if (visit != null)
                                    {
                                        Charge charge = _context.Charge.Where(c => c.VisitID == visit.ID && c.Cpt.CPTCode == chargeStatus.CPT).SingleOrDefault();
                                        if (charge != null)
                                        {
                                            if (visitStatus.ActionCode == "U" || C.ChargeActionCode1 == "U")
                                            {
                                                string rejReason = "";
                                                if (!C.ChargeRejection1.IsNull2()) rejReason = C.ChargeRejection1;
                                                if (!C.ChargeRejection2.IsNull2()) rejReason += Environment.NewLine + C.ChargeRejection2;
                                                if (!C.ChargeRejection3.IsNull2()) rejReason += Environment.NewLine + C.ChargeRejection3;

                                                charge.RejectionReason = rejReason;
                                                charge.PrimaryStatus = "R";

                                                _context.Charge.Update(charge);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        file.Processed = true;
                        data.Files277Processed += 1;
                    }
                    catch (Exception ex)
                    {
                        //file.Exception = ex.ToString();
                    }
                    finally
                    {
                        _context.DownloadedFile.Update(file);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(AllDownloads, "Exception277CA.txt"), ex.ToString());
            }

            int duplicateCheckCount = 0;
            List<PaymentCheck> importedChecks = new List<PaymentCheck>();
            try
            {
                List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "835" && d.Processed == false).ToList();
                foreach (DownloadedFile file in files)
                {
                    try
                    {
                        ERAParser eraParser = new ERAParser();
                        List<ERAHeader> EraData = eraParser.ParseERAFile(file.FilePath);
                        long? defaultActionID = _context.Action.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
                        long? defaultGroupID = _context.Group.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
                        long? defaultReasonID = _context.Reason.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
                        foreach (ERAHeader H in EraData)
                        {
                            bool CheckExists = _context.PaymentCheck.Count(p => p.CheckNumber == H.CheckNumber && p.PracticeID == UD.PracticeID) > 0;
                            if (CheckExists)
                            {
                                duplicateCheckCount += 1;
                                continue;
                            }
                            Practice practice = _context.Practice.Where(f => f.NPI == H.PayeeNPI).FirstOrDefault();
                            if (practice == null)
                                practice = _context.Practice.Where(f => f.TaxID == H.PayeeTaxID).FirstOrDefault();
                            long? PracticeID = practice?.ID;
                            if (PracticeID == null)
                            {
                                foreach (ERAVisitPayment V in H.ERAVisitPayments)
                                {
                                    string[] temp = null;
                                    if (V.PatientControlNumber.Contains(" "))
                                        temp = V.PatientControlNumber.Split(' ');
                                    else
                                        temp = V.PatientControlNumber.Split('_');
                                    if (temp != null && temp.Length >= 2)
                                    {
                                        PracticeID = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).FirstOrDefault()?.PracticeID;
                                        if (PracticeID > 0) break;
                                    }
                                }
                            }
                            string comments = "";
                            if (PracticeID.IsNull()) comments = "Practice Not found";
                            PaymentCheck paymentCheck = new PaymentCheck()
                            {
                                AddedBy = UD.Email,
                                AddedDate = DateTime.Now,
                                CheckAmount = H.CheckAmount,
                                CheckDate = H.CheckDate,
                                CheckNumber = H.CheckNumber,
                                CreditDebitFlag = H.CreditDebitFlag,
                                PracticeID = PracticeID,
                                PayeeName = H.PayeeName,
                                PayeeTaxID = H.PayeeTaxID,
                                PayeeNPI = H.PayeeNPI,
                                PayeeAddress = H.PayeeAddress,
                                PayeeCity = H.PayeeCity,
                                PayeeState = H.PayeeState,
                                PayeeZipCode = H.PayeeZip,
                                PayerName = H.PayerName,
                                PayerID = H.PayerID,
                                PayerAddress = H.PayerAddress,
                                PayerCity = H.PayerCity,
                                PayerState = H.PayerState,
                                PayerZipCode = H.PayerZip,
                                PayerContactNumber = H.PayerTelephone,
                                PayerContactPerson = H.PayerContactName,
                                REF_2U_ID = H.REF2U,
                                TransactionCode = H.TransactionCode,
                                PaymentMethod = H.PaymentMethod,
                                ReceiverID = id,  //ReceiverID            get as id use in first id as rId,
                                DownloadedFileID = file.ID, //downloadedFile.ID,
                                NumberOfVisits = H.ERAVisitPayments.Count,
                                NumberOfPatients = H.ERAVisitPayments.Select(o => o.SubscriberLastName + o.SubscirberFirstName).Distinct().Count(),
                                Status = PracticeID.IsNull() ? "F" : "NP",
                                Comments = comments
                            };
                            _context.PaymentCheck.Add(paymentCheck);
                            foreach (ERAVisitPayment V in H.ERAVisitPayments)
                            {
                                Visit visit = null;
                                Patient patient = null;
                                string[] temp = null;
                                if (V.PatientControlNumber.Contains(" "))
                                    temp = V.PatientControlNumber.Split(' ');
                                else
                                    temp = V.PatientControlNumber.Split('_');
                                if (temp != null && temp.Length >= 2)
                                {
                                    visit = _context.Visit.Where(v => v.ID == long.Parse(temp[0]) && v.PracticeID == PracticeID).FirstOrDefault();
                                    patient = _context.Patient.Where(v => v.AccountNum == temp[1]).FirstOrDefault();
                                }
                                if (patient == null)
                                {
                                    if (!V.PatientLastName.IsNull2() && !V.PatientFirstName.IsNull2())
                                    {
                                        List<Patient> lst = _context.Patient.Where(p => p.LastName == V.PatientLastName && p.FirstName == V.PatientFirstName).ToList();
                                        if (lst != null && lst.Count == 1)
                                            patient = lst[0];
                                    }
                                    if (patient == null && !V.SubscriberLastName.IsNull2() && !V.SubscirberFirstName.IsNull2())
                                    {
                                        List<Patient> lst = _context.Patient.Where(p => p.LastName == V.SubscriberLastName && p.FirstName == V.SubscirberFirstName).ToList();
                                        if (lst != null && lst.Count == 1)
                                            patient = lst[0];
                                    }
                                    if (patient == null && !V.SubscriberID.IsNull2())
                                    {
                                        List<PatientPlan> lst = _context.PatientPlan.Where(p => p.SubscriberId == V.SubscriberID).ToList();
                                        if (lst != null && lst.Count == 1)
                                            patient = _context.Patient.Find(lst[0].PatientID);
                                    }
                                }
                                long? VisitID = visit?.ID;
                                long? PatientID = patient?.ID;
                                PaymentVisit payVisit = new PaymentVisit()
                                {
                                    AddedBy = UD.Email,
                                    AddedDate = DateTime.Now,
                                    PaymentCheckID = paymentCheck.ID,
                                    BilledAmount = V.SubmittedAmt,
                                    PaidAmount = V.PaidAmt,
                                    PatientAmount = V.PatResponsibilityAmt,
                                    ClaimNumber = V.PatientControlNumber,
                                    ClaimStatementFromDate = V.ClaimStatementFrom,
                                    ClaimStatementToDate = V.ClaimStatementTo,
                                    ForwardedPayerID = V.CrossOverPayerID,
                                    ForwardedPayerName = V.CrossOverPayerName,
                                    InsuredFirstName = V.SubscirberFirstName,
                                    InsuredLastName = V.SubscriberLastName,
                                    InsuredID = V.SubscriberID,
                                    PatientFIrstName = V.PatientFirstName,
                                    PatientLastName = V.PatientLastName,
                                    ProvFirstName = V.RendPrvFirstName,
                                    ProvLastName = V.RendPrvLastName,
                                    ProvNPI = V.RendPrvNPI,
                                    PayerICN = V.PayerControlNumber,
                                    PayerReceivedDate = V.ClaimReceivedDate,
                                    ProcessedAs = V.ClaimProcessedAs,
                                    PatientID = PatientID,
                                    VisitID = VisitID,
                                    Status = "N"
                                };
                                decimal visitAllowedAmt = 0, mVisitAllowedAmt = 0;
                                decimal visitWriteOffAmt = 0;
                                decimal visitPatResp = 0;



                                foreach (ERAChargePayment C in V.ERAChargePayments)
                                {
                                    // Finding ChargeID
                                    Charge charge = null;
                                    if (!C.ChargeControlNumber.IsNull2() && C.ChargeControlNumber.All(char.IsDigit)
                                        && visit != null)
                                    {
                                        charge = _context.Charge.Where(c => c.PatientID == PatientID && c.ID == long.Parse(C.ChargeControlNumber) && c.VisitID == visit.ID).FirstOrDefault();
                                    }

                                    if (charge == null && visit != null)
                                    {
                                        charge = _context.Charge.Where(c => c.PatientID == PatientID && c.VisitID == visit.ID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                    }
                                    else if (charge == null)
                                    {
                                        if (C.ServiceDateFrom.Date() == null)
                                        {
                                            charge = _context.Charge.Where(c => c.PatientID == PatientID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == V.ClaimStatementFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                        }
                                        else
                                        {
                                            charge = _context.Charge.Where(c => c.PatientID == PatientID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                        }
                                    }
                                    long? ChargeID = charge?.ID;

                                    C.ChargeID = ChargeID;

                                    if (visit == null && charge != null)
                                    {
                                        visit = _context.Visit.Find(charge.VisitID);
                                        VisitID = visit?.ID;
                                    }
                                    //

                                    visitAllowedAmt += C.AllowedAmount.Val();
                                    mVisitAllowedAmt += C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val();
                                    visitWriteOffAmt += C.WriteOffAmt.Val();
                                    visitPatResp += C.CopayAmt.Val() + C.CoInsuranceAmt.Val() + C.DeductableAmt.Val();
                                    ERARemitCode E = null;
                                    if (C.RemitCodes != null && C.RemitCodes.Count > 0)
                                    {
                                        if (C.RemitCodes.Count > 0)
                                        {
                                            E = C.RemitCodes[0];
                                            if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                            {
                                            }
                                            else
                                            {
                                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                                if (adjustmentCode.Type == "W")
                                                    visitWriteOffAmt += E.Amount.Val();
                                            }
                                        }
                                        if (C.RemitCodes.Count > 1)
                                        {
                                            E = C.RemitCodes[1];
                                            if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                            {
                                            }
                                            else
                                            {
                                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                                if (adjustmentCode.Type == "W")
                                                    visitWriteOffAmt += E.Amount.Val();
                                            }
                                        }
                                        if (C.RemitCodes.Count > 2)
                                        {
                                            E = C.RemitCodes[2];
                                            if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                            {
                                            }
                                            else
                                            {
                                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                                if (adjustmentCode.Type == "W")
                                                    visitWriteOffAmt += E.Amount.Val();
                                            }
                                        }
                                        if (C.RemitCodes.Count > 3)
                                        {
                                            E = C.RemitCodes[3];
                                            if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                            {
                                            }
                                            else
                                            {
                                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                                if (adjustmentCode.Type == "W")
                                                    visitWriteOffAmt += E.Amount.Val();
                                            }
                                        }
                                        if (C.RemitCodes.Count > 4)
                                        {
                                            E = C.RemitCodes[4];
                                            if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                            {
                                            }
                                            else
                                            {
                                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                                if (adjustmentCode.Type == "W")
                                                    visitWriteOffAmt += E.Amount.Val();
                                            }
                                        }
                                    }
                                }



                                if (visitAllowedAmt != mVisitAllowedAmt)
                                    visitAllowedAmt = mVisitAllowedAmt;
                                payVisit.AllowedAmount = visitAllowedAmt.Val();
                                payVisit.WriteOffAmount = visitWriteOffAmt.Val();
                                payVisit.PatientAmount = visitPatResp.Val();
                                payVisit.VisitID = VisitID;

                                _context.PaymentVisit.Add(payVisit);
                                long? FirstAdjustmentID = null;
                                ICollection<PaymentCharge> paymentCharges = new List<PaymentCharge>();
                                foreach (ERAChargePayment C in V.ERAChargePayments)
                                {
                                    // This code has been moved upwords
                                    //Charge charge = null;
                                    //if (!C.ChargeControlNumber.IsNull2() && C.ChargeControlNumber.All(char.IsDigit)
                                    //    && visit != null)
                                    //{
                                    //    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.ID == long.Parse(C.ChargeControlNumber) && c.VisitID == visit.ID).FirstOrDefault();
                                    //}

                                    //if (charge == null && visit != null)
                                    //{
                                    //    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.VisitID == visit.ID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                    //}
                                    //else if (charge == null)
                                    //{
                                    //    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                    //}
                                    //long? ChargeID = charge?.ID;

                                    long? ChargeID = C.ChargeID;

                                    decimal? mAllowedAmount = C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val();
                                    if (mAllowedAmount != C.AllowedAmount.Val())
                                        C.AllowedAmount = mAllowedAmount;
                                    PaymentCharge paymentCharge = new PaymentCharge()
                                    {
                                        AddedBy = UD.Email,
                                        AddedDate = DateTime.Now,
                                        PaymentVisitID = payVisit.ID,
                                        ChargeID = ChargeID,
                                        AllowedAmount = C.AllowedAmount.IsNull() ? C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val() : C.AllowedAmount,
                                        BilledAmount = C.SubmittedAmt,
                                        ChargeControlNumber = C.ChargeControlNumber,
                                        Copay = C.CopayAmt,
                                        CoinsuranceAmount = C.CoInsuranceAmt,
                                        DeductableAmount = C.DeductableAmt,
                                        CPTCode = C.CPTCode,
                                        DOSFrom = C.ServiceDateFrom,
                                        DOSTo = C.ServiceDateTo,
                                        Modifier1 = C.Modifier1,
                                        Modifier2 = C.Modifier2,
                                        Modifier3 = C.Modifier3,
                                        Modifier4 = C.Modifier4,
                                        PaidAmount = C.PaidAmt,
                                        PatientAmount = C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val(),
                                        RevenueCode = C.RevenueCode,
                                        RemarkCodeID1 = C.RemarkCode1.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode1).FirstOrDefault()?.ID,
                                        RemarkCodeID2 = C.RemarkCode2.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode2).FirstOrDefault()?.ID,
                                        RemarkCodeID3 = C.RemarkCode3.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode3).FirstOrDefault()?.ID,
                                        RemarkCodeID4 = C.RemarkCode4.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode4).FirstOrDefault()?.ID,
                                        RemarkCodeID5 = C.RemarkCode5.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode5).FirstOrDefault()?.ID,
                                        Units = C.UnitsPaid,
                                        WriteoffAmount = C.WriteOffAmt,
                                        Status = "N",
                                        AppliedToSec = true
                                    };
                                    ERARemitCode E = null;
                                    if (C.RemitCodes != null && C.RemitCodes.Count > 0)
                                    {
                                        if (C.RemitCodes.Count > 0)
                                        {
                                            E = C.RemitCodes[0];
                                            AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                            paymentCharge.AdjustmentCodeID1 = adjustmentCode?.ID;
                                            paymentCharge.AdjustmentAmount1 = E.Amount;
                                            paymentCharge.GroupCode1 = E.GroupCode;
                                            paymentCharge.AdjustmentQuantity1 = E.Quantity;
                                            FirstAdjustmentID = paymentCharge.AdjustmentCodeID1;
                                            if (adjustmentCode.Type == "W")
                                            {
                                                if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                                {
                                                }
                                                else
                                                {
                                                    paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                                }
                                            }
                                        }
                                        if (C.RemitCodes.Count > 1)
                                        {
                                            E = C.RemitCodes[1];
                                            AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                            paymentCharge.AdjustmentCodeID2 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                            paymentCharge.AdjustmentAmount2 = E.Amount;
                                            paymentCharge.GroupCode2 = E.GroupCode;
                                            paymentCharge.AdjustmentQuantity2 = E.Quantity;
                                            if (adjustmentCode.Type == "W")
                                            {
                                                if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                                {
                                                }
                                                else
                                                {
                                                    paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                                }
                                            }
                                        }
                                        if (C.RemitCodes.Count > 2)
                                        {
                                            E = C.RemitCodes[2];
                                            AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                            paymentCharge.AdjustmentCodeID3 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                            paymentCharge.AdjustmentAmount3 = E.Amount;
                                            paymentCharge.GroupCode3 = E.GroupCode;
                                            paymentCharge.AdjustmentQuantity3 = E.Quantity;
                                            if (adjustmentCode.Type == "W")
                                            {
                                                if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                                {
                                                }
                                                else
                                                {
                                                    paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                                }
                                            }
                                        }
                                        if (C.RemitCodes.Count > 3)
                                        {
                                            E = C.RemitCodes[3];
                                            AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                            paymentCharge.AdjustmentCodeID4 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                            paymentCharge.AdjustmentAmount4 = E.Amount;
                                            paymentCharge.GroupCode4 = E.GroupCode;
                                            paymentCharge.AdjustmentQuantity4 = E.Quantity;
                                            if (adjustmentCode.Type == "W")
                                            {
                                                if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                                {
                                                }
                                                else
                                                {
                                                    paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                                }
                                            }
                                        }
                                        if (C.RemitCodes.Count > 4)
                                        {
                                            E = C.RemitCodes[5];
                                            AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                            paymentCharge.AdjustmentCodeID5 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                            paymentCharge.AdjustmentAmount5 = E.Amount;
                                            paymentCharge.GroupCode5 = E.GroupCode;
                                            paymentCharge.AdjustmentQuantity5 = E.Quantity;
                                            if (adjustmentCode.Type == "W")
                                            {
                                                if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                                {
                                                }
                                                else
                                                {
                                                    paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                                }
                                            }
                                        }
                                    }
                                    _context.PaymentCharge.Add(paymentCharge);
                                    paymentCharges.Add(paymentCharge);
                                }
                                CreateFollowup(payVisit, defaultActionID, defaultReasonID, defaultGroupID, UD.Email, paymentCharges);
                            }
                            importedChecks.Add(paymentCheck);
                        }
                        await _context.SaveChangesAsync();

                        file.Processed = true;
                        data.Files835Processed += 1;
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.WriteAllText(Path.Combine(AllDownloads, "Exception835.txt"), ex.ToString());
                    }
                    finally
                    {
                        _context.DownloadedFile.Update(file);
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(AllDownloads, "Exception835.txt"), ex.ToString());
            }
            log.Processed = true;

            _context.ReportsLog.Update(log);
            _context.SaveChanges();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(data);
        }
        private void CreateFollowup(PaymentVisit PayVisit, long? DefaultActionID, long? DefaultReasonID,
                                   long? DefaultGroupID, string Email, ICollection<PaymentCharge> PaymentCharges = null)
        {
            PlanFollowup followup = null;
            long? adjustmentCodeID = null;
            if (PayVisit.PaymentCharge != null) PaymentCharges = PayVisit.PaymentCharge;
            foreach (PaymentCharge payCharge in PaymentCharges)
            {
                if (payCharge.PaidAmount.IsNull() && payCharge.PatientAmount.IsNull() && payCharge.WriteoffAmount.IsNull())
                {
                    adjustmentCodeID = payCharge.AdjustmentCodeID1;
                    long? actionID = null, reasonID = null, groupID = null;
                    AdjustmentCode adjCode = _context.AdjustmentCode.Find(adjustmentCodeID);
                    if (adjCode != null)
                    {
                        actionID = adjCode.ActionID.IsNull() ? DefaultActionID : adjCode.ActionID;
                        reasonID = adjCode.ReasonID.IsNull() ? DefaultReasonID : adjCode.ReasonID;
                        groupID = adjCode.GroupID.IsNull() ? DefaultGroupID : adjCode.GroupID;
                    }
                    if (followup == null)
                    {
                        followup = _context.PlanFollowUp.Where(v => v.VisitID == PayVisit.VisitID).FirstOrDefault();
                        if (followup == null)
                        {
                            followup = new PlanFollowup()
                            {
                                ActionID = actionID,
                                AddedBy = Email,
                                AddedDate = System.DateTime.Now,
                                GroupID = groupID,
                                Notes = "",
                                PaymentVisitID = PayVisit.ID,
                                ReasonID = reasonID,
                                AdjustmentCodeID = adjustmentCodeID,
                                TickleDate = null,
                                VisitID = PayVisit.VisitID,
                                VisitStatusID = null
                            };
                            _context.PlanFollowUp.Add(followup);
                        }
                        else
                        {
                            followup.ActionID = actionID;
                            followup.UpdatedBy = Email;
                            followup.UpdatedDate = DateTime.Now;
                            followup.GroupID = groupID;
                            followup.PaymentVisitID = PayVisit.ID;
                            followup.ReasonID = reasonID;
                            followup.AdjustmentCodeID = adjustmentCodeID;
                            _context.PlanFollowUp.Update(followup);
                        }
                    }
                    PlanFollowupCharge followupCharge = _context.PlanFollowupCharge.Where(p => p.PlanFollowupID == followup.ID && p.ChargeID == payCharge.ChargeID).FirstOrDefault();
                    if (followupCharge == null)
                    {
                        followupCharge = new PlanFollowupCharge()
                        {
                            PlanFollowupID = followup.ID,
                            ChargeID = payCharge.ChargeID,
                            GroupID = groupID,
                            ActionID = actionID,
                            AdjustmentCodeID = adjustmentCodeID,
                            PaymentChargeID = payCharge.ID,
                            AddedBy = Email,
                            AddedDate = DateTime.Now,
                            ReasonID = reasonID,
                            RemarkCode1ID = payCharge.RemarkCodeID1,
                            RemarkCode2ID = payCharge.RemarkCodeID2,
                            RemarkCode3ID = payCharge.RemarkCodeID3,
                            RemarkCode4ID = payCharge.RemarkCodeID4
                        };
                        _context.PlanFollowupCharge.Add(followupCharge);
                    }
                    else
                    {
                        followupCharge.GroupID = groupID;
                        followupCharge.ActionID = actionID;
                        followupCharge.ChargeID = payCharge.ChargeID;
                        followupCharge.AdjustmentCodeID = adjustmentCodeID;
                        followupCharge.PaymentChargeID = payCharge.ID;
                        followupCharge.UpdatedBy = Email;
                        followupCharge.UpdatedDate = DateTime.Now;
                        followupCharge.ReasonID = reasonID;
                        followupCharge.RemarkCode1ID = payCharge.RemarkCodeID1;
                        followupCharge.RemarkCode2ID = payCharge.RemarkCodeID2;
                        followupCharge.RemarkCode3ID = payCharge.RemarkCodeID3;
                        followupCharge.RemarkCode4ID = payCharge.RemarkCodeID4;
                        _context.PlanFollowupCharge.Update(followupCharge);
                    }
                }
            }
        }


        [Route("ProcessReport")]
        [HttpPost()]
        public async Task<IActionResult> ProcessReport(FileUploadViewModel File)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

            Settings    settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
            if (settings == null)
            {
                return BadRequest("Document Server Settings Not Found");
            }

            string DownloadDirectory = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory,
                        "ManualImport",
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            string AllDownloads = Path.Combine(DownloadDirectory, "DOWNLOADS");
            Directory.CreateDirectory(AllDownloads);

            string fileType = string.Empty, FilePath = "";
            if (File.Type.Replace(".", "") == "zip" || File.Type.Replace(".", "") == "rar")
            {
                return BadRequest("Zip File is not supported. Please contaxt Bell MedEx.");
            }
            else
            {
                byte[] data = Convert.FromBase64String(File.Content.Substring(File.Content.IndexOf("base64,") + 7));
                string decodedString = Encoding.UTF8.GetString(data);

                fileType = Utilities.GetFileType(decodedString);
                FilePath = Path.Combine(Path.Combine(AllDownloads, fileType), File.Name);
                await System.IO.File.WriteAllTextAsync(FilePath, decodedString);
            }

            string zipFilePath = Path.Combine(AllDownloads, "alldownloads.zip");
            ZipFile.CreateFromDirectory(AllDownloads, zipFilePath);

            ReportsLog log = new ReportsLog()
            {
                AddedBy = UD.Email,
                AddedDate = DateTime.Now,
                ClientID = UD.ClientID,
                ReceiverID = null,
                UserResolved = false,
                Processed = false,
                ManualImport = true,
                SubmitterID = null,
                ZipFilePath = zipFilePath,
                FilesCount = 1
            };
            _context.ReportsLog.Add(log);
            await _context.SaveChangesAsync();

            DownloadedFile downloadedFile = new DownloadedFile()
            {
                AddedBy = UD.Email,
                AddedDate = DateTime.Now,
                FileType = fileType,
                FilePath = FilePath,
                Processed = false,
                ReportsLogID = log.ID
            };
            _context.DownloadedFile.Add(downloadedFile);
            _context.SaveChanges();

            //if (fileType == "999") Process999(UD.Email);
            //if (fileType == "277CA") Process277CA(UD.Email);
            //if (fileType == "835") Process835(UD.Email);

            if (fileType == "999")
            {
                try
                {
                    List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "999" && d.ID == downloadedFile.ID).ToList();
                    //if (files != null || files.Count > 0) 

                    foreach (DownloadedFile file in files)
                    {
                        try
                        {
                            FilePath = file.FilePath;

                            _999Parser parser = new _999Parser();
                            List<_999Header> header = parser.Parse999File(FilePath);

                            foreach (_999Header H in header)
                            {
                                foreach (_999TransactionSet ST in H.ListOfTransactions)
                                {
                                    SubmissionLog submitLog = _context.SubmissionLog.Where(m => m.ISAControlNumber == ST.AK1_02_GroupControlNum).SingleOrDefault();
                                    if (submitLog != null)
                                    {
                                        submitLog.AK9_Status = H.AK9_01_BatchAcknowledgmentCode;
                                        submitLog.NoOfTotalST = H.AK9_02_NoOfTotalST.Val();
                                        submitLog.NoOfAcceptedST = H.AK9_04_NoOfAcceptedST.Val();
                                        submitLog.NoOfReceivedST = H.AK9_03_NoOfReceivedST.Val();
                                        submitLog.IK5_Status = ST.IK5_01_ST_AcknowledgmentCode;
                                        submitLog.AK9_ErrorCode = "";
                                        submitLog.Transaction999Path = FilePath;
                                        submitLog.DownloadedFileID = file.ID;
                                        submitLog.UpdatedBy = UD.Email;
                                        submitLog.UpdatedDate = DateTime.Now;
                                        _context.SubmissionLog.Update(submitLog);

                                        string status = string.Empty;

                                        if (submitLog.IK5_Status == "A" && submitLog.AK9_Status == "A") status = "K";
                                        else if (submitLog.IK5_Status == "R" && submitLog.AK9_Status == "R") status = "D";
                                        else if (submitLog.IK5_Status == "E" && submitLog.AK9_Status == "E") status = "E";

                                        List<Visit> visits = _context.Visit.Where(m => m.SubmissionLogID == submitLog.ID).ToList();
                                        foreach (Visit v in visits)
                                        {
                                            if (v.PrimaryStatus == "S")
                                            {
                                                v.PrimaryStatus = status;
                                                _context.Visit.Update(v);
                                            }
                                        }
                                    }
                                }
                            }

                            file.Processed = true;
                        }
                        catch (Exception ex)
                        {
                            //file.Exception = ex.ToString();
                        }
                        finally
                        {
                            _context.DownloadedFile.Update(file);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(Path.Combine(AllDownloads, "Exception999.txt"), ex.ToString());
                }
            }

            if (fileType == "277CA")
            {
                try
                {
                    List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "277CA" && d.ID == downloadedFile.ID).ToList();
                    //if (files == null || files.Count == 0) return;

                    //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                    //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                    //{
                    foreach (DownloadedFile file in files)
                    {
                        try
                        {
                            FilePath = file.FilePath;
                            if (FilePath.Contains("Exception.txt")) continue;
                            ClaimAckParser parser = new ClaimAckParser();
                            List<ClaimAckHeader> header = parser.Parse277CAFile(FilePath);

                            _context.VisitStatusLog.Add(new VisitStatusLog()
                            {
                                AddedBy = UD.Email,
                                AddedDate = DateTime.Now,
                                DownloadedFileID = file.ID,
                                Transaction276Path = null,
                                Transaction277CAPath = FilePath,
                                Transaction277Path = null,
                                Transaction999Path = null
                            });

                            foreach (ClaimAckHeader H in header)
                            {
                                foreach (ClaimAckVisit V in H.ClaimAckVisits)
                                {
                                    VisitStatus visitStatus = new VisitStatus()
                                    {
                                        AddedBy = UD.Email,
                                        AddedDate = DateTime.Now,
                                        SubmitterTRN = H.SubmitterTRN,
                                        ResponseEntity = H.PayerEntity,
                                        BilledAmount = V.VisitClaimAmt1,
                                        CategoryCode1 = V.VisitCategoryCode1,
                                        CategoryCodeDesc1 = V.VisitCategoryCodeDesc1,
                                        CategoryCode2 = V.VisitCategoryCode2,
                                        CategoryCodeDesc2 = V.VisitCategoryCodeDesc2,
                                        CategoryCode3 = V.VisitCategoryCode3,
                                        CategoryCodeDesc3 = V.VisitCategoryCodeDesc3,
                                        CheckDate = null,
                                        CheckNumber = null,
                                        DOS = null,
                                        StatusCode1 = V.VisitStatusCode1,
                                        StatusCodeDesc1 = V.VisitStatusCodeDesc1,
                                        StatusCode2 = V.VisitStatusCode2,
                                        StatusCodeDesc2 = V.VisitStatusCodeDesc2,
                                        StatusCode3 = V.VisitStatusCode3,
                                        StatusCodeDesc3 = V.VisitStatusCodeDesc3,
                                        EntityCode1 = V.VisitEntityCode1,
                                        EntityCodeDesc1 = V.VisitEntityCodeDesc1,
                                        EntityCode2 = V.VisitEntityCode2,
                                        EntityCodeDesc2 = V.VisitEntityCodeDesc2,
                                        EntityCode3 = V.VisitEntityCode3,
                                        EntityCodeDesc3 = V.VisitEntityCodeDesc3,
                                        ErrorMessage = "",
                                        PatientLN = V.PatientLastName,
                                        PatientFN = V.PatientFirstName,
                                        SubscriberID = V.PatientSubscriberID,
                                        SubscriberFN = V.PatientFirstName,
                                        SubscriberLN = V.PatientLastName,
                                        TRNNumber = V.VisitTRN,
                                        ActionCode = V.VisitActionCode1,
                                        Status = V.Status,
                                        StatusDate = V.VisitStatusDate1,
                                        PayerID = H.PayerID,
                                        PayerName = H.PayerOrgName,
                                        PayerControlNumber = V.PayerClaimControlNum,
                                        FreeText1 = V.VisitRejection1,
                                        FreeText2 = V.VisitRejection2,
                                        FreeText3 = V.VisitRejection3,
                                        RejectionReason1 = V.VisitActionCode1 == "U" ?
                                                        (V.VisitStatusCodeDesc1.Contains("Entity") && !V.VisitEntityCodeDesc1.IsNull2() ?
                                                        V.VisitStatusCodeDesc1 + " (" + V.VisitEntityCodeDesc1 + ")" : V.VisitStatusCodeDesc1) : V.VisitStatusCodeDesc1,
                                        RejectionReason2 = V.VisitActionCode2 == "U" ?
                                                        (V.VisitStatusCodeDesc2.Contains("Entity") && !V.VisitEntityCodeDesc2.IsNull2() ?
                                                        V.VisitStatusCodeDesc2 + " (" + V.VisitEntityCodeDesc2 + ")" : V.VisitStatusCodeDesc2) : V.VisitStatusCodeDesc2,
                                        RejectionReason3 = V.VisitActionCode3 == "U" ?
                                                        (V.VisitStatusCodeDesc3.Contains("Entity") && !V.VisitEntityCodeDesc3.IsNull2() ?
                                                        V.VisitStatusCodeDesc3 + " (" + V.VisitEntityCodeDesc3 + ")" : V.VisitStatusCodeDesc3) : V.VisitStatusCodeDesc3,
                                        VisitAmount = V.VisitClaimAmt1,
                                        ProviderFN = V.BillProvFirstName,
                                        ProviderLN = V.BillProvOrgName,
                                        ProviderNPI = V.BillProvNPI
                                    };

                                    //if (visitStatus.ActionCode == "U" && !visitStatus.RejectionReason1.IsNull2())
                                    //{
                                    //    visitStatus.RejectionReason1 = V.VisitStatusCodeDesc1;
                                    //}

                                    Visit visit = null;
                                    string[] temp = null;
                                    if (V.VisitTRN.Contains(" "))
                                        temp = V.VisitTRN.Split(' ');
                                    else
                                        temp = V.VisitTRN.Split('_');
                                    if (temp != null && temp.Length >= 2)
                                    {
                                        visit = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).SingleOrDefault();
                                        if (visit != null)
                                        {
                                            visitStatus.VisitID = visit.ID;
                                            visitStatus.PracticeID = visit.PracticeID;
                                            visitStatus.ProviderID = visit.ProviderID;
                                            visitStatus.LocationID = visit.LocationID;
                                            visitStatus.PatientPlanID = visit.PrimaryPatientPlanID;

                                            string Status = string.Empty, rejectionReason1 = string.Empty, freeText = string.Empty ;
                                            if (V.VisitActionCode1 == "U")
                                            {
                                                //if (!visitStatus.FreeText1.IsNull2())
                                                //{
                                                //    rejectionReason1 = visitStatus.FreeText1;
                                                //    if (!visitStatus.FreeText2.IsNull2())
                                                //    {
                                                //        rejectionReason1 = Environment.NewLine + visitStatus.FreeText2;
                                                //        if (!visitStatus.FreeText3.IsNull2())
                                                //        {
                                                //            rejectionReason1 = Environment.NewLine + visitStatus.FreeText3;
                                                //        }
                                                //    }
                                                //}

                                                Status = "R";

                                                if (!visitStatus.FreeText1.IsNull2()) freeText = visitStatus.FreeText1;
                                                if (!visitStatus.FreeText2.IsNull2()) freeText += Environment.NewLine + visitStatus.FreeText2;
                                                if (!visitStatus.FreeText3.IsNull2()) freeText += Environment.NewLine + visitStatus.FreeText3;


                                                if (!visitStatus.RejectionReason1.IsNull2()) rejectionReason1 = visitStatus.RejectionReason1;
                                                if (!visitStatus.RejectionReason2.IsNull2()) rejectionReason1 += Environment.NewLine + visitStatus.RejectionReason2;
                                                if (!visitStatus.RejectionReason3.IsNull2()) rejectionReason1 += Environment.NewLine + visitStatus.RejectionReason3;

                                            }
                                            else if (V.VisitActionCode1 == "WQ")
                                            {
                                                Status = V.VisitCategoryCode1 + H.PayerEntity;
                                            }

                                            //if (!Status.IsNull2() && (visit.PrimaryStatus == "S" || visit.PrimaryStatus == "K" || visit.PrimaryStatus == "E" ||
                                            //    visit.PrimaryStatus == "D"))
                                            //{
                                                visit.PrimaryStatus = Status;
                                            //}

                                            //if (!rejectionReason1.IsNull2())
                                            //{
                                            //    visit.RejectionReason = rejectionReason1;
                                            //}

                                            if (!freeText.IsNull2())
                                            {
                                                visit.RejectionReason = freeText;
                                            }


                                            if (!V.PayerClaimControlNum.IsNull2())
                                                visit.PayerClaimControlNum = V.PayerClaimControlNum;

                                            _context.Visit.Update(visit);
                                        }
                                    }

                                    if (!H.SubmitterTRN.IsNull2())
                                    {
                                        SubmissionLog submitLog = _context.SubmissionLog.Where(m => m.ISAControlNumber == H.SubmitterTRN).SingleOrDefault();
                                        if (submitLog != null)
                                        {
                                            submitLog.Trasaction277CAPath = FilePath;
                                            submitLog.UpdatedBy = UD.Email;
                                            submitLog.UpdatedDate = DateTime.Now;
                                            _context.SubmissionLog.Update(submitLog);
                                        }
                                    }

                                    _context.VisitStatus.Add(visitStatus);

                                    string chargeRej = string.Empty;

                                    foreach (ClaimAckCharge C in V.ClaimAckCharges)
                                    {
                                        ChargeStatus chargeStatus = new ChargeStatus()
                                        {
                                            ID = visitStatus.ID,
                                            AddedBy = UD.Email,
                                            AddedDate = DateTime.Now,
                                            BilledAmount = C.ChargeAmt,
                                            CategoryCode1 = C.ChargeCategoryCode1,
                                            CategoryCode2 = C.ChargeCategoryCode2,
                                            CategoryCode3 = C.ChargeCategoryCode3,
                                            EntityCode1 = C.ChargeEntityCode1,
                                            EntityCode2 = C.ChargeEntityCode2,
                                            EntityCode3 = C.ChargeEntityCode3,
                                            StatusCode1 = C.ChargeStatusCode1,
                                            StatusCode2 = C.ChargeStatusCode2,
                                            StatusCode3 = C.ChargeStatusCode3,
                                            DOS = C.ServiceDateFrom,
                                            CPT = C.CPT,
                                            Modifier1 = C.Modifier1,
                                            Modifier2 = C.Modifier2,
                                            Modifier3 = C.Modifier3,
                                            Modifier4 = C.Modifier4,
                                            RejectionReason1 = C.ChargeRejection1,
                                            RejectionReason2 = C.ChargeRejection2,
                                            RejectionReason3 = C.ChargeRejection3
                                        };
                                        _context.ChargeStatus.Add(chargeStatus);


                                        if (visit != null)
                                        {
                                            Charge charge = _context.Charge.Where(c => c.VisitID == visit.ID && c.Cpt.CPTCode == chargeStatus.CPT).SingleOrDefault();
                                            if (charge != null)
                                            {
                                                if(visitStatus.ActionCode == "U" || C.ChargeActionCode1 == "U")
                                                {
                                                    string rejReason = "";
                                                    if (!C.ChargeRejection1.IsNull2()) rejReason = C.ChargeRejection1;
                                                    if (!C.ChargeRejection2.IsNull2()) rejReason += Environment.NewLine + C.ChargeRejection2;
                                                    if (!C.ChargeRejection3.IsNull2()) rejReason += Environment.NewLine + C.ChargeRejection3;

                                                    charge.RejectionReason = rejReason;
                                                    charge.PrimaryStatus = "R";

                                                    chargeRej += chargeStatus.CPT + ": " + rejReason + Environment.NewLine;

                                                    _context.Charge.Update(charge);
                                                }
                                            }
                                        }
                                    }

                                    if (!chargeRej.IsNull2())
                                    {
                                        visit.RejectionReason = chargeRej;
                                        visit.PrimaryStatus = "R";
                                        _context.Visit.Update(visit);
                                    }
                                }
                            }
                            file.Processed = true;
                        }
                        catch (Exception ex)
                        {
                            //file.Exception = ex.ToString();
                        }
                        finally
                        {
                            _context.DownloadedFile.Update(file);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(Path.Combine(AllDownloads, "Exception277CA.txt"), ex.ToString());
                }
            }

            else
            {
                return BadRequest("File is not supported");
            }

            log.Processed = true;
            _context.ReportsLog.Update(log);
            _context.SaveChanges();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok();
        }

        //private async void Process999(string Email)
        //{
        //    string FilePath = string.Empty;
        //    try
        //    {
        //        List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "999" && d.Processed == false).ToList();
        //        if (files == null || files.Count == 0) return;

        //        //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        //        //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
        //        //{
        //        foreach (DownloadedFile file in files)
        //        {
        //            try
        //            {
        //                FilePath = file.FilePath;

        //                _999Parser parser = new _999Parser();
        //                List<_999Header> header = parser.Parse999File(FilePath);

        //                foreach (_999Header H in header)
        //                {
        //                    foreach (_999TransactionSet ST in H.ListOfTransactions)
        //                    {
        //                        SubmissionLog log = _context.SubmissionLog.Where(m => m.ISAControlNumber == ST.AK1_02_GroupControlNum).SingleOrDefault();
        //                        if (log != null)
        //                        {
        //                            log.AK9_Status = H.AK9_01_BatchAcknowledgmentCode;
        //                            log.NoOfTotalST = H.AK9_02_NoOfTotalST.Val();
        //                            log.NoOfAcceptedST = H.AK9_04_NoOfAcceptedST.Val();
        //                            log.NoOfReceivedST = H.AK9_03_NoOfReceivedST.Val();
        //                            log.IK5_Status = ST.IK5_01_ST_AcknowledgmentCode;
        //                            log.AK9_ErrorCode = "";
        //                            log.Transaction999Path = FilePath;
        //                            log.DownloadedFileID = file.ID;
        //                            log.UpdatedBy = Email;
        //                            log.UpdatedDate = DateTime.Now;
        //                            _context.SubmissionLog.Update(log);
        //                        }
        //                    }
        //                }

        //                file.Processed = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                //file.Exception = ex.ToString();
        //            }
        //            finally
        //            {
        //                _context.DownloadedFile.Update(file);
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //        //objTrnScope.Complete();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(FilePath), "Exception.txt"), ex.ToString());
        //    }
        //}

        //private async void Process277CA(string Email)
        //{
        //    string FilePath = string.Empty;
        //    try
        //    {
        //        List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "277CA" && d.Processed == false).ToList();
        //        if (files == null || files.Count == 0) return;

        //        //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        //        //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
        //        //{
        //        foreach (DownloadedFile file in files)
        //        {
        //            try
        //            {
        //                FilePath = file.FilePath;
        //                if (FilePath.Contains("Exception.txt")) continue;
        //                ClaimAckParser parser = new ClaimAckParser();
        //                List<ClaimAckHeader> header = parser.Parse277CAFile(FilePath);

        //                _context.VisitStatusLog.Add(new VisitStatusLog()
        //                {
        //                    AddedBy = Email,
        //                    AddedDate = DateTime.Now,
        //                    DownloadedFileID = file.ID,
        //                    Transaction276Path = null,
        //                    Transaction277CAPath = FilePath,
        //                    Transaction277Path = null,
        //                    Transaction999Path = null
        //                });

        //                foreach (ClaimAckHeader H in header)
        //                {
        //                    foreach (ClaimAckVisit V in H.ClaimAckVisits)
        //                    {
        //                        VisitStatus visitStatus = new VisitStatus()
        //                        {
        //                            AddedBy = Email,
        //                            AddedDate = DateTime.Now,
        //                            SubmitterTRN = H.SubmitterTRN,
        //                            ResponseEntity = H.PayerEntity,
        //                            BilledAmount = V.VisitClaimAmt1,
        //                            CategoryCode1 = V.VisitCategoryCode1,
        //                            CategoryCodeDesc1 = V.VisitCategoryCodeDesc1,
        //                            CategoryCode2 = V.VisitCategoryCode2,
        //                            CategoryCodeDesc2 = V.VisitCategoryCodeDesc2,
        //                            CategoryCode3 = V.VisitCategoryCode3,
        //                            CategoryCodeDesc3 = V.VisitCategoryCodeDesc3,
        //                            CheckDate = null,
        //                            CheckNumber = null,
        //                            DOS = null,
        //                            StatusCode1 = V.VisitStatusCode1,
        //                            StatusCodeDesc1 = V.VisitStatusCodeDesc1,
        //                            StatusCode2 = V.VisitStatusCode2,
        //                            StatusCodeDesc2 = V.VisitStatusCodeDesc2,
        //                            StatusCode3 = V.VisitStatusCode3,
        //                            StatusCodeDesc3 = V.VisitStatusCodeDesc3,
        //                            EntityCode1 = V.VisitEntityCode1,
        //                            EntityCodeDesc1 = V.VisitEntityCodeDesc1,
        //                            EntityCode2 = V.VisitEntityCode2,
        //                            EntityCodeDesc2 = V.VisitEntityCodeDesc2,
        //                            EntityCode3 = V.VisitEntityCode3,
        //                            EntityCodeDesc3 = V.VisitEntityCodeDesc3,
        //                            ErrorMessage = "",
        //                            PatientLN = V.PatientLastName,
        //                            PatientFN = V.PatientFirstName,
        //                            SubscriberID = V.PatientSubscriberID,
        //                            SubscriberFN = V.PatientFirstName,
        //                            SubscriberLN = V.PatientLastName,
        //                            TRNNumber = V.VisitTRN,
        //                            ActionCode = V.VisitActionCode1,
        //                            Status = V.Status,
        //                            StatusDate = V.VisitStatusDate1,
        //                            PayerID = H.PayerID,
        //                            PayerName = H.PayerOrgName,
        //                            PayerControlNumber = V.VisitTRN,
        //                            FreeText1 = V.VisitRejection1,
        //                            FreeText2 = V.VisitRejection2,
        //                            FreeText3 = V.VisitRejection3,
        //                            RejectionReason1 = V.VisitActionCode1 == "U" ? 
        //                                            (V.VisitStatusCodeDesc1.Contains("Entity") && !V.VisitEntityCodeDesc1.IsNull2() ?
        //                                            V.VisitStatusCodeDesc1 + " (" + V.VisitEntityCodeDesc1 + ")" : "") : string.Empty,
        //                            RejectionReason2 = V.VisitActionCode2 == "U" ?
        //                                            (V.VisitStatusCodeDesc2.Contains("Entity") && !V.VisitEntityCodeDesc2.IsNull2() ?
        //                                            V.VisitStatusCodeDesc2 + " (" + V.VisitEntityCodeDesc2 + ")" : "") : string.Empty,
        //                            RejectionReason3 = V.VisitActionCode3 == "U" ?
        //                                            (V.VisitStatusCodeDesc3.Contains("Entity") && !V.VisitEntityCodeDesc3.IsNull2() ?
        //                                            V.VisitStatusCodeDesc3 + " (" + V.VisitEntityCodeDesc3 + ")" : "") : string.Empty,
        //                            VisitAmount = V.VisitClaimAmt1,
        //                            ProviderFN = V.BillProvFirstName,
        //                            ProviderLN = V.BillProvOrgName,
        //                            ProviderNPI = V.BillProvNPI
        //                        };

        //                        string[] temp = V.VisitTRN.Split('_');
        //                        if (temp != null && temp.Length >= 2)
        //                        {
        //                            Visit visit = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).SingleOrDefault();
        //                            if (visit != null)
        //                            {
        //                                visitStatus.VisitID = visit.ID;
        //                                visitStatus.PracticeID = visit.PracticeID;
        //                                visitStatus.ProviderID = visit.ProviderID;
        //                                visitStatus.LocationID = visit.LocationID;
        //                                visitStatus.PatientPlanID = visit.PrimaryPatientPlanID;
        //                            }
        //                        }

        //                        if (!H.SubmitterTRN.IsNull2())
        //                        {
        //                            SubmissionLog log = _context.SubmissionLog.Where(m => m.ISAControlNumber == H.SubmitterTRN).SingleOrDefault();
        //                            if (log != null)
        //                            {
        //                                log.Trasaction277CAPath = FilePath;
        //                                log.UpdatedBy = Email;
        //                                log.UpdatedDate = DateTime.Now;
        //                                _context.SubmissionLog.Update(log);
        //                            }
        //                        }

        //                        _context.VisitStatus.Add(visitStatus);

        //                        foreach (ClaimAckCharge C in V.ClaimAckCharges)
        //                        {
        //                            ChargeStatus chargeStatus = new ChargeStatus()
        //                            {
        //                                AddedBy = Email,
        //                                AddedDate = DateTime.Now,
        //                                BilledAmount = C.ChargeAmt,
        //                                CategoryCode1 = C.ChargeCategoryCode1,
        //                                CategoryCode2 = C.ChargeCategoryCode2,
        //                                CategoryCode3 = C.ChargeCategoryCode3,
        //                                EntityCode1 = C.ChargeEntityCode1,
        //                                EntityCode2 = C.ChargeEntityCode2,
        //                                EntityCode3 = C.ChargeEntityCode3,
        //                                StatusCode1 = C.ChargeStatusCode1,
        //                                StatusCode2 = C.ChargeStatusCode2,
        //                                StatusCode3 = C.ChargeStatusCode3,
        //                                DOS = C.ServiceDateFrom,
        //                                CPT = C.CPT,
        //                                Modifier1 = C.Modifier1,
        //                                Modifier2 = C.Modifier2,
        //                                Modifier3 = C.Modifier3,
        //                                Modifier4 = C.Modifier4,
        //                                RejectionReason1 = C.ChargeRejection1,
        //                                RejectionReason2 = C.ChargeRejection2,
        //                                RejectionReason3 = C.ChargeRejection3
        //                            };
        //                            _context.ChargeStatus.Add(chargeStatus);
        //                        }
        //                    }
        //                }

        //                file.Processed = true;


        //            }
        //            catch (Exception ex)
        //            {
        //                //file.Exception = ex.ToString();
        //            }
        //            finally
        //            {
        //                _context.DownloadedFile.Update(file);
        //            }

        //            await _context.SaveChangesAsync();
        //            //objTrnScope.Complete();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(FilePath), "Exception.txt"), ex.ToString());
        //    }
        //}

        //private async void Process835(string Email)
        //{
        //    string FilePath = string.Empty;
        //    try
        //    {
        //        List<DownloadedFile> files = _context.DownloadedFile.Where(d => d.FileType == "835" && d.Processed == false).ToList();
        //        if (files == null || files.Count == 0) return;

        //        //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        //        //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
        //        //{
        //        foreach (DownloadedFile file in files)
        //        {
        //            try
        //            {
        //                PaymentCheckController controller = new PaymentCheckController(_context);
        //                //controller.ImportERA(FilePath, Email, file.ID);

        //                file.Processed = true;

        //            }
        //            catch (Exception ex)
        //            {
        //                //file.Exception = ex.ToString();
        //            }
        //            finally
        //            {
        //                _context.DownloadedFile.Update(file);
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //        //objTrnScope.Complete();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(FilePath), "Exception.txt"), ex.ToString());
        //    }
        //}

        


    }
}