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

        //Vamos a pensar que esto es el filepath a la imagen porque no pinta guardar una imagen entera en la base de datos, y las migraciones de .net core todavia no soportan renombrar columnas
        public string Base64Image { get; set; }

        public float IdealHumidity { get; set; }
        public float HumidityThreshold { get; set; }
        public float IdealTemperature { get; set; }
        public float TemperatureThreshold { get; set; }

        public int DaysToRipe { get; set; }
    }
}
