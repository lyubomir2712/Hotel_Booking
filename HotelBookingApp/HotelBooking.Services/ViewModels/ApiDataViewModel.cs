using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    public class ApiDataViewModel
    {
        
        public string City { get; set; }
        public string Locale { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = 800;
        public bool HasPool { get; set; }
        public bool HasParking { get; set; }
        public bool HasBreakfast { get; set; }
    }
}
