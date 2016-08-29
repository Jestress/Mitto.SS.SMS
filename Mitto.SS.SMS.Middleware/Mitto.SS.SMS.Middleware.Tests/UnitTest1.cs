using System;
using NUnit.Framework;
using Mitto.SS.SMS.Middleware.ServiceInterface;
using Mitto.SS.SMS.Middleware.ServiceModel;
using ServiceStack.Testing;
using ServiceStack;

namespace Mitto.SS.SMS.Middleware.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(ShortMessageServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            var service = appHost.Container.Resolve<ShortMessageServices>();

            //var response = (GetCountriesResponse)service.Any(new Operations { Name = "World" });

            //Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }
    }
}
