using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts;
using HotelBooking.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.HotelAddService
{

    public class HotelAddService
    {
        private readonly BookingDbContext _bookingDbContext;
        public HotelAddService(BookingDbContext bookingDbContext) 
        {
            _bookingDbContext = bookingDbContext;
        }

        public List<BookingModel> GetBookedHotels(string userId)
        {
            return _bookingDbContext.UserBookings
                .Where(b => b.UserId == Convert.ToInt32(userId))
                .Select(b => b.BookingModel)
                .ToList();
                
        }

        public List<HotelModel> GetHotels(List<BookingModel> bookingModel)
        {

            return _bookingDbContext.Hotels.Where(b => bookingModel.Select(a => a.Id).Contains(b.Id)).ToList();

        }


    }
}
