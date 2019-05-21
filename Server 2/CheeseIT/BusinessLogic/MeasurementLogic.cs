using CheeseIT.Models;
using System;

namespace CheeseIT.BusinessLogic
{
    public class MeasurementLogic
    {
        private CheeseContext db;

        public MeasurementLogic(CheeseContext cheeseContext)
        {
            db = cheeseContext;
        }

        internal void AddMeasurement(float temperature, float humidity)
        {
            Measurement measurement = new Measurement()
            {
                Id = Guid.NewGuid(),
                Temperature = temperature,
                Humidity = humidity,
                DateTime = DateTime.Now
            };

        }
    }
}
