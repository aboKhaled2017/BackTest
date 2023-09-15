using Backend.Controllers;
using Backend.DataModels;
using Backend.DtoModels;
using Backend.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace BackTest.Tests.DriverControllerTests
{
    /// <summary>
    /// test Driver controller actions (just 3 of the most important actions)
    /// </summary>
    public class DriverControllerTests
    {
        private Mock<IDriverRepo> _mockDriverRepo;
        private Mock<ILogger<DriverController>> _mockLogger;
        private DriverController driverController;

        public DriverControllerTests()
        {
            _mockDriverRepo = new Mock<IDriverRepo>();
            _mockLogger = new Mock<ILogger<DriverController>>();
            driverController = new DriverController(_mockLogger.Object, _mockDriverRepo.Object);
        }

        [Fact]
        public async Task GetDriverById_NotValidId_ReturnsBadResuest()
        {
            // Arrange
            int driverId = 5;

            _mockDriverRepo.Setup(repo => repo.GetDriverByIdAsync(driverId)).ReturnsAsync(null as Driver);

            // Act
            var result = await driverController.GetDriverById(driverId);

            // Assert

            var notFoundRequestResult = result.ShouldBeOfType<NotFoundObjectResult>();
            var message = notFoundRequestResult.Value.ShouldBeOfType<string>();
            message.ShouldBe($"cannot find driver as database with id {driverId}");
        }

        [Fact]
        public async Task GetDriverById_ValidId_ReturnsDriver()
        {
            // Arrange
            int driverId = 1;
            var driver = Driver.Create("ahmed", "ali", "test@t.t", "201152506434");
            driver.Id = 1;

            _mockDriverRepo.Setup(repo => repo.GetDriverByIdAsync(driverId)).ReturnsAsync(driver);

            // Act
            var result = await driverController.GetDriverById(driverId);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDriver = Assert.IsType<Driver>(okResult.Value);
            driverId.ShouldBe(returnedDriver.Id);
            driver.ShouldBe(returnedDriver);
        }


        [Fact]
        public async Task CreateDriver_NotValidData_ShouldFails()
        {
            // Arrange
            int driverId = 1;
            var driver = Driver.Create("ahmed", "ali", "test@t.t", "201152506434");
            driver.Id = 1;

            var createDriverReq = new CreateDriverReq(driver.FirstName, driver.LastName, driver.Email, driver.PhoneNumber);

            _mockDriverRepo.Setup(repo => repo.CreateDriverAsync(It.IsAny<Driver>())).Callback((Driver driver) =>
            {
                driver.Id = driverId;
            });

            // Act
            var result = await driverController.AddNewDriver(createDriverReq);

            // Assert

            var createAtActionResult = result.ShouldBeOfType<CreatedAtActionResult>();
            var returnedDriver = createAtActionResult.Value.ShouldBeOfType<Driver>();
            createAtActionResult.ActionName.ShouldBe(nameof(DriverController.GetDriverById));
            returnedDriver.ShouldNotBeNull();
        }
    }
}