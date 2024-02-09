using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExileTrack.Models;
//https://None@exiletrack.scm.azurewebsites.net/ExileTrack.git
namespace ExileTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExileItemsController : ControllerBase
    {
        private readonly Context _context;

        public ExileItemsController(Context context)
        {
            _context = context;
        }

        [HttpGet("BeastProfit")]
        public async Task<IActionResult> GetBeastProfits()
        {
            // Await the async operation and get the result
            Dictionary<string, double> value = await ExilePipeline.GetBeasts("Farric Tiger Alpha");


            return Ok(value);
        }

        [HttpGet("FlaskProfit")]
        public async Task<IActionResult> GetFlaskProfit()
        {
            // Await the async operation and get the result
            Dictionary<string, Dictionary<string, double>> value = await ExilePipeline.GetFlaskProfits();


            return Ok(value);
        }

       
//"url": "https://None@exiletrack.scm.azurewebsites.net/ExileTrack.git"



        // GET: api/ExileItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExileItem>>> GetGetItems()
        {
            return await _context.GetItems.ToListAsync();
        }

        // GET: api/ExileItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExileItem>> GetExileItem(long id)
        {
            var exileItem = await _context.GetItems.FindAsync(id);

            if (exileItem == null)
            {
                return NotFound();
            }

            return exileItem;
        }

        // PUT: api/ExileItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExileItem(long id, ExileItem exileItem)
        {
            if (id != exileItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(exileItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExileItemExists(id))
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

        // POST: api/ExileItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExileItem>> PostExileItem(ExileItem exileItem)
        {
            _context.GetItems.Add(exileItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExileItem", new { id = exileItem.Id }, exileItem);
        }

        // DELETE: api/ExileItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExileItem(long id)
        {
            var exileItem = await _context.GetItems.FindAsync(id);
            if (exileItem == null)
            {
                return NotFound();
            }

            _context.GetItems.Remove(exileItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExileItemExists(long id)
        {
            return _context.GetItems.Any(e => e.Id == id);
        }
    }
}
