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
        private readonly ICloudinaryServices _cloudinaryServices;

        public ExperimentsController(CheeseContext context, IExperimentServices experimentServices, ICloudinaryServices cloudinaryServices)
        {
            _context = context;
            _experimentServices = experimentServices;
            _cloudinaryServices = cloudinaryServices;
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
            var experiment = await _context.Experiments.Include(exp => exp.Observations).Include(exp => exp.Measurements).Where(exp => exp.Id.Equals(id)).FirstOrDefaultAsync();

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
            if (await _experimentServices.GetCurrentExperiment() != null)
            {
                return StatusCode(412, "There is an active experiment already, please end it before starting a new one.");
            }
            experiment.StartDate = DateTime.Now;
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

        // POST: api/Experiments/{id}/observations
        [HttpPost]
        [Route("{experimentId}/observations")]
        public async Task<ActionResult<Observation>> PostObservation([FromBody] Observation observation, [FromRoute] string experimentId)
        {
            Guid experimentGuid;
            try
            {
                experimentGuid = Guid.Parse(experimentId);
            }
            catch (Exception)
            {
                return BadRequest("Malformed ID");
            }

            if (ExperimentExists(experimentGuid))
            {
                //Process image, if any
                string filepath = "";
                if (!string.IsNullOrWhiteSpace(observation.Base64Image))
                {
                    filepath = _cloudinaryServices.ProcessImage(observation.Base64Image);
                }
                observation.Base64Image = filepath;

                //Add observation to experiment and save
                Experiment experiment = await _context.Experiments.FindAsync(experimentGuid);

                observation.Id = Guid.NewGuid();
                observation.DateTime = DateTime.Now;

                experiment.Observations.Add(observation);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    return NotFound();
                }

                return Ok();
            }
            else
            {
                return NotFound($"No experiment with ID {experimentId}");
            }

        }

        // POST: api/Experiments/current/measures
        [HttpPost]
        [Route("current/measures")]
        public async Task<ActionResult<string>> PostMeasure([FromBody] Measurement measure)
        {
            Experiment currentExperiment = await _experimentServices.GetCurrentExperiment();

            if (currentExperiment == null)
            {
                return StatusCode(412, "There is no active experiment");
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
        public async Task<ActionResult<Experiment>> GetCurrentExperiment()
        {
            Experiment experiment = await _experimentServices.GetCurrentExperiment();
            if (experiment != null)
            {
                return experiment;
            }
            else
            {
                return StatusCode(412, "There is no active experiment");
            }
        }

    }
}
