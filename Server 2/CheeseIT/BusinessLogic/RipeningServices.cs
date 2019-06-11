using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic
{
    public class RipeningServices : IRipeningServices
    {
        private readonly CheeseContext _context;
        private readonly IMobileMessagingService _messaging;

        public RipeningServices(CheeseContext context, IMobileMessagingService messaging)
        {
            _context = context;
            _messaging = messaging;
        }

        public Ripening CreateRipening(Guid cheeseId)
        {
            Ripening ripening = new Ripening()
            {
                StartDate = DateTime.Now
            };
            Cheese cheese = _context.Cheeses.Find(cheeseId);
            ripening.Cheese = cheese ?? throw new EntityNotFoundException($"Could not find entity 'Cheese' for Id {cheeseId}");

            return ripening;
        }

        public Ripening FinishRipening(Guid ripeningId)
        {
            Ripening ripening = _context.Ripenings.Find(ripeningId) ?? throw new EntityNotFoundException($"Could not find entity 'Ripening' for Id {ripeningId}");
            ripening.EndTime = DateTime.Now;

            return ripening;
        }

        public async Task<Ripening> GetCurrentRipening()
        {
            return await _context.Ripenings.Include(rip => rip.Measurements).Include(rip => rip.Cheese).Where(r => r.EndTime == null).FirstOrDefaultAsync();
        }

        public void ValidateMeasure(Measurement measure)
        {
            Cheese currentCheese = GetCurrentRipening().Result.Cheese;

            string message = "";

            if (Math.Abs(measure.Humidity - currentCheese.IdealHumidity) > currentCheese.HumidityThreshold)
            {
                message += $"Se ha registrado una humedad {(measure.Humidity - currentCheese.IdealHumidity) - currentCheese.HumidityThreshold}% por fuera del rango establecido en la maduracion actual. \n";
            }

            if (Math.Abs(measure.Temperature - currentCheese.IdealTemperature) > currentCheese.TemperatureThreshold)
            {
                message += $"Se ha registrado una temperatura {(measure.Temperature - currentCheese.IdealTemperature) - currentCheese.TemperatureThreshold}°C por fuera del rango establecido en la maduracion actual. \n";
            }

            if (!string.IsNullOrEmpty(message))
            {
                string token = TokenRepository.GetInstance().FirebaseToken;

                _messaging.SendNotification(token, "Alerta de Medicion", message);
            }
        }
    }
}
