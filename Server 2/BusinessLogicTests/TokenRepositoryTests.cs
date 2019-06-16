using CheeseIT.BusinessLogic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicTests
{
    class TokenRepositoryTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestSingleton()
        {
            TokenRepository instance1 = TokenRepository.GetInstance();
            TokenRepository instance2 = TokenRepository.GetInstance();

            Assert.AreSame(instance1, instance2);

            instance1.FirebaseToken = "testToken";

            Assert.AreEqual(instance2.FirebaseToken, "testToken");
        }
    }
}
