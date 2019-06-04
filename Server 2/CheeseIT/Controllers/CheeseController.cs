using CheeseIT.BusinessLogic;
using CheeseIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheeseController : ControllerBase
    {
        private readonly CheeseContext _context;
        private readonly CheeseServices _cheeseServices;

        public CheeseController(CheeseContext context)
        {
            _context = context;
            _cheeseServices = new CheeseServices();
        }

        // GET: api/Cheese
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cheese>>> GetCheeses()
        {
            return await _context.Cheeses.ToListAsync();
        }

        [HttpGet]
        [Route("test")]
        public ActionResult<string> GetTest()
        {
            return "Hola oreo";
        }

        // GET: api/Cheese/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cheese>> GetCheese(Guid id)
        {
            var cheese = await _context.Cheeses.FindAsync(id);

            if (cheese == null)
            {
                return NotFound();
            }

            return cheese;
        }

        // PUT: api/Cheese/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheese(Guid id, Cheese cheese)
        {
            if (id != cheese.Id)
            {
                return BadRequest();
            }

            _context.Entry(cheese).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheeseExists(id))
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

        // POST: api/Cheese
        [HttpPost]
        public async Task<ActionResult<Cheese>> PostCheese(Cheese cheese)
        {
            string filepath = "";
            if (!string.IsNullOrWhiteSpace(cheese.Base64Image))
            {
                filepath = _cheeseServices.ProcessImage(cheese.Base64Image);
            }
            cheese.Base64Image = filepath;
            _context.Cheeses.Add(cheese);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheese", new { id = cheese.Id }, cheese);
        }

        // DELETE: api/Cheese/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cheese>> DeleteCheese(Guid id)
        {
            var cheese = await _context.Cheeses.FindAsync(id);
            if (cheese == null)
            {
                return NotFound();
            }

            _context.Cheeses.Remove(cheese);
            await _context.SaveChangesAsync();

            return cheese;
        }

        private bool CheeseExists(Guid id)
        {
            return _context.Cheeses.Any(e => e.Id == id);
        }
    }
}
