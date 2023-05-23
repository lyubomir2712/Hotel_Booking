using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    public class ApiDataViewModel
    {
        public ApiDataViewModel()
        {

        }
        public ApiDataViewModel(string locale= "en-gb")
        {
            Locale = locale;
        }
        public string City { get; set; }
        public string Locale { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public decimal MinPrice { get; set; } = 50;
        public decimal MaxPrice { get; set; } = 800;
        public bool? HasPool { get; set; }
        public bool? HasParking { get; set; }
        public bool? HasFitness { get; set; }
        public bool? HasInternet { get; set; }
        public bool? HasRestaurant { get; set; }
    }
}
