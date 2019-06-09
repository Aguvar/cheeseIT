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
            ripening.Cheese = cheese;

            return ripening;
        }

        public Ripening FinishRipening(Guid ripeningId)
        {
            Ripening ripening = _context.Ripenings.Find(ripeningId);
            ripening.EndTime = DateTime.Now;

            return ripening;
        }

        public async Task<Ripening> GetCurrentRipeningModel()
        {
            return await _context.Ripenings.Include(rip => rip.Measurements).Include(rip => rip.Cheese).Where(r => r.EndTime == DateTime.MinValue).FirstOrDefaultAsync();
        }

        public void ValidateMeasure(Measurement measure)
        {
            Cheese currentCheese = GetCurrentRipeningModel().Result.Cheese;

            string message = "";

            if (Math.Abs(measure.Humidity - currentCheese.IdealHumidity) > currentCheese.HumidityThreshold)
            {
                message += $"Se ha registrado una humedad {(measure.Humidity - currentCheese.IdealHumidity) - currentCheese.HumidityThreshold}% por fuera del rango establecido. \n";
            }

            if (Math.Abs(measure.Temperature - currentCheese.IdealTemperature) > currentCheese.TemperatureThreshold)
            {
                message += $"Se ha registrado una temperatura {(measure.Temperature - currentCheese.IdealTemperature) - currentCheese.TemperatureThreshold}°C por fuera del rango establecido. \n";
            }

            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Se deberia haber mandado una notificacion");
                string token = TokenRepository.GetInstance().FirebaseToken;
                if (!string.IsNullOrEmpty(token))
                {
                    _messaging.SendNotification(token, "Alerta de Medicion", message);

                }
                else
                {
                    _messaging.SendNotification("cwln3Z2MWFo:APA91bHejttas_XngT6GydOXcFYXsywgeYJTJAtv-_7WMBsSMSNEsKu3j3obuiRpXtwADo5i3ViyX76rAPFDJXd3v4P8BA1aW2mvBSxoTGrbBvx4EIdnBCNsauorC6zrTFf0YSSZ3oJg", "Alerta de Medicion", message);

                }

            }
        }
    }
}
