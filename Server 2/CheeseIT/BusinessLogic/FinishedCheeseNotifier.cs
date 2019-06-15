using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic
{
    public class FinishedCheeseNotifier : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IMobileMessagingService _messaging;
        private readonly IServiceScopeFactory _scopeFactory;


        public FinishedCheeseNotifier(IServiceScopeFactory scopeFactory, IMobileMessagingService messaging)
        {
            //_ripenings = ripenings;
            //_experiments = experiments;
            _scopeFactory = scopeFactory;
            _messaging = messaging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine("Timed Background Service is working.");
            Console.WriteLine("Estoy corriendo cada 5 secs");
            using (var scope = _scopeFactory.CreateScope())
            {
                IRipeningServices ripeningServices = scope.ServiceProvider.GetRequiredService<IRipeningServices>();
                IExperimentServices experimentServices = scope.ServiceProvider.GetRequiredService<IExperimentServices>();

                Ripening ripening = ripeningServices.GetCurrentRipening().Result;
                Experiment experiment = experimentServices.GetCurrentExperiment().Result;

                string message = "";

                if (ripening != null)
                {
                    DateTime ripeDay = DateTime.Now.AddDays(ripening.Cheese.DaysToRipe);
                    if (ripeDay < DateTime.Now)
                    {
                        message += "¡Tu maduracion esta lista!";
                    }
                }

                if (experiment != null && experiment.EstimatedEndTime < DateTime.Now)
                {
                    message += "¡Tu experimento esta listo!";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    string token = TokenRepository.GetInstance().FirebaseToken;
                    _messaging.SendNotification(token, "Maduracion Terminada", message);
                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
