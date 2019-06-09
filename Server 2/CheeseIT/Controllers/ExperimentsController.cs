using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheeseIT.Models;
using CheeseIT.BusinessLogic.Interfaces;

namespace CheeseIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperimentsController : ControllerBase
    {
        private readonly CheeseContext _context;
        private readonly IExperimentServices _experimentServices;

        public ExperimentsController(CheeseContext context, IExperimentServices experimentServices)
        {
            _context = context;
            _experimentServices = experimentServices;
        }

        // GET: api/Experiments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Experiment>>> GetExperiments()
        {
            return await _context.Experiments.ToListAsync();
        }

        // GET: api/Experiments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Experiment>> GetExperiment(Guid id)
        {
            var experiment = await _context.Experiments.FindAsync(id);

            if (experiment == null)
            {
                return NotFound();
            }

            return experiment;
        }

        // PUT: api/Experiments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExperiment(Guid id, Experiment experiment)
        {
            if (id != experiment.Id)
            {
                return BadRequest();
            }

            _context.Entry(experiment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperimentExists(id))
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

        // POST: api/Experiments
        [HttpPost]
        public async Task<ActionResult<Experiment>> PostExperiment(Experiment experiment)
        {
            _context.Experiments.Add(experiment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExperiment", new { id = experiment.Id }, experiment);
        }

        // DELETE: api/Experiments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Experiment>> DeleteExperiment(Guid id)
        {
            var experiment = await _context.Experiments.FindAsync(id);
            if (experiment == null)
            {
                return NotFound();
            }

            _context.Experiments.Remove(experiment);
            await _context.SaveChangesAsync();

            return experiment;
        }

        private bool ExperimentExists(Guid id)
        {
            return _context.Experiments.Any(e => e.Id == id);
        }

        // POST: api/Experiments/observation
        [HttpPost]
        [Route("observation")]
        public async Task<ActionResult<Observation>> PostObservation([FromBody] Observation observation)
        {


            return Ok();
        }

        // POST: api/Experiments/measure
        [HttpPost]
        [Route("measure")]
        public async Task<ActionResult<string>> PostMeasure([FromBody] Measurement measure)
        {
            Experiment currentExperiment = await _experimentServices.GetCurrentExperiment();

            if (currentExperiment == null)
            {
                return StatusCode(412, "There is no active ripening");
            }

            measure.DateTime = DateTime.Now;
            currentExperiment.Measurements.Add(measure);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return NotFound();
            }

            //Revisar si la medida actual esta dentro de lo estandar
            _experimentServices.ValidateMeasure(measure);

            return Created("measure", measure);
        }

        // GET: api/Experiments/current
        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<Experiment>> GetCurrentRipening()
        {
            Experiment experiment = await _experimentServices.GetCurrentExperiment();
            if (experiment != null)
            {
                return experiment;
            }
            else
            {
                return StatusCode(412, "There is no active ripening");
            }
        }

    }
}
