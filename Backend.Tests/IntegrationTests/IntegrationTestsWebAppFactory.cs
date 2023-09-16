using Backend.Configurations;
using Backend.DataModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

namespace Backend.Tests.IntegrationTests
{
    //public abstract class IntegrationTestsWebAppFactory: IClassFixture<WebApplicationFactory<TestStartup>>
    //{
    //    protected readonly WebApplicationFactory<TestStartup> _factory;

    //    public IntegrationTestsWebAppFactory(WebApplicationFactory<TestStartup> factory)
    //    {
    //        _factory = factory;
    //    }
    //}

    public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.tests.json")  // Adjust the path if needed
               .Build();

                var ovriddenDescriptors = services.Where(
                    x => x.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                         x.ServiceType == typeof(IConfigureOptions<SeederSettings>))
                .ToList();

                foreach (var descriptor in ovriddenDescriptors)
                {
                    services.Remove(descriptor);
                }


                services.AddDbContext<AppDbContext>(config =>
                {
                    config.UseSqlite(Configuration.GetConnectionString("testdb"), opt =>
                    {
                        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).FullName);
                    });
                });

                services.Configure<SeederSettings>(Configuration.GetSection(nameof(SeederSettings)));
            });

            builder.UseTestServer();
        }
    }
}
