using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic
{
    public class ExperimentServices : IExperimentServices
    {
        private readonly CheeseContext _context;
        private readonly IMobileMessagingService _messaging;

        public ExperimentServices(CheeseContext context, IMobileMessagingService mobileMessagingService)
        {
            _context = context;
            _messaging = mobileMessagingService;
        }

        public async Task<Experiment> GetCurrentExperiment()
        {
            return await _context.Experiments.Include(exp => exp.Measurements).Include(rip => rip.Observations).Where(r => r.EndTime == null).FirstOrDefaultAsync();
        }

        public void ValidateMeasure(Measurement measure)
        {

            Experiment currentExperiment = GetCurrentExperiment().Result;

            string message = "";

            if (Math.Abs(measure.Humidity - currentExperiment.IdealHumidity) > currentExperiment.HumidityThreshold)
            {
                message += $"Se ha registrado una humedad {(measure.Humidity - currentExperiment.IdealHumidity) - currentExperiment.HumidityThreshold}% por fuera del rango establecido. Para el experimento actual\n";
            }

            if (Math.Abs(measure.Temperature - currentExperiment.IdealTemperature) > currentExperiment.TemperatureThreshold)
            {
                message += $"Se ha registrado una temperatura {(measure.Temperature - currentExperiment.IdealTemperature) - currentExperiment.TemperatureThreshold}°C por fuera del rango establecido. Para el experimento actual. \n";
            }

            if (!string.IsNullOrEmpty(message))
            {
                string token = TokenRepository.GetInstance().FirebaseToken;

                _messaging.SendNotification(token, "Alerta de Medicion", message);
            }
        }

    }
}
