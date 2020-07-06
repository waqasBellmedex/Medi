using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionalDataController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public InstitutionalDataController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }


        [Route("SaveInstitutionalVisit")]
        [HttpPost]
        public async Task<ActionResult<InstitutionalData>> SaveInstitutionalVisit(InstitutionalData item)
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
            InstitutionalData data = _context.InstitutionalData.Where(c => c.ID == item.ID ).SingleOrDefault();

            if(data == null)

            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context.InstitutionalData.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                
                _context.InstitutionalData.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok(item);
        }


    }
    }
