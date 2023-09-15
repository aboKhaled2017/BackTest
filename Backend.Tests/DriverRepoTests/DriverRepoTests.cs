using Backend.DataModels;
using Backend.Repos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BackTest.Tests.DriverRepoTests
{
    /// <summary>
    /// tests for making sure that Driver repo works properly
    /// </summary>
    public class DriverRepoTests : IDisposable
    {
        private DbContextOptions<AppDbContext> _options;
        private AppDbContext _context;

        public DriverRepoTests()
        {
            // Initialize the in-memory database
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRepoDatabase")
                .Options;

            // Create a new context for each test
            _context = new AppDbContext(_options);
        }

        public void Dispose()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Fact]
        public async Task CreateDriverAsync_Should_CreateDriverInDatabase()
        {
            // Arrange
            var repo = new DriverRepo(_context);
            var driver = Driver.Create("ahmed", "ali", "test@t.t", "201152506434");

            // Act
            await repo.CreateDriverAsync(driver);

            // Assert
            var createdDriver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driver.Id);

            createdDriver.ShouldNotBeNull();
            createdDriver.FirstName.ShouldBe(driver.FirstName);
            createdDriver.LastName.ShouldBe(driver.LastName);
            createdDriver.PhoneNumber.ShouldBe(driver.PhoneNumber);
            createdDriver.Email.ShouldBe(driver.Email);
        }


        [Fact]
        public async Task GetDriverAsync_Should_GetDriverFromTheDatabase()
        {
            // Arrange
            var repo = new DriverRepo(_context);
            var driver = Driver.Create("ahmed", "ali", "test@t.t", "201152506434");
            await repo.CreateDriverAsync(driver);
            // Act
            var fetcheddDriver = await repo.GetDriverByIdAsync(driver.Id);

            // Assert
            fetcheddDriver.ShouldNotBeNull();
            fetcheddDriver.Id.ShouldBe(driver.Id);
        }

        [Fact]
        public async Task UpdateDriverAsync_Should_UpdateDriverInTheDatabase()
        {
            // Arrange
            var repo = new DriverRepo(_context);
            var driver = Driver.Create("ahmed", "ali", "test@t.t", "201152506434");
            string updatedLastName = "ali-text";
            await repo.CreateDriverAsync(driver);
            driver.LastName = updatedLastName;
            // Act
            await repo.UpdateDriverAsync(driver);

            // Assert
            var updatedDriver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driver.Id);

            updatedDriver.ShouldNotBeNull();
            updatedDriver.LastName.ShouldBe(updatedLastName);
        }

        [Fact]
        public async Task DeleteDriverAsync_Should_DeleteTheDriverFromTheDatabase()
        {
            // Arrange
            var repo = new DriverRepo(_context);
            var driver = Driver.Create("ahmed", "ali", "test@t.t", "201152506434");
            await repo.CreateDriverAsync(driver);
            // Act
            await repo.DeleteDriverAsync(driver);
            var deletedDriver=await _context.Drivers.FirstOrDefaultAsync(x=>x.Id==driver.Id);
            // Assert
            deletedDriver.ShouldBeNull();
        }
    }
}
