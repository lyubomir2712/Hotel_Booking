using HotelBooking.Services.Contracts;
using HotelBooking.Services.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelBooking.Services.ApiModule
{
    public class ApiService : IApiService
    {
        public async Task<List<Location>?> GetHotelsByLocation(string apiUrl,ApiDataViewModel model)
        {
            //var queryString = HttpUtility.ParseQueryString(string.Empty);
            //queryString["name"] = model.City;
            //queryString["locale"] = model.Locale;
            //var urlBuilder = new UriBuilder(apiUrl);
            //urlBuilder.Query = queryString.ToString();

            //urlBuilder.Query

            //var response = await apiConnectionService.ApiConnection.GetAsync(urlBuilder.Uri);
            //response.EnsureSuccessStatusCode();
            //var responseJson = await response.Content.ReadAsStringAsync();
            //var hotels = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);

            //return hotels;


            var client = new HttpClient();
            

            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "booking-com.p.rapidapi.com");
            


            //var body = new StringContent("{{\"post\":{{\"contact\":{{\"isActive\":true,\"phone\":\"99999999\"}}", Encoding.UTF8, "text/plain");

            var response = await client.GetAsync(apiUrl + $"?name={model.City}&locale={model.Locale}");

            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<List<Location>>(responseJson);

            return hotels;

        }

    }
}
