using CheeseIT.BusinessLogic;
using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RipeningsController : ControllerBase
    {
        private readonly CheeseContext _context;
        private readonly ILogger<RipeningsController> _logger;
        private readonly IRipeningServices _ripeningServices;

        public RipeningsController(CheeseContext context, ILogger<RipeningsController> logger, IRipeningServices services)
        {
            _context = context;
            _logger = logger;
            _ripeningServices = services;
        }

        // GET: api/Ripenings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ripening>>> GetRipenings()
        {
            return await _context.Ripenings.ToListAsync();
        }

        // GET: api/Ripenings/current
        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<Ripening>> GetCurrentRipening()
        {
            Ripening ripening = await _ripeningServices.GetCurrentRipeningModel();
            if (ripening != null)
            {
                return ripening;
            }
            else
            {
                return StatusCode(412, "There is no current ripening");
            }
        }

        // POST: api/Ripenings/measure
        [HttpPost]
        [Route("measure")]
        public async Task<ActionResult<string>> PostMeasure([FromBody] Measurement measure)
        {
            _logger.LogInformation($"Humedad: {measure.Humidity.ToString()}");
            _logger.LogInformation($"Temperatura: {measure.Temperature.ToString()}");

            Ripening currentRipening = await _ripeningServices.GetCurrentRipeningModel();

            if (currentRipening == null)
            {
                return StatusCode(412, "There is no current ripening");
            }

            measure.DateTime = DateTime.Now;
            currentRipening.Measurements.Add(measure);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return NotFound();
            }

            //Revisar si la medida actual esta dentro de lo estandar
            _ripeningServices.ValidateMeasure(measure);

            return Created("measure", measure);
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

            return Ok(ripening);
        }

        // POST: api/Ripenings
        [HttpPost]
        public async Task<ActionResult<Ripening>> PostRipening(string cheeseId)
        {
            if (_ripeningServices.GetCurrentRipeningModel() == null)
            {
                return StatusCode(412, "There is an ongoing ripening already. Please end the previous one before starting a new one");
            }
            try
            {
                Guid cheeseGuid = Guid.Parse(cheeseId);
                Ripening ripening = _ripeningServices.CreateRipening(cheeseGuid);

                _context.Ripenings.Add(ripening);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRipening", new { id = ripening.Id }, ripening);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut]
        [Route("{ripeningId}/end")]
        public async Task<IActionResult> EndRipening(string ripeningId)
        {
            if (!RipeningExists(Guid.Parse(ripeningId)))
            {
                return NotFound();
            }

            Ripening ripening = _ripeningServices.FinishRipening(Guid.Parse(ripeningId));

            _context.Entry(ripening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RipeningExists(Guid.Parse(ripeningId)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(ripening);
        }

        private bool RipeningExists(Guid id)
        {
            return _context.Ripenings.Any(e => e.Id == id);
        }
    }
}
