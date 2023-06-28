using HotelBooking.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    public class UserBookedHotels
    {
        public List<HotelModel> Hotels { get; set; }
        public List<BookingModel> Bookings { get; set; }
    }
}
