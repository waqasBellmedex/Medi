using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMBatchDocument;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class BatchDocumentController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public BatchDocumentController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetBatchDocument")]
        public async Task<ActionResult<IEnumerable<BatchDocument>>> GetBatchDocument()
        {
                return await _context.BatchDocument.ToListAsync();
         
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMBatchDocument>> GetProfiles(long id)
        {
        
            ViewModels.VMBatchDocument obj = new ViewModels.VMBatchDocument();
            obj.GetProfiles(_context);
            return obj;
        }
        [Route("FindBatchDocumentPath/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BatchDocument>> FindBatchDocumentPath(long id)
        {
            try
            {
                var Batch = await _context.BatchDocument.FindAsync(id);
                string TimeAtFileCreation = DateTime.Now.ToString("MMddyyyyhhmmss").Replace("/", "");
                


                if (Batch == null)
                {
                    return BadRequest("No Record Found.");
                }
                else
                {
                    string inputFilePath = Batch.DocumentFilePath;
                    string staticFilePath = Path.Combine(_context.env.ContentRootPath, "wwwroot", "accessible-files", TimeAtFileCreation + " " + inputFilePath.Split("\\")[inputFilePath.Split("\\").Length - 1]);

                    System.IO.File.Copy(inputFilePath, staticFilePath, true);
                    string NetworkFilePath = "http:\\\\96.69.218.154:8020\\accessible-files\\" + TimeAtFileCreation + " " + inputFilePath.Split("\\")[inputFilePath.Split("\\").Length - 1];
                    Batch.DocumentFilePath = NetworkFilePath;
                    return Batch;
                }
            } 
            catch(FileNotFoundException fnfEx)
            {
                return BadRequest("File not found");
            }
            catch(NotSupportedException)
            {
                return BadRequest("File not supported");
            } 
            catch(DirectoryNotFoundException)
            {
                return BadRequest("Directory not found");
            } 
            catch(IOException ex)
            {
                return BadRequest("Something went wrong. Please contact BellMedEx\n\n"+ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest("Something went wrong. Please contact BellMedEx\n\n" + ex.Message);
            }

        }

        [Route("FindBatchDocument/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BatchDocument>> FindBatchDocument(long id)
        {
            var Batch = await _context.BatchDocument.FindAsync(id);

            if (Batch == null)
            {
                return NotFound();
            }
            else
            {
                Batch.BatchDocumentCharges = _context.BatchDocumentCharges.Where(p => p.BatchDocumentNoID == id).ToList();
                Batch.BatchDocumentPayment = _context.BatchDocumentPayment.Where(p => p.BatchDocumentNoID == id).ToList();
                Batch.Note = _context.Notes.Where(p => p.BatchDocumentNoID == id).ToList();
            }
            return Batch;
        }


        [HttpPost]
        [Route("FindBatchDocument")]
        public async Task<ActionResult<IEnumerable<GBatchDocument>>> FindBatchDocument(CBatchDocument CBatchDocument)
            {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindBatchDocument(CBatchDocument, PracticeId);
        }
        //private List<GBatchDocument> FindBatchDocument(CBatchDocument CBatchDocument, long PracticeId)
        //{
        //    List<GBatchDocument> data = (from bd in _context.BatchDocument
        //                                 join f in _context.Practice on bd.PracticeID equals f.ID
        //                                 join bdc in _context.BatchDocumentCharges on bd.ID equals bdc.BatchDocumentNoID into bcCharge
        //                                 join bdp in _context.BatchDocumentPayment on bd.ID equals bdp.BatchDocumentNoID
        //                                 from bdc in bcCharge.DefaultIfEmpty()
        //                                 join dc in _context.DocumentType on bd.DocumentTypeID equals dc.ID into dType
        //                                 from dc in dType.DefaultIfEmpty()
        //                                 where f.ID == PracticeId &&
        //                                 (CBatchDocument.BatchNumber.IsNull() ? true : bd.ID.IsNull() ? true : bd.ID.Equals(CBatchDocument.BatchNumber)) &&
        //                                 (CBatchDocument.ResponsibleParty.IsNull() ? true : bd.ResponsibleParty.IsNull() ? true : bd.ResponsibleParty.Equals(CBatchDocument.ResponsibleParty)) &&
        //                                 (CBatchDocument.DocumentType.IsNull() ? true : bd.DocumentTypeID.IsNull() ? true : bd.DocumentTypeID.Equals(CBatchDocument.DocumentType)) &&
        //                                 (CheckNullLong(CBatchDocument.LocationID) ? true : bd.LocationID.IsNull() ? true : bd.LocationID.Equals(CBatchDocument.LocationID)) &&
        //                                 (CheckNullLong(CBatchDocument.ProviderID) ? true : bd.ProviderID.IsNull() ? true : bd.ProviderID.Equals(CBatchDocument.ProviderID)) &&
        //                                 (CBatchDocument.Status.IsNull() ? true : bd.Status.IsNull() ? true : bd.Status.Equals(CBatchDocument.Status))
        //                                 group new
        //                                 {
        //                                     BatchNumber = bd.ID,
        //                                     EntryDate = bd.AddedDate.Format("MM/dd/yyyy"),
        //                                     ResponsibleParty = ReponsiblePartyTranslate(bd.ResponsibleParty),
        //                                     Status = StatusTranslate(bd.Status),
        //                                     NumOfPages = bd.NumberOfPages,
        //                                     NumOfDemographics = bd.NoOfDemographics,
        //                                     //ChargesEntry = bdc.NoOfVisits,
        //                                     // Payment = bdc.OtherPatientAmount,
        //                                     StartDate = bd.StartDate.Format("MM/dd/yyyy"),
        //                                     EndDate = bd.EndDate.Format("MM/dd/yyyy"),
                                             
        //                                 } by new { ID = bd.ID } into gp
        //                                 orderby gp.Key.ID descending
        //                                 select new GBatchDocument()
        //                                 {
        //                                     BatchNumber = gp.Key.ID,
        //                                     EndDate = gp.Select(a => a.EndDate).FirstOrDefault(),
        //                                     ResponsibleParty = gp.Select(a => a.ResponsibleParty).FirstOrDefault(),
        //                                     Status = gp.Select(a => a.Status).FirstOrDefault(),
        //                                     NumOfPages = gp.Select(a => a.NumOfPages).FirstOrDefault(),
        //                                     NumOfDemographics = gp.Select(a => a.NumOfDemographics).FirstOrDefault(),
        //                                     NumOfVisits = (
        //                                     (from bdc in _context.BatchDocumentCharges where bdc.BatchDocumentNoID == gp.Key.ID select bdc.NoOfVisits).Count()),
        //                                     NumOfCheck = (
        //                                     (from bdp in _context.BatchDocumentPayment where bdp.BatchDocumentNoID == gp.Key.ID select bdp.CheckAmount).Count()),
        //                                     StartDate = gp.Select(a => a.StartDate).FirstOrDefault(),
        //                                     EntryDate = gp.Select(a => a.EntryDate).FirstOrDefault(),
        //                                 }).ToList();
        //    return data;
        //}



        private List<GBatchDocument> FindBatchDocument(CBatchDocument CBatchDocument, long PracticeId)
        {
            List<GBatchDocument> data = (from bd in _context.BatchDocument
                                         join f in _context.Practice on bd.PracticeID equals f.ID
                                         join bdc in _context.BatchDocumentCharges on bd.ID equals bdc.BatchDocumentNoID into bcCharge
                                         from bdc in bcCharge.DefaultIfEmpty()
                                         join dc in _context.DocumentType on bd.DocumentTypeID equals dc.ID into dType
                                         from dc in dType.DefaultIfEmpty()
                                         where f.ID == PracticeId &&
                                         (CBatchDocument.BatchNumber.IsNull() ? true : bd.ID.IsNull() ? true : bd.ID.Equals(CBatchDocument.BatchNumber)) &&
                                         (CBatchDocument.ResponsibleParty.IsNull() ? true : bd.ResponsibleParty.IsNull() ? true : bd.ResponsibleParty.Equals(CBatchDocument.ResponsibleParty)) &&
                                         (CBatchDocument.DocumentType.IsNull() ? true : bd.DocumentTypeID.IsNull() ? true : bd.DocumentTypeID.Equals(CBatchDocument.DocumentType)) &&
                                         (CheckNullLong(CBatchDocument.LocationID) ? true : bd.LocationID.IsNull() ? true : bd.LocationID.Equals(CBatchDocument.LocationID)) &&
                                         (CheckNullLong(CBatchDocument.ProviderID) ? true : bd.ProviderID.IsNull() ? true : bd.ProviderID.Equals(CBatchDocument.ProviderID)) &&
                                         (CBatchDocument.Status.IsNull() ? true : bd.Status.IsNull() ? true : bd.Status.Equals(CBatchDocument.Status))
                                         group new
                                         {
                                             BatchNumber = bd.ID,
                                             EntryDate = bd.AddedDate.Format("MM/dd/yyyy"),
                                             ResponsibleParty = ReponsiblePartyTranslate(bd.ResponsibleParty),
                                             Status = StatusTranslate(bd.Status),
                                             NumOfPages = bd.NumberOfPages,
                                             NumOfDemographics = bd.NoOfDemographics,
                                             //ChargesEntry = bdc.NoOfVisits,
                                             // Payment = bdc.OtherPatientAmount,
                                             StartDate = bd.StartDate.Format("MM/dd/yyyy"),
                                             EndDate = bd.EndDate.Format("MM/dd/yyyy"),

                                         } by new { ID = bd.ID } into gp
                                         orderby gp.Key.ID descending
                                         select new GBatchDocument()
                                         {
                                             BatchNumber = gp.Key.ID,
                                             EndDate = gp.Select(a => a.EndDate).FirstOrDefault(),
                                             ResponsibleParty = gp.Select(a => a.ResponsibleParty).FirstOrDefault(),
                                             Status = gp.Select(a => a.Status).FirstOrDefault(),
                                             NumOfPages = gp.Select(a => a.NumOfPages).FirstOrDefault(),
                                             NumOfDemographics = gp.Select(a => a.NumOfDemographics).FirstOrDefault(),
                                             NumOfVisits = (
                                             (from bdc in _context.BatchDocumentCharges where bdc.BatchDocumentNoID == gp.Key.ID select bdc.NoOfVisits).Count()),
                                             NumOfCheck = (
                                             (from bdc in _context.BatchDocumentCharges where bdc.BatchDocumentNoID == gp.Key.ID select bdc.OtherPatientAmount).Count()),
                                             StartDate = gp.Select(a => a.StartDate).FirstOrDefault(),
                                             EntryDate = gp.Select(a => a.EntryDate).FirstOrDefault(),
                                         }).ToList();
            return data;
        }

        public string ReponsiblePartyTranslate(String Status)
        {
           
            string desc = string.Empty;
            if (Status == "N")
            {
                desc = "NOT STARTED";
            }
            if (Status == "C")

            {
                desc = "CLIENT";
            }
            if (Status == "I")

            {
                desc = "IN PROCESS";
            }
            if (Status == "B")

            {
                desc = "BELLMEDEX";
            }
            return desc;
        }

        public string StatusTranslate(String Status)
        {
            string desc = string.Empty;
            if (Status == "N")

            {
                desc = "NOT STARTED";
            }
            if (Status == "C")
            //if (_context.BatchDocument.FirstOrDefault().Status == "C")
            {
                desc = "CLOSED";
            }
            if (Status == "I")
            //if (_context.BatchDocument.FirstOrDefault().Status == "I")
            {
                desc = "IN PROCESS";
            }
            return desc;
        }
        //private List<GBatchDocument> FindBatchDocument(CBatchDocument CBatchDocument, UserInfoData UD)
        //{
        //    List<GBatchDocument> data = (from bd in _context.BatchDocument
        //                                     // join b in _context.Biller on bd.BillerID equals b.ID
        //                                 join f in _context.Practice on bd.PracticeID equals f.ID
        //                                 //join up in _context.UserPractices
        //                                 //on f.ID equals up.PracticeID
        //                                 //join u in _context.Users
        //                                 //on up.UserID equals u.Id
        //                                 join bdc in _context.BatchDocumentCharges on bd.ID equals bdc.BatchDocumentNoID into bcCharge
        //                                 from bdc in bcCharge.DefaultIfEmpty()
        //                                 join dc in _context.DocumentType on bd.DocumentTypeID equals dc.ID
        //                                 where f.ID == UD.PracticeID &&
        //                                 //u.Id.ToString() == UD.UserID
        //                                 //&& bd.AddedBy == Email
        //                                 //&& u.IsUserBlock == false &&

        //                                 (CBatchDocument.BatchNumber.IsNull() ? true : bd.ID.Equals(CBatchDocument.BatchNumber)) &&
        //                                 // (CBatchDocument.BillerId.IsNull() ? true : b.ID.Equals(CBatchDocument.BillerId)) && // Check Disable Because Biller Is Removed From Model
        //                                 (CBatchDocument.ResponsibleParty.IsNull() ? true : bd.ResponsibleParty.Equals(CBatchDocument.ResponsibleParty)) &&
        //                                 (CBatchDocument.DocumentType.IsNull() ? true : bd.DocumentTypeID.Equals(CBatchDocument.DocumentType)) &&
        //                                  (CheckNullLong(CBatchDocument.LocationID) ? true : bd.LocationID.Equals(CBatchDocument.LocationID)) &&
        //                                  (CheckNullLong(CBatchDocument.ProviderID) ? true : bd.ProviderID.Equals(CBatchDocument.ProviderID)) &&
        //                                 (CBatchDocument.Status.IsNull() ? true : bd.Status.Equals(CBatchDocument.Status))

        //                                 select new GBatchDocument()
        //                                 {
        //                                     BatchNumber = bd.ID,
        //                                     ChargesEntry = bdc.NoOfVisits,
        //                                     Payment = bdc.OtherPatientAmount,
        //                                     // Biller = b.LastName + ", " + b.FirstName,
        //                                     StartDate = DateTime.Now.Format("MM/dd/yyyy"),
        //                                     EndDate = DateTime.Now.Format("MM/dd/yyyy"),
        //                                     EntryDate = DateTime.Now.Format("MM/dd/yyyy"),
        //                                     ResponsibleParty = bd.ResponsibleParty,
        //                                     Demographics = bd.NoOfDemographics,
        //                                     Status = bd.Status,
        //                                     ChargeBatchDetail = "",
        //                                     AdmitBatchDetail = "",
        //                                     PaymentBatchDetail = "",
        //                                 }).ToList();
        //    return data;
        //}
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CBatchDocument CBatchDocument)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GBatchDocument> data = FindBatchDocument(CBatchDocument, PracticeId);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CBatchDocument, "Batch Document Report");

        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CBatchDocument CBatchDocument)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value );
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GBatchDocument> data = FindBatchDocument(CBatchDocument, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        [Route("DeleteBatchDocument/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBatchDocument(long id)
        {
            var DeleteBatchDocument = await _context.BatchDocument.FindAsync(id);

            if (DeleteBatchDocument == null)
            {
                return NotFound();
            }

            _context.BatchDocument.Remove(DeleteBatchDocument);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("SaveBatchDocument")]
        [HttpPost]
        public async Task<ActionResult<BatchDocument>> SaveBatchDocument(BatchDocument item)
        {
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (item.ID == 0)
                    {
                        FileUploadViewModel File = item.DocumentInfo;
                        if (File == null)
                            return BadRequest("File is required");

                        if (File != null)
                        {
                            Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
                            if (settings == null)
                            {
                                return BadRequest("Document Server Settings Not Found");
                            }

                            byte[] data = Convert.FromBase64String(File.Content.Substring(File.Content.IndexOf("base64,") + 7));
                            //string decodedString = Encoding.UTF8.GetString(data);

                            string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                            settings.DocumentServerDirectory, UD.ClientID.ToString(), "Documents", "0",
                            DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

                            Directory.CreateDirectory(directoryPath);
                            string FilePath = Path.Combine(directoryPath, File.Name);
                            //await System.IO.File.WriteAllTextAsync(FilePath, decodedString);
                            await System.IO.File.WriteAllBytesAsync(FilePath, data);

                            item.DocumentFilePath = FilePath;
                            item.FileName = File.Name;
                            item.FileSize = File.Size.ToString();
                            item.FileType = Path.GetExtension(File.Name);
                            try
                            {
                                iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(FilePath);
                                item.NumberOfPages = pdfReader.NumberOfPages;
                                pdfReader.Close();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        item.AddedBy = UD.Email;
                        item.AddedDate = DateTime.Now;
                        _context.BatchDocument.Add(item);
                    }
                    else
                    {
                        item.UpdatedBy = UD.Email;
                        item.UpdatedDate = DateTime.Now;
                        _context.BatchDocument.Update(item);
                    }
                    await _context.SaveChangesAsync();

                    if (item.BatchDocumentCharges != null)
                    {
                        foreach (BatchDocumentCharges batchDocumentCharges in item.BatchDocumentCharges)
                        {
                            if (batchDocumentCharges.ID <= 0)
                            {
                                batchDocumentCharges.BatchDocumentNoID = item.ID;
                                batchDocumentCharges.AddedBy = UD.Email;
                                batchDocumentCharges.AddedDate = DateTime.Now;
                                _context.BatchDocumentCharges.Add(batchDocumentCharges);
                            }
                            else
                            {
                                batchDocumentCharges.UpdatedBy = UD.Email;
                                batchDocumentCharges.UpdatedDate = DateTime.Now;
                                _context.BatchDocumentCharges.Update(batchDocumentCharges);
                            }
                        }
                    }

                    if (item.BatchDocumentPayment != null)
                    {
                        foreach (BatchDocumentPayment batchDocumentPayment in item.BatchDocumentPayment)
                        {
                            if (batchDocumentPayment.ID <= 0)
                            {
                                batchDocumentPayment.BatchDocumentNoID = item.ID;
                                batchDocumentPayment.AddedBy = UD.Email;
                                batchDocumentPayment.AddedDate = DateTime.Now;
                                _context.BatchDocumentPayment.Add(batchDocumentPayment);
                            }
                            else
                            {
                                batchDocumentPayment.UpdatedBy = UD.Email;
                                batchDocumentPayment.UpdatedDate = DateTime.Now;
                                _context.BatchDocumentPayment.Update(batchDocumentPayment);
                            }
                        }
                    }
                    if (item.Note != null)
                    {
                        foreach (Notes notes in item.Note)
                        {
                            if (notes.ID <= 0)
                            {
                                notes.BatchDocumentNoID = item.ID;
                                notes.AddedBy = UD.Email;
                                notes.AddedDate = DateTime.Now;
                                
                                _context.Notes.Add(notes);
                            }
                            else
                            {
                                notes.UpdatedBy = UD.Email;
                                notes.UpdatedDate = DateTime.Now;
                                _context.Notes.Update(notes);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();

                    objTrnScope.Complete();
                    objTrnScope.Dispose();
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Visit.txt"), ex.ToString());
                    throw ex;

                }
                finally
                {

                }
            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(item);
        }

        [Route("DownloadBatchDocument/{ID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadBatchDocument(long ID)
        {
            BatchDocument document = await _context.BatchDocument.FindAsync(ID);
            if (document != null)
            {
                if (!System.IO.File.Exists(document.DocumentFilePath))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(document.DocumentFilePath);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                return File(stream, "application/octec-stream", "837p.pdf");
            }

            return NotFound();
        }

        [Route("FindAudit/{BatchDocumentID}")]
        [HttpGet("{BatchDocumentID}")]
        public List<BatchDocumentAudit> FindAudit(long BatchDocumentID)
        {
            List<BatchDocumentAudit> data = (from pAudit in _context.BatchDocumentAudit
                                             where pAudit.BatchDocumentID == BatchDocumentID orderby pAudit.AddedDate descending
                                             select new BatchDocumentAudit()
                                             {
                                                 ID = pAudit.ID,
                                                 BatchDocumentID = pAudit.BatchDocumentID,
                                                 TransactionID = pAudit.TransactionID,
                                                 ColumnName = pAudit.ColumnName,
                                                 CurrentValue = pAudit.CurrentValue,
                                                 NewValue = pAudit.NewValue,
                                                 CurrentValueID = pAudit.CurrentValueID,
                                                 NewValueID = pAudit.NewValueID,
                                                 HostName = pAudit.HostName,
                                                 AddedBy = pAudit.AddedBy,
                                                 AddedDate = pAudit.AddedDate,
                                             }).ToList<BatchDocumentAudit>();
          
            return data;
        }
        [Route("BatchDocumentChargesById/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BatchDocumentChargesById(long id)
        {
            
            var BatDocChargesId = await _context.BatchDocumentCharges.FindAsync(id);

            if (BatDocChargesId == null)
            {
                return BadRequest("Record Not Found.");
            }
            _context.BatchDocumentCharges.Remove(BatDocChargesId);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("BatchDocumentPaymentById/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BatchDocumentPaymentById(long id)
        {
            var BatDocPaymentId = await _context.BatchDocumentPayment.FindAsync(id);

            if (BatDocPaymentId == null)
            {
                return BadRequest("Record Not Found.");
            }
            _context.BatchDocumentPayment.Remove(BatDocPaymentId);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("DeleteBatchDocumentCharge/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBatchDocumentCharge(long id)
        {

            var bCharge = await _context.BatchDocumentCharges.FindAsync(id);

            if (bCharge == null)
            {
                return BadRequest("No Record Found.");
            }

            _context.BatchDocumentCharges.Remove(bCharge);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("DeleteBatchDocumentPayment/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBatchDocumentPayment(long id)
        {
            
            var bPayment = await _context.BatchDocumentPayment.FindAsync(id);

            if (bPayment == null)
            {
                return BadRequest("No Record Found.");
            }
            _context.BatchDocumentPayment.Remove(bPayment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("DeleteBatchDocumentNote/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBatchDocumentNote(long id)
        {
            var bNote = await _context.Notes.FindAsync(id);

            if (bNote == null)
            {
                return BadRequest("No Record Found.");
            }
            _context.Notes.Remove(bNote);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        private bool CheckNullLong(long? Value)
        {
            return Value.IsNull();
        }

    }
}