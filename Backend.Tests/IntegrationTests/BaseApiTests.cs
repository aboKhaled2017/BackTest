using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public abstract class BaseApiTests : IClassFixture<IntegrationTestsWebAppFactory>
    {
        protected readonly IServiceScope _serviceScope;
        protected readonly TestServer _server;
        public BaseApiTests(WebApplicationFactory<Program> factory)
        {
            //factory = factory.WithWebHostBuilder(builder =>
            //{
            //    builder.ConfigureAppConfiguration((context, config) =>
            //    {
            //        config.AddJsonFile("appsettings.tests.json", optional: true);
            //    });
            //});
            _serviceScope = factory.Services.CreateScope();
            _server = factory.Server;
        }
    }
}
