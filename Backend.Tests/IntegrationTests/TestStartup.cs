using Backend.Configurations;
using Backend.DataModels;
using Backend.Repos;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Backend.Tests.IntegrationTests
{
    public class TestStartup
    {
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.tests.json")  // Adjust the path if needed
           .Build();

            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlite(Configuration.GetConnectionString("db"));
            });

            services.AddScoped<IDriverRepo, DriverRepo>();

            services.AddControllers()
                .AddFluentValidation(opt =>
                {
                    opt.AutomaticValidationEnabled = true;
                    opt.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(AppDbContext)));
                });

            services.ConfigureOptions<SeederConfigurations>();

            services.AddSingleton<DataSeederManager>();
        }

        public void Configure(WebApplication app)
        {
            app.UseMigrationsMiddleware();

            app.RunAppSeederAsync(app.Services);

            app.UseCustomeExceptionMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
