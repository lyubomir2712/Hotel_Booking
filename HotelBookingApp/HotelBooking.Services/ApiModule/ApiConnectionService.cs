using HotelBooking.Services.Contracts;
using HotelBooking.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelBooking.Services.ApiModule
{
    internal class ApiConnectionService : IApiConnectionService
    {
        private readonly ApiConnectionViewModel model;

        public ApiConnectionService(ApiConnectionViewModel model)
        {
            this.model = model;
        }
        public HttpClient ApiConnection { get;private set; }
        
        public void EstablisConnection()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-rapidapi-host", model.DomainName);
            client.DefaultRequestHeaders.Add("x-rapidapi-key", model.ApiKey);

            ApiConnection= client;
        }
    }
}
