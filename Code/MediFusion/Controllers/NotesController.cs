using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public NotesController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }


        // GET: api/Notes
        [HttpGet]
        [Route("GetNotes")]
        public async Task<ActionResult<IEnumerable<Notes>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
        }


        // Save Notes
        [Route("SaveNotes")]
        [HttpPost]
        public async Task<ActionResult<Notes>> SaveNotes(Notes item)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            bool succ = TryValidateModel(item);
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
                _context.Notes.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.Notes.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok(item);
        }

        // PUT: api/Notes/5
        [Route("FindNotes/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> FindNotes(long id)
        {
            var notes = await _context.Notes.FindAsync(id);

            if (notes == null)
            {
                return NotFound();
            }
            else
            return notes;
        }

        // DELETE: api/Notes/5
        [Route("DeleteNotes/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(long id)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var note = await _context.Notes.FindAsync(id);
            DateTime today = DateTime.Now;
            try
            {
                   
                   if(((DateTime)note.AddedDate).Date != today.Date)
                {
                    return BadRequest("Old Notes cannot be deleted");
                }
                if(note.AddedBy != Email)
                {
                    return BadRequest("Can Not Delete Notes Of Any Other User.");
                }
                else
                {
                    _context.Notes.Remove(note);
                    await _context.SaveChangesAsync();
                   
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + "  " + ex.StackTrace);
            }
            return Ok(id);
        }

        [Route("FindNotesByFollowUpId/{PlanFollowupID}")]
        [HttpGet("{PlanFollowupID}")]
        public  List<Notes> FindNotesByFollowUpId(long PlanFollowupID)
        {
            List<Notes> notes = _context.Notes.Where(c => c.PlanFollowupID == PlanFollowupID ).ToList<Notes>();
            if (notes == null)
            {
                return null;
            }
            else
            return notes;
            
        }

        [Route("FindAudit/{NotesID}")]
        [HttpGet("{NotesID}")]
        public List<NotesAudit> FindAudit(long NotesID)
        {
            List<NotesAudit> data = (from pAudit in _context.NotesAudit
                                     where pAudit.NotesID == NotesID
                                     orderby pAudit.AddedDate descending
                                     select new NotesAudit()
                                     {
                                         ID = pAudit.ID,
                                         NotesID = pAudit.NotesID,
                                         TransactionID = pAudit.TransactionID,
                                         ColumnName = pAudit.ColumnName,
                                         CurrentValue = pAudit.CurrentValue,
                                         NewValue = pAudit.NewValue,
                                         CurrentValueID = pAudit.CurrentValueID,
                                         NewValueID = pAudit.NewValueID,
                                         HostName = pAudit.HostName,
                                         AddedBy = pAudit.AddedBy,
                                         AddedDate = pAudit.AddedDate,
                                     }).ToList<NotesAudit>();
            return data;
        }

    }
}