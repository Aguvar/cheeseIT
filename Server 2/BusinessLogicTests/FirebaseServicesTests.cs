using CheeseIT.BusinessLogic;
using FirebaseAdmin.Messaging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicTests
{
    class FirebaseServicesTests
    {
        private MobileMessagingService _messaging;

        [SetUp]
        public void Setup()
        {
            _messaging = new MobileMessagingService();
        }

        [Test]
        public void TestSendNotification()
        {
            Assert.DoesNotThrow( () => { _messaging.SendNotification("", "Test Notification", "Test Body").Wait(); });
        }

    }
}
