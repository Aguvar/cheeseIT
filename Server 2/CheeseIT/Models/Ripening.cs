using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Models
{
    public class Ripening
    {
        public Guid Id { get; set; }

        public Cheese Cheese { get; set; }
        public IList<Measurement> Measurements { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
    }
}
