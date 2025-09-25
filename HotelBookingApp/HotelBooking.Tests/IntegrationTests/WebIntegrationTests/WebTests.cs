using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HotelBooking.Tests.IntegrationTests.WebIntegrationTests
{
    public class WebTests
    {
        private static WebApplicationFactory<global::Program> CreateFactory()
        {
            return new WebApplicationFactory<global::Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        var overrides = new Dictionary<string, string?>
                        {
                            ["RapidApi:Key"] = "test-key"
                        };
                        config.AddInMemoryCollection(overrides);
                    });
                });
        }

        [Fact]
        public async Task HomePage_ShouldContainTitle()
        {
            using var factory = CreateFactory();
            using HttpClient client = factory.CreateClient();

            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("EasyBook", html);
        }

        [Fact]
        public void Configuration_ShouldOverride_RapidApiKey()
        {
            using var factory = CreateFactory();
            var configuration = factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;

            Assert.NotNull(configuration);
            Assert.Equal("test-key", configuration!["RapidApi:Key"]);
        }

        [Fact]
        public async Task NonExistingRoute_ShouldReturn_NotFound()
        {
            using var factory = CreateFactory();
            using HttpClient client = factory.CreateClient();

            var response = await client.GetAsync("/definitely-not-a-real-route-404");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}