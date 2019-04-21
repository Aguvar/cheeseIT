using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Models
{
    public class Experiment
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public float IdealHumidity { get; set; }
        public float HumidityThreshold { get; set; }
        public float IdealTemperature { get; set; }
        public float TemperatureThreshold { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EstimatedEndTime { get; set; }
        public DateTime EndTime { get; set; }

        public IList<Observation> Observations { get; set; }

        public IList<Measurement> Measurements { get; set; }
    }
}
