using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    public class BookedHotel
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string hotelImg { get; set; }
        public string HotelPrice { get; set;}
        public string StartAt { get; set;}
        public string EndAt { get; set;}
    }
}
