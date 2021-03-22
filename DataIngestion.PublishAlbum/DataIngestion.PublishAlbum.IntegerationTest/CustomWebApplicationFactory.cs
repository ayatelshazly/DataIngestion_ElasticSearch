using Broker;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.IO;

namespace DataIngestion.PublishAlbum.IntegerationTest
{
    public  class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public  string validDataTestFilesPath = Path.GetFullPath(@"../../../ValidDataTestFiles/");
        public  string inValidDataTestFilesPath = Path.GetFullPath(@"../../../InvalidDataTestFiles/");

        public WebApplicationFactory<TStartup> ConfigureTest()
        {
            return this.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                {
                    var eventBus = new Mock<IEventBus>(MockBehavior.Strict);
                    eventBus.Setup(c => c.Broker).Returns("fake-broker");
                    eventBus.Setup(c => c.DaprPort).Returns("invalid-port-number");
                    eventBus.Setup(c => c.EventsOn).Returns(true);
                    eventBus.Setup(c => c.Publish(It.IsAny<object>(), It.IsAny<string>())).ReturnsAsync(true);

                    var descriptorEventBus =
                    new ServiceDescriptor(typeof(IEventBus), p => eventBus.Object, ServiceLifetime.Transient);
                    services.Replace(descriptorEventBus);
 
                })
            );


        }
    }
}
