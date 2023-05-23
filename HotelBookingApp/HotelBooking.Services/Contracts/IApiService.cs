using HotelBooking.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.Contracts
{
    public interface IApiService
    {
        public Task<Dictionary<string, string>> GetHotelsByLocation(string apiUrl,ApiDataViewModel model);
    }
}
