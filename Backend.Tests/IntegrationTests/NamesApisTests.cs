using Backend.DataModels;
using Backend.DtoModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public sealed class NamesApisTests : BaseApiTests
    {
        public NamesApisTests(IntegrationTestsWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create10RandomNames_WhenValidData_Should_ReturnOkResult()
        {
            //arrange
            var random10names = new InsertRandomNamesReq
            {
                Names=new List<string>
                {
                    "ahemd ali",
                    "samy ali",
                    "ahemd kamal",
                    "waleed ali",
                    "ahemd omar",
                    "ashry ali",
                    "kaarm ahmed",
                    "fouaad sss",
                    "shaban vbv",
                    "kkk ammar",
                }
            };

            var client = _server.CreateClient();

            //act
            var response = await client.PostAsJsonAsync($"api/NamesProcessing", random10names);
            response.EnsureSuccessStatusCode();

            //assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            response.Headers.Location.ShouldNotBeNull();

            response = await client.GetAsync(response.Headers.Location);
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            var driver = JsonConvert.DeserializeObject<List<NameIdentifier>>(str);

            driver.ShouldNotBeNull().Count.ShouldBe(10);
        }

        [Fact]
        public async Task WhenFetchAllNames_Should_Returns_10Names()
        {
            //arrange
            var client = _server.CreateClient();

            //act
            var response = await client.GetAsync("api/NamesProcessing");
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<NameIdentifier>>(str);

            //assert
            data.ShouldNotBeNull().Count.ShouldBe(10);
        }
    }
}
