using CheeseIT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic
{
    public class RipeningServices
    {
        private readonly CheeseContext _context;

        public RipeningServices(CheeseContext context)
        {
            _context = context;
        }

        public Ripening CreateRipening(Guid cheeseId)
        {
            Ripening ripening = new Ripening()
            {
                StartDate = DateTime.Now
            };
            Cheese cheese = _context.Cheeses.Find(cheeseId);
            ripening.Cheese = cheese;

            return ripening;
        }

        internal Ripening FinishRipening(Guid ripeningId)
        {
            Ripening ripening = _context.Ripenings.Find(ripeningId);
            ripening.EndTime = DateTime.Now;

            return ripening;
        }

        internal async Task<Ripening> GetCurrentRipeningModel()
        {
            return await _context.Ripenings.Include(rip => rip.Measurements).Where(r => r.EndTime == DateTime.MinValue).FirstOrDefaultAsync();
        }
    }
}
