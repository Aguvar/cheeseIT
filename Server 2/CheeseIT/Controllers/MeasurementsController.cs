using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseIT.BusinessLogic;
using CheeseIT.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheeseIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        // GET: api/Measurements
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Measurements/5
        [HttpGet("{id}", Name = "GetMeasure")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Measurements
        [HttpPost]
        public HttpResponse Post([FromBody] MeasurementDTO value)
        {
            //logic.AddMeasurement(value.temperature, value.humidity);

            Response.StatusCode = 200;

            return Response;
        }

        // PUT: api/Measurements/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
