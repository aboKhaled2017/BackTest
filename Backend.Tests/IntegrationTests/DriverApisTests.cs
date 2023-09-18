using Backend.DataModels;
using Backend.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public sealed class DriverApisTests : BaseApiTests
    {
        public DriverApisTests(IntegrationTestsWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task OnApplicationStarts_WhenFetchAllDrivers_Should_Returns_10Drivers()
        {
            //arrange
            var client = _server.CreateClient();

            //act
            var response = await client.GetAsync("api/driver/getall");
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Driver>>(str);

            //assert
            data.ShouldNotBeNull().Count.ShouldBe(10);
        }

        [Fact]
        public async Task GetDriverById_WhenValidId_Should_ReturnDriver()
        {
            //arrange
            int driverId = 1;
            var client = _server.CreateClient();

            //act
            var response = await client.GetAsync($"api/driver/{driverId}");
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            var driver = JsonConvert.DeserializeObject<Driver>(str);

            //assert
            driver.ShouldNotBeNull().Id.ShouldBe(driverId);
        }

        [Fact]
        public async Task GetDriverById_WhenNotValidId_Should_ReturnDriver()
        {
            //arrange
            int driverId = 0;
            var client = _server.CreateClient();

            //act
            var response = await client.GetAsync($"api/driver/{driverId}");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateDriver_WhenValidData_Should_ReturnOkResult()
        {
            //arrange
            var driverCreateReq=new CreateDriverReq("ahmed", "ali", "test@tt.tt", "201152506434");
            var client = _server.CreateClient();

            //act
            var response = await client.PostAsJsonAsync($"api/driver",driverCreateReq);
            response.EnsureSuccessStatusCode();

            //assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            response.Headers.Location.ShouldNotBeNull();

            response = await client.GetAsync(response.Headers.Location);
            response.EnsureSuccessStatusCode() ;
            var str=await response.Content.ReadAsStringAsync(); 
            var driver=JsonConvert.DeserializeObject<Driver>(str);
            
            driver.ShouldNotBe(null);
            driver.ShouldSatisfyAllConditions(x =>
            {
                x.FirstName.ShouldBe(driverCreateReq.firstName);
                x.LastName.ShouldBe(driverCreateReq.lastname);
                x.Email.ShouldBe(driverCreateReq.email);
                x.PhoneNumber.ShouldBe(driverCreateReq.phoneNumber);
            });      
        }

        [Theory]
        [InlineData("", "ali", "test@tt.tt", "201152506434","firstName")]
        [InlineData("ahmed", "a", "test@tt.tt", "201152506434","lastname")]
        [InlineData("ahmed", "ali", "test@.tt", "201152506434","email")]
        [InlineData("ahmed", "ali", "test@t.tt", "20115250643","phoneNumber")]
        public async Task CreateDriver_WhenNotValidData_Should_ReturnBadequestResult(string fname,string lname,string email,string phone,string expectedPropertyError)
        {
            //arrange
            var driverCreateReq = new CreateDriverReq(fname,lname,email,phone);
            var client = _server.CreateClient();

            //act
            var response = await client.PostAsJsonAsync($"api/driver", driverCreateReq);

            //assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

            var str = await response.Content.ReadAsStringAsync();
            var validationproblem = JsonConvert.DeserializeObject<ValidationProblemDetails>(str);

            validationproblem.ShouldSatisfyAllConditions(x => {
                x.ShouldNotBeNull();
                x.Errors.Count.ShouldBeGreaterThan(0);
                x.Errors.Keys.ShouldContain(expectedPropertyError);
            });
        }


        [Fact]
        public async Task DeleteDriverById_WhenValidId_Should_ReturnOkResult()
        {
            //arrange
            var driverCreateReq = new CreateDriverReq("ahmed", "ali", "test@tt.tt", "201152506434");
            var client = _server.CreateClient();
            var res=await client.PostAsJsonAsync($"api/driver", driverCreateReq);
            var driverStr=await res.Content.ReadAsStringAsync();
            var createdDriver=JsonConvert.DeserializeObject<Driver>(driverStr);

            //act
            var response = await client.DeleteAsync($"api/driver/{createdDriver.Id}");
            response.EnsureSuccessStatusCode();

            //assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteDriverById_WhenNotValidId_Should_ReturnNotFoundResult()
        {
            //arrange
            var client = _server.CreateClient();

            //act
            var response = await client.DeleteAsync($"api/driver/1");
            response = await client.DeleteAsync($"api/driver/1");

            //assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateDriver_WhenValid_Should_ReturnOkResult()
        {
            //arrange
            var driverCreateReq = new CreateDriverReq("ahmed", "ali", "test@tt.tt", "201152506434");
            var updatedFName = "ahmdup";
            var client = _server.CreateClient();
            var res = await client.PostAsJsonAsync($"api/driver", driverCreateReq);
            res.EnsureSuccessStatusCode();
            var driverStr = await res.Content.ReadAsStringAsync();
            var expectedDriver = JsonConvert.DeserializeObject<Driver>(driverStr);

            var driverUpdateReq = new UpdateDriverReq(updatedFName, driverCreateReq.lastname, driverCreateReq.email, driverCreateReq.phoneNumber);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(driverUpdateReq), Encoding.UTF8, "application/json"); 
            //act
            var response = await client.SendAsync(new HttpRequestMessage {RequestUri= new System.Uri($"api/driver/{expectedDriver.Id}",System.UriKind.Relative), Method=HttpMethod.Put,Content=content});
            response.EnsureSuccessStatusCode();

            //assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            response = await client.GetAsync($"api/driver/{expectedDriver.Id}");
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            var actualDriver = JsonConvert.DeserializeObject<Driver>(str);

            actualDriver.ShouldNotBe(null);
            actualDriver.Id.ShouldBe(expectedDriver.Id);
            actualDriver.FirstName.ShouldBe(updatedFName);
        }
    }
}
