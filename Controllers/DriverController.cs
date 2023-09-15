using BackTest.DataModels;
using BackTest.DtoModels;
using BackTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<DriverController> _logger;
        public DriverController(ILogger<DriverController> logger, IDriverService driverService)
        {
            _logger = logger;
            _driverService = driverService;
        }

        /// <summary>
        /// Get a driver by ID.
        /// </summary>
        /// <param name="id">The ID of the driver to retrieve.</param>
        /// <returns>The driver with the specified ID.</returns>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(typeof(Driver))]
        [Tags("Get driver by Id")]
        public async Task<IActionResult> GetDriverById([FromRoute] int id)
        {
            if (id == default)
                return NotFound("not valid id");

            _logger.LogInformation($"trying to fetch a driver with id {id}");

            var driver = await _driverService.GetDriverByIdAsync(id);

            if (driver is null)
                return NotFound($"cannot find driver as database with id {id}");
            
            _logger.LogInformation($"a driver with id {id} retreived successully");

            return Ok(driver);
        }

        /// <summary>
        /// Get a driver by ID.
        /// </summary>
        /// <param name="id">The ID of the driver to retrieve.</param>
        /// <returns>The driver with the specified ID.</returns>
        [HttpGet]
        [Route("getall")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(List<Driver>))]
        [Tags("Get all drivers")]
        public async Task<IActionResult> GetAllDriver()
        {
            _logger.LogInformation($"trying to fetch all the drivers");

            var drivers = await _driverService.GetAllDriversAsync();

            _logger.LogInformation($"a {drivers.Count} drivers list retreived successully from database");

            return Ok(drivers);
        }

        /// <summary>
        /// create a new driver
        /// </summary>
        /// <param name="req">request object which contains driver details</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Create a new driver")]
        public async Task<IActionResult> AddNewDriver([FromBody] CreateDriverReq req)
        {
            if (req is null)
                return BadRequest("Invalid request data");

            _logger.LogInformation($"trying to create a driver with {JsonConvert.SerializeObject(req)}");
           
            var driver = Driver.Create(req.firstName, req.lastname, req.email, req.phoneNumber);
           
            await _driverService.CreateDriverAsync(driver);

            _logger.LogInformation($"a new driver with id {driver.Id} created successully");

            return CreatedAtAction(nameof(GetDriverById), new { id = driver.Id }, driver);
        }

        /// <summary>
        /// update a  driver
        /// </summary>
        /// <param name="req">request object which contains driver details</param>
        /// <param name="id">a driver id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(typeof(Driver))]
        [Tags("Update a driver")]
        public async Task<IActionResult> UpdateNewDriver([FromRoute] int id,[FromBody] UpdateDriverReq req)
        {
            if (req is null || id ==default)
                return BadRequest("Invalid request data");

            _logger.LogInformation($"trying to update a driver with id {id} with {JsonConvert.SerializeObject(req)}");

            var driver = await _driverService.GetDriverByIdAsync(id);

            if (driver is null)
                return NotFound($"cannot find driver as database with id {id}");
            
            req.BindTo(driver);

            await _driverService.UpdateDriverAsync(driver);

            _logger.LogInformation($"the driver with id {driver.Id} update successully");

            return Ok();
        }

        /// <summary>
        /// Delete a driver by ID.
        /// </summary>
        /// <param name="id">The ID of the driver to delete.</param>
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Delete driver by Id")]
        public async Task<IActionResult> DeleteDriverById([FromRoute] int id)
        {
            if (id == default)
                return NotFound("not valid id");

            _logger.LogInformation($"trying to delete a driver with id {id}");

            var driver = await _driverService.GetDriverByIdAsync(id);

            if (driver is null)
                return NotFound($"cannot find driver as database with id {id}");

            await _driverService.DeleteDriverAsync(driver);

            _logger.LogInformation($"the driver with id {driver.Id} delete successully");

            return Ok();
        }
    }
}
