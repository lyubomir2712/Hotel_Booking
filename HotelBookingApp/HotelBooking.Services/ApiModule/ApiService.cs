using HotelBooking.Services.Contracts;
using HotelBooking.Services.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelBooking.Services.ApiModule
{
    public class ApiService : IApiServices
    {
        private readonly IApiConnectionService apiConnectionService;

        public ApiService(IApiConnectionService apiConnectionService)
        {
            this.apiConnectionService = apiConnectionService;
        }
        public async Task<Dictionary<string, string>> GetHotelsByLocation(string apiUrl,ApiDataViewModel model)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["name"] = model.City;
            queryString["locale"] = model.Locale;
            var urlBuilder = new UriBuilder(apiUrl);
            urlBuilder.Query = queryString.ToString();


            var response = await apiConnectionService.ApiConnection.GetAsync(urlBuilder.Uri);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);
          
            return hotels;

        }
        
    }
}
