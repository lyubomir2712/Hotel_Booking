using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.HotelsServices
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
                .Include(b => b.BookingModel)
                    .ThenInclude(b => b.HotelModel)
                .Select(b => b.BookingModel)
                .ToList();
        }

        public List<HotelModel> GetHotels(List<BookingModel> bookingModel)
        {
            return _bookingDbContext.Hotels.Where(b => bookingModel.Select(a => a.Id).Contains(b.Id)).ToList();
        }


    }
}
