using Backend;
using Backend.Configurations;
using Backend.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackTest.Tests.SeedingTests
{
    /// <summary>
    /// tests to make sure the behavior of data seeder works properly at the start of the application
    /// </summary>
    public class DataSeedsTests:IDisposable
    {
        private DbContextOptions<AppDbContext> _options;
        private AppDbContext _context;
        private readonly IServiceProvider _sp;
        IOptions<SeederSettings> _settings;
        public DataSeedsTests()
        {
            // Initialize the in-memory database
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSeedsDatabase")
                .Options;

            // Create a new context for each test
            _context = new AppDbContext(_options);

            //create new DI for registering context & settings
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<AppDbContext>(sp => _context);
            serviceCollection.Configure<SeederSettings>(o=>o.EnableSeeding=true);

            _sp = serviceCollection.BuildServiceProvider();
             _settings= _sp.GetRequiredService<IOptions<SeederSettings>>();  
        }

        public void Dispose()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task SeedDefaultDataAsync_Should_Seed10DriversRecordsInDatabase()
        {
            // Arrange
            var seeder = new DataSeederManager(_sp, _settings); 

            // Act
            await seeder.CleanDataAsync();
            await seeder.Seed10RandomDrivers();
            // Assert
            var createdDrivers = await _context.Drivers.ToListAsync();

            createdDrivers.ShouldNotBeNull();
            createdDrivers.Count.ShouldBe(10);
        }

        [Fact]
        public async Task CleanSeededDataAsync_Should_CleanTheDataInDatabase()
        {
            // Arrange
            var seeder = new DataSeederManager(_sp, _settings);

            // Act
            await seeder.CleanDataAsync();
            await seeder.Seed10RandomDrivers();
            await seeder.CleanDataAsync();
            // Assert
            var createdDrivers = await _context.Drivers.ToListAsync();

            createdDrivers.Count.ShouldBe(0);
        }
    }
}
