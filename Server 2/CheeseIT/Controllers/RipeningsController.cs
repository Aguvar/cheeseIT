using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheeseIT.Models;
using Microsoft.Extensions.Logging;
using CheeseIT.DTOs;

namespace CheeseIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RipeningsController : ControllerBase
    {
        private readonly CheeseContext _context;
        private readonly ILogger<RipeningsController> _logger;

        public RipeningsController(CheeseContext context, ILogger<RipeningsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Ripenings
        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<IEnumerable<Ripening>>> GetRipenings()
        {
            return await _context.Ripenings.ToListAsync();
        }

        // GET: api/Ripenings/current
        [HttpGet]
        public async Task<ActionResult<Ripening>> GetCurrentRipening()
        {
            return await _context.Ripenings.Where(r => r.EndTime == null).FirstOrDefaultAsync();
        }

        // POST: api/Ripenings/measure
        [HttpPost]
        [Route("measure")]
        public ActionResult<string> PostMeasure([FromBody] Measurement measure)
        {
            _logger.LogInformation($"Humedad: {measure.Humidity.ToString()}");
            _logger.LogInformation($"Temperatura: {measure.Temperature.ToString()}");
            return "Recibido";
        }

        // GET: api/Ripenings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ripening>> GetRipening(Guid id)
        {
            var ripening = await _context.Ripenings.FindAsync(id);

            if (ripening == null)
            {
                return NotFound();
            }

            return ripening;
        }

        // PUT: api/Ripenings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRipening(Guid id, Ripening ripening)
        {
            if (id != ripening.Id)
            {
                return BadRequest();
            }

            _context.Entry(ripening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RipeningExists(id))
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

        // POST: api/Ripenings
        [HttpPost]
        public async Task<ActionResult<Ripening>> PostRipening(Ripening ripening)
        {
            _context.Ripenings.Add(ripening);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRipening", new { id = ripening.Id }, ripening);
        }

        //// DELETE: api/Ripenings/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Ripening>> DeleteRipening(Guid id)
        //{
        //    var ripening = await _context.Ripenings.FindAsync(id);
        //    if (ripening == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Ripenings.Remove(ripening);
        //    await _context.SaveChangesAsync();

        //    return ripening;
        //}

        private bool RipeningExists(Guid id)
        {
            return _context.Ripenings.Any(e => e.Id == id);
        }
    }
}
