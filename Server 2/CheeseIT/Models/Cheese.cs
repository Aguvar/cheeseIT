using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Models
{
    public class Cheese
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Base64Image { get; set; }

        public float IdealHumidity { get; set; }
        public float HumidityThreshold { get; set; }
        public float IdealTemperature { get; set; }
        public float TemperatureThreshold { get; set; }

        public int DaysToRipe { get; set; }
    }
}
