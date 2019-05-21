using CheeseIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.DTOs
{
    public class MeasurementDTO
    {
        public float temperature { get; set; }
        public float humidity { get; set; }
        public string date { get; set; }

        public static MeasurementDTO Transform(Measurement measurement)
        {
            MeasurementDTO dto = new MeasurementDTO();

            dto.temperature = measurement.Temperature;
            dto.humidity = measurement.Humidity;
            dto.date = measurement.DateTime.ToString();

            return dto;
        }

    }
}
