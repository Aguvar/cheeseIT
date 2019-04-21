using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Models
{
    public class Observation
    {
        public Guid Id { get; set; }

        public string Base64Image { get; set; }
        public string Note { get; set; }
        public DateTime DateTime { get; set; }

    }
}
