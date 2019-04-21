using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Models
{
    public class Measurement
    {
        public Guid Id { get; set; }
        public float Humidity { get; set; }
        public float Temperature { get; set; }
        public DateTime DateTime { get; set; }
    }
}
