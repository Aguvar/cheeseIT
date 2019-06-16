using CheeseIT.BusinessLogic;
using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicTests
{
    class ExperimentServicesTests
    {
        private ExperimentServices _services;
        private CheeseContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CheeseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var testContext = new CheeseContext(options);
            var messagingMock = new Mock<IMobileMessagingService>();
            messagingMock.Setup(messaging => messaging.SendNotification("", "", "")).Returns(new Task(() => { Console.WriteLine("Mensaje Enviado"); }));
            _services = new ExperimentServices(testContext, messagingMock.Object);
            _context = testContext;
        }

        [Test]
        public void TestGetCurrentExperiment()
        {
            Experiment experiment = new Experiment()
            {
                Id = Guid.NewGuid()
            };

            _context.Experiments.Add(experiment);
            _context.SaveChanges();

            Experiment currentExperiment = _services.GetCurrentExperiment().Result;

            Assert.AreEqual(currentExperiment.Id, experiment.Id);
        }

        [Test]
        public void TestValidateMeasure()
        {
            Experiment experiment = new Experiment()
            {
                Id = Guid.NewGuid(),
                IdealHumidity = 50,
                IdealTemperature = 50,
                HumidityThreshold = 3,
                TemperatureThreshold = 3
            };

            _context.Experiments.Add(experiment);
            _context.SaveChanges();

            Measurement measure = new Measurement()
            {
                Humidity = 60,
                Temperature = 60
            };

            Assert.DoesNotThrow(() => { _services.ValidateMeasure(measure); }, "Measurement Validation Failed");

        }
    }
}
