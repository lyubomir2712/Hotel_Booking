// using System.Collections.Generic;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Hosting;
// using Xunit;
//
// namespace HotelBooking.Tests;
//
// public class WebTests
// {
//     [Fact]
//     public async Task HomePageShouldContainDevelopmentHeading()
//     {
//         var webApplicationFactory = new WebApplicationFactory<global::Program>()
//             .WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureAppConfiguration((context, config) =>
//                 {
//                     var overrides = new Dictionary<string, string?>
//                     {
//                         ["RapidApi:Key"] = "test-key"
//                     };
//                     config.AddInMemoryCollection(overrides);
//                 });
//             });
//         HttpClient client = webApplicationFactory.CreateClient();
//
//         var response = await client.GetAsync("/");
//         response.EnsureSuccessStatusCode();
//         var html = await response.Content.ReadAsStringAsync();
//
//         Assert.Contains("<div>Children</div>", html);
//         Assert.True(response.Headers.Contains("x-info-action-name"));
//     }
// }
