using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using MediFusionPM.Models.Main;
using MediFusionPM.Models.Audit;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationsController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public DesignationsController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        // GET: api/Designations
        [HttpGet]
        [Route("GetDesignations")]
        public IEnumerable<MainDesignations> GetDesignations()
        {
            return _contextMain.MainDesignations;

        }

        // GET: api/Designations/5
        [HttpGet("{id}")]
        [Route("GetDesignations/{id}")]
        public async Task<IActionResult> GetDesignations([FromRoute] long id)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var designations = await _contextMain.MainDesignations.FindAsync(id);

            if (designations == null)
            {
                return NotFound();
            }
            
            return Ok(designations);
        }


        // POST: api/Designations
        [HttpPost]
        [Route("SaveDesignations")]
        public async Task<IActionResult> SaveDesignations(MainDesignations designations)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (designations.ID.IsNull())
            {
                _contextMain.MainDesignations.Add(designations);
                await _contextMain.SaveChangesAsync();
                Designations des = new Designations();
                des.ID = designations.ID;
                des.Name = designations.Name;
           //     _context.Designations.Add(des);
             //   await _context.SaveChangesAsync();
            }
            else
            {
                var OldTeam = await _contextMain.MainTeam.FindAsync(designations.ID);
                OldTeam.Name = designations.Name;
                _contextMain.Entry(OldTeam).State = EntityState.Modified;
                await _contextMain.SaveChangesAsync();

                var des = await _contextMain.MainTeam.FindAsync(designations.ID);
                des.Name = designations.Name;
               // _context.Entry(des).State = EntityState.Modified;
                //await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetDesignations", new { id = designations.ID }, designations);
        }

        // DELETE: api/Designations/5
        [HttpDelete("{id}")]
        [Route("DeleteDesignations/{id}")]
        public async Task<IActionResult> DeleteDesignations([FromRoute] long id)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var designations = await _contextMain.MainDesignations.FindAsync(id);
            if (designations == null)
            {
                return NotFound();
            }

            _contextMain.MainDesignations.Remove(designations);
            await _contextMain.SaveChangesAsync();

            return Ok(designations);
        }

        private bool DesignationsExists(long id)
        {
            return _contextMain.MainDesignations.Any(e => e.ID == id);
        }
        [Route("FindAudit/{DesignationsID}")]
        [HttpGet("{DesignationsID}")]
        public List<DesignationsAudit> FindAudit(long DesignationsID)
        {
           

            List<DesignationsAudit> data = (from pAudit in _context.DesignationsAudit
                                            where pAudit.DesignationsID == DesignationsID
                                            orderby pAudit.AddedDate descending
                                            select new DesignationsAudit()
                                           {
                                               ID = pAudit.ID,
                                               DesignationsID = pAudit.DesignationsID,
                                               TransactionID = pAudit.TransactionID,
                                               ColumnName = pAudit.ColumnName,
                                               CurrentValue = pAudit.CurrentValue,
                                               NewValue = pAudit.NewValue,
                                               CurrentValueID = pAudit.CurrentValueID,
                                               NewValueID = pAudit.NewValueID,
                                               HostName = pAudit.HostName,
                                               AddedBy = pAudit.AddedBy,
                                               AddedDate = pAudit.AddedDate,
                                           }).ToList<DesignationsAudit>();
            
            return data;
        }



    }

}