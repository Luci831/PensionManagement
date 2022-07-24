using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDetailsMicroservice.Models;
using Microsoft.AspNetCore.Authorization;

namespace UserDetailsMicroservice.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PensionerDetailsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public PensionerDetailsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/PensionerDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PensionerDetails>>> GetPensionerDetails()
        {
            return await _context.PensionerDetails.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<PensionerDetails>>> GetPensionerDetailsByUser(int id)
        {
            return await _context.PensionerDetails.Where(x => x.UserId == id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PensionerDetails>> GetPensionerDetails(int id)
        {
            var pensionerDetails = await _context.PensionerDetails.FindAsync(id);

            if (pensionerDetails == null)
            {
                return NotFound();
            }

            return pensionerDetails;
        }

        // PUT: api/PensionerDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPensionerDetails(int id, PensionerDetails pensionerDetails)
        {
            if (id != pensionerDetails.Prid)
            {
                return BadRequest();
            }

            _context.Entry(pensionerDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PensionerDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PensionerDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PensionerDetails>> PostPensionerDetails(PensionerDetails pensionerDetails)
        {
            _context.PensionerDetails.Add(pensionerDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPensionerDetails", new { id = pensionerDetails.Prid }, pensionerDetails);
        }

        // DELETE: api/PensionerDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePensionerDetails(int id)
        {
            var pensionerDetails = await _context.PensionerDetails.FindAsync(id);
            if (pensionerDetails == null)
            {
                return NotFound();
            }

            _context.PensionerDetails.Remove(pensionerDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PensionerDetailsExists(int id)
        {
            return _context.PensionerDetails.Any(e => e.Prid == id);
        }
    }
}
