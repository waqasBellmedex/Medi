using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class TaxonomyController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public TaxonomyController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;


        }
        [Route("SaveTaxonomy")]
        [HttpPost]
        public async Task<ActionResult<Taxonomy>> SaveTaxonomy(Taxonomy Item)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(Item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (Item.ID == 0)
            {
                Item.UpdateBy = Email;
                Item.AddedDate = DateTime.Now;
                _context.Taxonomy.Add(Item);
                await _context.SaveChangesAsync();
            }
            else
            {
                Item.UpdateBy = Email;
                Item.AddedDate = DateTime.Now;
                _context.Taxonomy.Update(Item);
                await _context.SaveChangesAsync();
            }
            return Ok(Item);
        }

        [Route("DeleteTaxonomy/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteTaxonomy(long id)
        {
            var Tax = await _context.Taxonomy.FindAsync(id);
            if (Tax == null)
            {
                return BadRequest("Record Not Found");
            }

            _context.Taxonomy.Remove(Tax);
            await _context.SaveChangesAsync();
            return Ok();
        }




    }
}