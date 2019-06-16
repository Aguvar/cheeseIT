using CheeseIT.BusinessLogic;
using CheeseIT.BusinessLogic.Interfaces;
using CheeseIT.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicTests
{
    class RipeningServicesTests
    {
        private RipeningServices _services;
        private CheeseContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CheeseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var testContext = new CheeseContext(options);
            var messagingMock = new Mock<IMobileMessagingService>();
            messagingMock.Setup(messaging => messaging.SendNotification("", "", "")).Returns(new Task(() => { Console.WriteLine("Mensaje Enviado"); }));
            _services = new RipeningServices(testContext, messagingMock.Object);
            _context = testContext;
        }

        [Test]
        public void TestCreateRipening()
        {
            Cheese cheese = new Cheese()
            {
                Id = Guid.NewGuid()
            };
            _context.Cheeses.Add(cheese);

            Ripening ripening = _services.CreateRipening(cheese.Id);

            Assert.AreEqual(ripening.Cheese, cheese);
        }

        [Test]
        public void TestFinishRipening()
        {
            Cheese cheese = new Cheese()
            {
                Id = Guid.NewGuid()
            };
            _context.Cheeses.Add(cheese);

            Ripening ripening = _services.CreateRipening(cheese.Id);

            _context.Ripenings.Add(ripening);

            _services.FinishRipening(ripening.Id);

            Ripening updatedRipening = _context.Ripenings.Find(ripening.Id);

            Assert.IsNotNull(updatedRipening.EndTime);
        }

        [Test]
        public void TestGetCurrentRipening()
        {
            Cheese cheese = new Cheese()
            {
                Id = Guid.NewGuid()
            };
            _context.Cheeses.Add(cheese);

            Ripening ripening = _services.CreateRipening(cheese.Id);

            _context.Ripenings.Add(ripening);
            _context.SaveChanges();

            Ripening currentRipening = _services.GetCurrentRipening().Result;

            Assert.AreEqual(currentRipening.Id, ripening.Id);
        }

        [Test]
        public void TestValidateMeasure()
        {
            Cheese cheese = new Cheese()
            {
                Id = Guid.NewGuid(),
                IdealHumidity = 50,
                IdealTemperature = 50,
                HumidityThreshold = 3,
                TemperatureThreshold = 3
            };
            _context.Cheeses.Add(cheese);

            Ripening ripening = _services.CreateRipening(cheese.Id);

            _context.Ripenings.Add(ripening);
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
