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
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMDocumentType;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    public class DocumentTypeController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public DocumentTypeController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

       [Route("GetDocumentTypes")]
        public async Task<ActionResult<IEnumerable<DocumentType>>> GetDocumentTypes()
        {

            return await _context.DocumentType.ToListAsync();
        }


        [Route("FindDocumentType/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentType>> FindDocumentType(long id)
        {
          
            var po = await _context.DocumentType.FindAsync(id);

            if (po == null)
            {
                return NotFound();
            }

            return po;
        }

        [HttpPost]
        [Route("FindDocumentTypes")]
        public async Task<ActionResult<IEnumerable<GDocumentType>>> FindReasons(CDocumentType CDocumentType)
        {
            return await (from s in _context.DocumentType
                          where
                          (CDocumentType.Name == null ? true : s.Name.Contains(CDocumentType.Name))
                        &&(CDocumentType.Description == null ? true : s.Description.Contains(CDocumentType.Description))
                          select new GDocumentType()
                          {
                              ID = s.ID,
                              Name = s.Name,
                              Description=s.Description,
                              AddedBy=s.AddedBy,
                              AddedDate=s.AddedDate.Format("MM/dd/yyyy")

                          }).ToListAsync();
        }



        [Route("SaveDocumentTypes")]
        [HttpPost]
        public async Task<ActionResult<DocumentType>> SaveDocumentTypes(DocumentType item)
        {
            //string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            try
            {
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return BadRequest(messages);
                }

                if (item.ID == 0)
                {
                    item.AddedBy = Email;
                    item.AddedDate = DateTime.Now;
                    _context.DocumentType.Add(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    item.UpdatedBy = Email;
                    item.UpdatedDate = DateTime.Now;
                    _context.DocumentType.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
            return Ok(item);
        }


        // DELETE: api/Typs/5
        [Route("DeleteDocumentType/{id}")]
        [HttpDelete]
        public async Task<ActionResult<DocumentType>> DeleteDocumentType(long id)
        {
            var pr = await _context.DocumentType.FindAsync(id);

            if (pr == null)
            {
                return NotFound();
            }

            _context.DocumentType.Remove(pr);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool TypExists(long id)
        {
            return _context.DocumentType.Any(e => e.ID == id);
        }

        [Route("FindAudit/{DocumentTypeID}")]
        [HttpGet("{DocumentTypeID}")]
        public List<DocumentTypeAudit> FindAudit(long DocumentTypeID)
        {
            List<DocumentTypeAudit> data = (from pAudit in _context.DocumentTypeAudit
                                            where pAudit.DocumentTypeID == DocumentTypeID
                                            orderby pAudit.AddedDate descending
                                            select new DocumentTypeAudit()
                                            {
                                                ID = pAudit.ID,
                                                DocumentTypeID = pAudit.DocumentTypeID,
                                                TransactionID = pAudit.TransactionID,
                                                ColumnName = pAudit.ColumnName,
                                                CurrentValue = pAudit.CurrentValue,
                                                NewValue = pAudit.NewValue,
                                                CurrentValueID = pAudit.CurrentValueID,
                                                NewValueID = pAudit.NewValueID,
                                                HostName = pAudit.HostName,
                                                AddedBy = pAudit.AddedBy,
                                                AddedDate = pAudit.AddedDate,
                                            }).ToList<DocumentTypeAudit>();

            return data;
        }

    }
}