using Backend.Configurations;
using Backend.DataModels;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.IntegrationTests
{

    public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>,IAsyncLifetime
    {
        private  DockerClient _dockerClient;
        private  string _containerId;
        private  string _connectionString;
        private static int _containerCounter = 0;
        public static object obj = new object();

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
                    //config.UseSqlite(Configuration.GetConnectionString("testdb"), opt =>
                    //{
                    //    opt.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).FullName);
                    //});
                    
                    config.UseSqlServer(_connectionString, opt =>
                    {
                        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).FullName);
                        opt.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(8), errorNumbersToAdd: null);
                    });
                });

                services.Configure<SeederSettings>(Configuration.GetSection(nameof(SeederSettings)));
            });

            builder.UseTestServer();
        }

        public async Task InitializeAsync()
        {
            lock (obj)
            {
                _containerCounter += 1;
  
            _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            var createImageParameters = new ImagesCreateParameters
            {
                FromImage = "mcr.microsoft.com/mssql/server",
                Tag = "2019-latest"
            };
             _dockerClient.Images.CreateImageAsync(createImageParameters, null, new Progress<JSONMessage>()).Wait();

            var createContainerParameters = new CreateContainerParameters
            {
                Image = "mcr.microsoft.com/mssql/server:2019-latest",
                Name = $"DriverSqTestContainer_{_containerCounter}",
                Env = new List<string>
                {
                    "ACCEPT_EULA=Y",
                    "SA_PASSWORD=123",
                    "MSSQL_PID=Express"
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            "1433/tcp",
                            new List<PortBinding> { new PortBinding {HostIP="0.0.0.0", HostPort = "0" } }
                        }
                    }
                }
            };
            var response =  _dockerClient.Containers.CreateContainerAsync(createContainerParameters).Result;
            _containerId = response.ID;

             _dockerClient.Containers.StartContainerAsync(_containerId, null).Wait();


                _connectionString = $"Server=localhost;Database=DriverTest_{_containerCounter};User Id=sa;Password=123;";
        }
        }



        async Task IAsyncLifetime.DisposeAsync()
        {
            // Stop and remove the container
            if (!string.IsNullOrEmpty(_containerId))
            {
                await _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters());
                await _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters { Force = true });
            }

            _dockerClient.Dispose();
        }

      
    }
}
